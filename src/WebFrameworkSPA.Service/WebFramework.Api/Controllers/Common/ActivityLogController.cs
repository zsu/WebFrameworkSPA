﻿using App.Common.Logging;
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
using WebFramework.Data.Domain;
using App.Common.InversionOfControl;
using Web.Infrastructure;
using Web.Infrastructure.JqGrid;

namespace Web.Controllers
{
    public class ActivityLogController : ApiController
    {
        private IActivityLogService _service;
        public ActivityLogController()
        {
            _service = IoC.GetService<IActivityLogService>();
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public dynamic GetGridData([FromUri] JqGridSearchModel searchModel)
        {
            var query = _service.Query();
            if(Constants.SHOULD_FILTER_BY_APP)
                query = query.Where(x => x.Application==App.Common.Util.ApplicationConfiguration.AppAcronym);
            var data = Util.GetGridData<ActivityLog>(searchModel, query);
            var dataList = data.Items.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new { x.Id, x.Application, x.CreatedDate, x.Activity, x.Detail, x.UserName, x.ClientIP }).ToArray()
            };
        }
    }
}
