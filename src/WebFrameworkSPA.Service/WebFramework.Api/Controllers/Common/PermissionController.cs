using BrockAllen.MembershipReboot.Nh;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic;
using App.Common.InversionOfControl;
using System.Text;
using Web.Infrastructure.JqGrid;
using Web.Infrastructure;
using System.IO;

namespace Web.Controllers.Api
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class PermissionController : ApiController
    {
        private IPermissionService _permissionService;
        public PermissionController()
        {
            _permissionService = IoC.GetService<IPermissionService>();
        }
        // GET api/permission
        public dynamic GetGridData([FromUri] JqGridSearchModel searchModel)
        {
            var query = _permissionService.Query();
            //query = query.Where(x => x.Name.StartsWith(string.Format("{0}.", App.Common.Util.ApplicationConfiguration.AppAcronym)));
            var data = Web.Infrastructure.Util.GetGridData<Permission>(searchModel, query);
            var dataList = data.Items.Select(x => new { x.Id, x.Name, x.Description }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new { x.Id, x.Name, x.Description }).ToArray()
            };
        }
        [Route("api/permission/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcel([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            var query = _permissionService.Query();
            //query = query.Where(x => x.Name.StartsWith(string.Format("{0}.", App.Common.Util.ApplicationConfiguration.AppAcronym)));
            searchModel.rows = 0;
            var data = Web.Infrastructure.Util.GetGridData<Permission>(searchModel, query);
            var dataList = data.Items.Select(x => new {x.Name, x.Description }).ToList();
            string filePath = ExporterManager.Export("permission", ExporterType.CSV, dataList.ToList(), "");
            HttpResponseMessage result = null;

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
        // GET api/permission/5
        public IHttpActionResult Get(Guid id)
        {
            var item = _permissionService.Query().FirstOrDefault((p) => p.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/permission
        public IHttpActionResult Post([FromBody] Permission item)
        {
            StringBuilder message = new StringBuilder();
            if (item == null)
                return BadRequest("Permission cannot be empty.");
            if (string.IsNullOrEmpty(item.Name))
                return BadRequest("Permissionname cannot be empty.");
            item.Name = item.Name.Trim();
            item.Description = string.IsNullOrEmpty(item.Description) ? null : item.Description.Trim();
            if (_permissionService.PermissionExists(item.Name))
                return BadRequest(string.Format("Permission {0} already exists.", item.Name));
            _permissionService.CreatePermission(item);
            message.AppendFormat("Permission {0}  is saved successflly.", item.Name);
            return Json<object>(new { Success = true, Message = message.ToString(), RowId = item.Id });
        }

        // PUT api/permission/5
        public IHttpActionResult Put(Guid id, [FromBody] Permission item)
        {
            StringBuilder message = new StringBuilder();
            if (id == default(Guid))
                return BadRequest("Permission id cannot be empty.");
            if (item==null)
            {
                return BadRequest("Permission cannot be empty.");
            }
            if (string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("Permissionname cannot be empty.");
            }
            item.Id = id;
            item.Name = item.Name.Trim();
            item.Description=string.IsNullOrEmpty(item.Description)?null:item.Description.Trim();
            _permissionService.UpdatePermission(item);
            message.AppendFormat("Permission {0}  is saved successflly.", item.Name);
            return Json<object>(new { Success = true, Message = message.ToString(), RowId=item.Id });

        }

        // DELETE api/permission/5
        public IHttpActionResult Delete(Guid id)
        {
            if (id == default(Guid))
                return BadRequest("Permission id cannot be empty.");
            if (!_permissionService.CanDelete(id))
                return BadRequest("Permission is still being used and cannot be deleted.");
            if (_permissionService.DeletePermission(id))
                return Ok();
            else
                return NotFound();
        }

    }
}
