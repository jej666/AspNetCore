﻿// <copyright file="SatisfyingController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using MetricsReportingSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace MetricsReportingSandboxMvc.Controllers
{
    [Route("api/[controller]")]
    public class SatisfyingController : Controller
    {
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;

        public static Random Rnd { get; } = new Random();

        private readonly IMetrics _metrics;

        public SatisfyingController(IMetrics metrics, RequestDurationForApdexTesting durationForApdexTesting)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _durationForApdexTesting = durationForApdexTesting;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            var duration = _durationForApdexTesting.NextSatisfiedDuration;

            foreach (var i in Enumerable.Range(1, 3))
            {
                var tags = new MetricTags($"key{i}", $"value{i}");

                _metrics.Measure.Histogram.Update(Registry.One, tags, Rnd.Next(1, 500));
                _metrics.Measure.Histogram.Update(Registry.Two, tags, Rnd.Next(1, 500));
                _metrics.Measure.Histogram.Update(Registry.Three, tags, Rnd.Next(1, 500));
            }

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }
    }
}