﻿using System;
using System.Web.Http.Filters;

namespace PerfIt
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PerfItFilterAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Optional name of the counter. 
        /// If not specified it will be [controller].[action].[counterType] for each counter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the counter. Will be published to counter metadata visible in Perfmon.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Optional category name. If left blank, name of the assembly containing controllers is used.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Counter types. Each value as a string.
        /// </summary>
        public string[] Counters { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Request.Properties.ContainsKey(Constants.PerfItKey))
            {
                var context = (PerfItContext) actionExecutedContext.Request.Properties[Constants.PerfItKey];
                if (string.IsNullOrEmpty(Name))
                {
                    Name = string.Format("{0}.{1}",
                                         actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor
                                                              .ControllerType.Name,
                                         actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
                }
                foreach (var counter in Counters)
                {
                    context.CountersToRun.Add(Name + "." + counter);    
                }

                context.Filter = this;
            }
        } 
    }
}
