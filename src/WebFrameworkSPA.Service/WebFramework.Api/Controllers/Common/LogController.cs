using WebFramework.Data.Domain;
using App.Common.InversionOfControl;
using App.Common.Logging;
using Service;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Linq.Dynamic;
using Web.Infrastructure;
using Web.Infrastructure.Exceptions;
using Web.Infrastructure.JqGrid;
using System.IO;
using System.Data;

namespace Web.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class LogController : ApiController
    {
        private ILogService _service;
        public LogController()
        {
            _service = IoC.GetService<ILogService>();
        }
        // GET api/log/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/log
        [AllowAnonymous]
        public HttpResponseMessage PostJavascriptLog(LogEntry data)
        {
            //LogLevel logLevel = LogLevel.Debug;
            //foreach (var item in data)
            //{
            //    StringBuilder message = new StringBuilder();
            //    message.AppendLine(item.Message);
            //    message.AppendFormat("Request Url: {0}", item.Url);
            //    Enum.TryParse<LogLevel>(item.Level, true, out logLevel);
            //    //Logger.Log(logLevel, new JavascriptException(item.Message));
            //    if (logLevel == LogLevel.Error || logLevel == LogLevel.Fatal)
            //        Logger.Log(logLevel, new JavascriptException(message.ToString()));
            //    else
            //        Logger.Log(logLevel, message.ToString());
            //}
            //var response = Request.CreateResponse(HttpStatusCode.Created);
            //return response;
            LogLevel logLevel = LogLevel.Debug;
            StringBuilder message = new StringBuilder();
            message.AppendLine(data.Message);
            message.AppendFormat("Request Url: {0}", data.Url);
            Enum.TryParse<LogLevel>(data.Level, true, out logLevel);
            if (logLevel == LogLevel.Error || logLevel == LogLevel.Fatal)
                Logger.Log(logLevel, new JavascriptException(message.ToString()));
            else
                Logger.Log(logLevel, message.ToString());
            var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;

        }
        public struct LogEntry
        {
            public string Logger;
            public long Timestamp;
            public string Level;
            public string Url;
            public string Message;
        }
        public dynamic GetGridData([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            var data = GetQuery(_service.Query(),searchModel);
            var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.LogLevel, x.UserName, x.Message, x.ClientIP,x.Host, x.SessionId }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                //records = data.TotalNumber,
                //rows = dataList.Select(x => new { id = x.FundReqId, cell = new object[] { x.FundReqId, x.Year, x.ApplFormCode, x.FundReqStatusCd, x.FundReqReceiptNtfctnDt, x.CmmtmntNtfctnDt, x.CrtfctnRecvdDt, x.PiaCertificationCutoffDt } }).ToArray()
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new { x.Id, x.Application, x.CreatedDate, x.LogLevel, x.UserName, x.Message,x.ClientIP, x.Host, x.SessionId }).ToArray()
            };
        }
        [Route("api/log/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcel([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            string filePath=null;
            HttpResponseMessage result = null;
            try
            {
                searchModel.rows = 0;
                var data = GetQuery(Util.GetStatelessQuery<Logs>(), searchModel);
                var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.LogLevel, x.UserName, x.Message,x.ClientIP, x.Host, x.SessionId });
                filePath = ExporterManager.Export("Logs", ExporterType.CSV, data.Items.ToList(), "");
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
        public dynamic GetById(long id)
        {
            if (id == default(long))
                return BadRequest("Id cannot be emapty.");
            var item = _service.GetLogById(id);
            return Ok(item);
        }
        private GridModel<Logs> GetQuery(IQueryable<Logs> query, [FromUri] JqGridSearchModel searchModel, int maxRecords = Constants.DEFAULT_MAX_RECORDS_RETURN)
        {
            if (Constants.SHOULD_FILTER_BY_APP)
                query = query.Where(x => x.Application == App.Common.Util.ApplicationConfiguration.AppAcronym);
            var data = Util.GetGridData<Logs>(searchModel, query);
            return data;
        }
    }

}
