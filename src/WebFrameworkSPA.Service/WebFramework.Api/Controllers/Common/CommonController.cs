using App.Common;
using App.Common.Caching;
using App.Common.InversionOfControl;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Web.Infrastructure.Extensions;

namespace Web.Controllers.Common
{
    public class CommonController : ApiController
    {
        [Route("api/cache")]
        [Authorize(Roles=Constants.ROLE_ADMIN)]
        public IHttpActionResult DeleteCache()
        {
            var cacheManager = IoC.GetService<ICacheManager>(Util.ApplicationCacheKey);
            cacheManager.Clear();
            return Ok("Cache is clear.");
        }
        [Route("api/app")]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpGet]
        public IHttpActionResult RestartApplication()
        {
            Util.RestartAppDomain();
            return Ok("Application is restarted.");
        }
        [Route("api/version")]
        [HttpGet]
        public IHttpActionResult GetVersion()
        {
            string version = string.Format("{0} {1} {2} {3}", ConfigurationManager.AppSettings["SourceBranch"],ConfigurationManager.AppSettings["SourceVersion"], ConfigurationManager.AppSettings["Environment"], File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location).ToClientTime("yyyy-MM-dd"));
            return Json<object>(new { Success = true, Version = version }); 
        }
    }
}
