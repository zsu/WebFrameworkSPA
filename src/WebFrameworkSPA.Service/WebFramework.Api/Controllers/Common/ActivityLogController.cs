﻿using Service;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic;
using WebFramework.Data.Domain;
using App.Common.InversionOfControl;
using Web.Infrastructure;
using Web.Infrastructure.JqGrid;
using System.IO;

namespace Web.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class ActivityLogController : ApiController
    {
        private IActivityLogService _service;
        public ActivityLogController()
        {
            _service = IoC.GetService<IActivityLogService>();
        }

        public dynamic GetGridData([FromUri] JqGridSearchModel searchModel)
        {
            var data = GetQuery(_service.Query(),searchModel);
            var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToArray()
            };
        }
        [Route("api/activitylog/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcel([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            string filePath = null;
            HttpResponseMessage result = null;
            try
            {
                searchModel.rows = 0;
                var data = GetQuery(Util.GetStatelessQuery<ActivityLog>(), searchModel);
                var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToList();
                filePath = ExporterManager.Export("activitylog", ExporterType.CSV, dataList.ToList(), "");
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
        private GridModel<ActivityLog> GetQuery(IQueryable<ActivityLog> query,[FromUri] JqGridSearchModel searchModel, int maxRecords = Constants.DEFAULT_MAX_RECORDS_RETURN)
        {
            if (Constants.SHOULD_FILTER_BY_APP)
                query = query.Where(x => x.Application == App.Common.Util.ApplicationConfiguration.AppAcronym);
            var data = Util.GetGridData<ActivityLog>(searchModel, query);
            return data;
        }
    }
}
