﻿using System;
using System.Diagnostics;
using System.Net.Http;

namespace PerfIt.Handlers
{
    public class LastOperationExecutionTimeHandler : CounterHandlerBase
    {
        private const string TimeTakenTicksKey = "LastOperationExecutionTimeHandler_#_StopWatch_#_";
        private readonly Lazy<PerformanceCounter> _counter;

        public LastOperationExecutionTimeHandler(string applicationName, PerfItFilterAttribute filter) : base(applicationName, filter)
        {
            _counter = new Lazy<PerformanceCounter>(() =>
                {
                    var counter = new PerformanceCounter()
                    {
                        CategoryName = filter.CategoryName,
                        CounterName = Name,
                        InstanceName = applicationName,
                        ReadOnly = false,
                        InstanceLifetime = PerformanceCounterInstanceLifetime.Process
                    };
                    counter.RawValue = 0;
                    return counter;
                });
        }

        public override string CounterType
        {
            get { return CounterTypes.LastOperationExecutionTime; }
        }

        protected override void OnRequestStarting(HttpRequestMessage request, PerfItContext context)
        {
            context.Data.Add(TimeTakenTicksKey + Name, Stopwatch.StartNew());
        }

        protected override void OnRequestEnding(HttpResponseMessage response, PerfItContext context)
        {
            var sw = (Stopwatch)context.Data[TimeTakenTicksKey + Name];
            sw.Stop();
            _counter.Value.RawValue = sw.ElapsedMilliseconds;
        }

        protected override CounterCreationData[] DoGetCreationData()
        {
            var counterCreationDatas = new CounterCreationData[1];
            counterCreationDatas[0] = new CounterCreationData()
            {
                CounterType = PerformanceCounterType.NumberOfItems32,
                CounterName = Name,
                CounterHelp = _filter.Description
            };

            return counterCreationDatas;
        }
    }
}
