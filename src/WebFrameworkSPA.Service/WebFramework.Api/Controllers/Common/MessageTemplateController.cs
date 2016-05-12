using Service;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic;
using App.Common.InversionOfControl;
using System.Text;
using BrockAllen.MembershipReboot;
using System.Web;
using System.Security.Claims;
using App.Common.SessionMessage;
using Web.Infrastructure.JqGrid;
using WebFramework.Data.Domain;
using Web.Infrastructure;
using System.IO;

namespace Web.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class MessageTemplateController : ApiController
    {
        private IMessageTemplateService _service;
        public MessageTemplateController()
        {
            _service = IoC.GetService<IMessageTemplateService>();
        }
        // GET api/MessageTemplate
        public dynamic GetGridData([FromUri] JqGridSearchModel searchModel)
        {
            var data = GetQuery(_service.Query(),searchModel);
            var dataList = data.Items.Select(x => new { x.Id, x.Name, x.BccEmailAddresses, x.Subject, x.Body, x.IsActive }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new { x.Id, x.Name, x.BccEmailAddresses, x.Subject, x.Body, x.IsActive }).ToArray()
            };
        }
        [Route("api/messagetemplate/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcel([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            string filePath = null;
            HttpResponseMessage result = null;
            try
            {
                searchModel.rows = 0;
                var data = GetQuery(Web.Infrastructure.Util.GetStatelessQuery<MessageTemplate>(), searchModel);
                var dataList = data.Items.Select(x => new { x.Name, x.BccEmailAddresses, x.Subject, x.Body, x.IsActive }).ToList();
                filePath = ExporterManager.Export("messagetemplate", ExporterType.CSV, dataList, "");
            }
            catch (Exception ex)
            {
                return Web.Infrastructure.Util.DisplayExportError(ex);
            }
            if (!File.Exists(filePath))
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(filePath);
                result.Content.Headers.ContentLength = new FileInfo(filePath).Length;

            }
            return result;
        }
        // GET api/MessageTemplate/5
        public IHttpActionResult Get(Guid id)
        {
            var item = _service.Query().FirstOrDefault((p) => p.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/MessageTemplate
        public IHttpActionResult Post([FromBody] MessageTemplate item)
        {
            StringBuilder message = new StringBuilder();
            if (item == null)
                return BadRequest("MessageTemplate cannot be empty.");
            if (string.IsNullOrEmpty(item.Name))
                return BadRequest("MessageTemplate name cannot be empty.");
            item.Name = item.Name.Trim();
            if (Constants.SHOULD_FILTER_BY_APP && !item.Name.StartsWith(App.Common.Util.ApplicationConfiguration.AppAcronym))
                return BadRequest(string.Format("Name must start with '{0}.'", App.Common.Util.ApplicationConfiguration.AppAcronym));
            if (_service.Exists(item.Name))
                return BadRequest(string.Format("Permission {0} already exists.", item.Name));
            _service.Add(item);
            message.AppendFormat("MessageTemplate {0}  is saved successflly.", item.Name);
            return Json<object>(new { Success = true, Message = message.ToString(), RowId = item.Id });
        }

        // PUT api/MessageTemplate/5
        public IHttpActionResult Put(Guid id, [FromBody] MessageTemplate item)
        {
            StringBuilder message = new StringBuilder();
            if (id == default(Guid))
                return BadRequest("MessageTemplate id cannot be empty.");
            if (item == null)
            {
                return BadRequest("MessageTemplate cannot be empty.");
            }
            if (string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("MessageTemplate name cannot be empty.");
            }
            item.Id = id;
            item.Name = item.Name.Trim();
            _service.Update(item);
            message.AppendFormat("MessageTemplate {0}  is saved successflly.", item.Name);
            return Json<object>(new { Success = true, Message = message.ToString(), RowId = item.Id });

        }

        // DELETE api/MessageTemplate/5
        public IHttpActionResult Delete(Guid id)
        {
            if (id == default(Guid))
                return BadRequest("MessageTemplate id cannot be empty.");
            if (_service.Delete(id))
                return Ok();
            else
                return NotFound();
        }
        [Route("api/messagetemplatetest/{id}")]
        public IHttpActionResult TestMessageTemplate(Guid id)
        {
            if (id == default(Guid))
                return BadRequest("MessageTemplate id cannot be empty.");
            ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
            if (principal == null)
                return NotFound();
            Claim emailClaim = principal.FindFirst(ClaimTypes.Email);
            if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                return BadRequest("Cannot find email address.");
            string to = emailClaim.Value.Trim();
            IMessageTemplateService messageTemplateServce = IoC.GetService<IMessageTemplateService>();
            MessageTemplate template = messageTemplateServce.GetById(id);
            if (template == null)
                return BadRequest(string.Format("Cannot find email template {0}.", id));
            string subject = string.IsNullOrEmpty(template.Subject) ? null : Tokenize(template.Subject);
            string body = string.IsNullOrEmpty(template.Body) ? null : Tokenize(template.Body);
            var smtp = new SmtpMessageDelivery();
            var msg = new Message
            {
                To = to,
                Subject = "[Email Template Test] " + subject,
                Body = body
            };
            smtp.Send(msg);
            SessionMessageManager.SetMessage(MessageType.Success, MessageBehaviors.StatusBar, "Test email is sent.");
            return Ok();
        }
        private string Tokenize(string msg)
        {
            string username = null, email = null, mobile = null;
            string verificationKey = "dT34Lgd3O8cyjsHCREf78g";
            string baseUrl = GetApplicationBaseUrl(), loginUrl = "login", confirmPasswordResetUrl = "confirmpasswordreset/", confirmChangeEmailUrl = "confirmemail/",
                cancelVerificationUrl = "cancelverificationrequest/", initialPassword = "A2^kF7%mG5(vJ8$uP", thumbprint = "kF7%mG", providerName = "Microsoft";
            ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
            if (principal != null)
            {
                Claim claim = principal.FindFirst(ClaimTypes.Email);
                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    email = claim.Value;
                    msg = msg.Replace("{email}", email);
                }
                claim = principal.FindFirst(ClaimTypes.Name);
                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    username = claim.Value;
                    msg = msg.Replace("{username}", username);
                }
                claim = principal.FindFirst(ClaimTypes.MobilePhone);
                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    mobile = claim.Value;
                    msg = msg.Replace("{mobile}", mobile);
                }
            }

            msg = msg.Replace("{applicationName}", App.Common.Util.ApplicationConfiguration.AppAcronym);
            msg = msg.Replace("{emailSignature}", App.Common.Util.ApplicationConfiguration.SupportOrganization);
            msg = msg.Replace("{loginUrl}", baseUrl + loginUrl);

            msg = msg.Replace("{confirmPasswordResetUrl}", baseUrl + confirmPasswordResetUrl + verificationKey);
            msg = msg.Replace("{confirmChangeEmailUrl}", baseUrl + confirmChangeEmailUrl + verificationKey);
            msg = msg.Replace("{cancelVerificationUrl}", baseUrl + cancelVerificationUrl + verificationKey);

            msg = msg.Replace("{InitialPassword}", initialPassword);
            msg = msg.Replace("{Thumbprint}", thumbprint);
            msg = msg.Replace("{ProviderName}", providerName);

            return msg;
        }
        private GridModel<MessageTemplate> GetQuery(IQueryable<MessageTemplate> query,[FromUri] JqGridSearchModel searchModel, int maxRecords = Constants.DEFAULT_MAX_RECORDS_RETURN)
        {
            if (Constants.SHOULD_FILTER_BY_APP)
                query = query.Where(x => x.Name.StartsWith(string.Format("{0}.", App.Common.Util.ApplicationConfiguration.AppAcronym)));
            var data = Web.Infrastructure.Util.GetGridData<MessageTemplate>(searchModel, query);
            return data;
        }
        private string GetApplicationBaseUrl()
        {
            string baseUrl = System.Configuration.ConfigurationManager.AppSettings[Constants.APPSETTING_KEY_BASE_URL];
            if (string.IsNullOrWhiteSpace(baseUrl))
                baseUrl = (HttpContext.Current.Request.Url.GetComponents(
                    UriComponents.SchemeAndServer, UriFormat.Unescaped).TrimEnd('/')
                 + HttpContext.Current.Request.ApplicationPath.Substring(0, HttpContext.Current.Request.ApplicationPath.LastIndexOf('/')).TrimEnd('/')); //HttpContext.Current.GetApplicationUrl();
            return baseUrl;
        }
    }
}
