using App.Common.Logging;
using BrockAllen.MembershipReboot.Nh;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic;
using Web.Infrastructure.Exceptions;
using App.Common.InversionOfControl;
using Web.Infrastructure.JqGrid;
using Web.Infrastructure;
using WebFramework.Data.Domain;
using System.IO;

namespace Web.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class AuthenticationAuditController : ApiController
    {
        private IAuthenticationAuditService _service;
        public AuthenticationAuditController()
        {
            _service = IoC.GetService<IAuthenticationAuditService>();
        }

        public dynamic GetGridData([FromUri] JqGridSearchModel searchModel)
        {
            var query = _service.Query();
            if (Constants.SHOULD_FILTER_BY_APP)
                query = query.Where(x => x.Application == App.Common.Util.ApplicationConfiguration.AppAcronym);
            var data = Util.GetGridData<AuthenticationAudit>(searchModel, query);
            var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToArray()
            };
        }
        [Route("api/authenticationaudit/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcel([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            string filePath = null;
            HttpResponseMessage result = null;
            try
            {
                var query = _service.Query();
                if (Constants.SHOULD_FILTER_BY_APP)
                    query = query.Where(x => x.Application == App.Common.Util.ApplicationConfiguration.AppAcronym);
                searchModel.rows = 0;
                var data = Util.GetGridData<AuthenticationAudit>(searchModel, query);
                var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToList();
                filePath = ExporterManager.Export("authenticationaudit", ExporterType.CSV, dataList.ToList(), "");
            }
            catch (Exception ex)
            {
                return Util.DisplayExportError(ex);
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
    }
}
