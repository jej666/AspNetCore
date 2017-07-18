﻿// <copyright file="AppMetricsApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Middleware to the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> request
        ///     execution pipeline.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsDefaults(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.
                    AddMetrics(context.Configuration.GetSection("AppMetrics")).
                    AddMetricsMiddleware(
                        context.Configuration.GetSection("AppMetricsAspNetCore"),
                        optionsBuilder =>
                        {
                            optionsBuilder.AddMetricsJsonFormatters().
                                           AddMetricsTextAsciiFormatters().
                                           AddEnvironmentAsciiFormatters();
                        });
            });

            return hostBuilder;
        }
    }
}