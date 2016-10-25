﻿using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Registries;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Facts
{
    public static class TestContextHelper
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        public static IMetricsContext Instance(string defaultGroupName, IClock clock, IScheduler scheduler)
        {
            var options = Options.Create(new AppMetricsOptions
            {
                Clock = clock,
                DefaultGroupName = defaultGroupName
            });
            Func<string, IMetricGroupRegistry> newGroupRegistry = name => new DefaultMetricGroupRegistry(name);

              Func<IMetricsContext, IMetricReporterRegistry> newReportManager = context => new DefaultMetricReporterRegistry(options, context, LoggerFactory);

        var registry = new DefaultMetricsRegistry(LoggerFactory, options, new EnvironmentInfoBuilder(LoggerFactory), newGroupRegistry);
            return new DefaultMetricsContext(options, registry,
                new TestMetricsBuilder(clock, scheduler),
                new DefaultHealthCheckManager(options, LoggerFactory,
                    new DefaultHealthCheckRegistry(LoggerFactory, Enumerable.Empty<HealthCheck>(), Options.Create(new AppMetricsOptions()))),
                new DefaultMetricsDataManager(registry), newReportManager);
        }

        public static IMetricsContext Instance()
        {
            return Instance("TestContext", new Clock.TestClock());
        }

        public static IMetricsContext Instance(IClock clock, IScheduler scheduler)
        {
            return Instance("TestContext", clock, scheduler);
        }

        public static IMetricsContext Instance(string defaultGroupName, IClock clock)
        {
            return Instance(defaultGroupName, clock, new TestScheduler(clock));
        }
    }
}