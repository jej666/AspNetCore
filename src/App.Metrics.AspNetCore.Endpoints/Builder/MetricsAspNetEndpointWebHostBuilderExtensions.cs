﻿// <copyright file="MetricsAspNetEndpointWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.AspNetCore.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetEndpointWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsEndpoints(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsEndpoints(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsEndpoints(
            this IWebHostBuilder hostBuilder,
            Action<MetricEndpointsOptions> optionsDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsEndpoints(optionsDelegate, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupDelegate">A callback to configure <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsEndpoints(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, MetricEndpointsOptions> setupDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var endpointOptions = new MetricEndpointsOptions();
                    services.AddMetricsEndpoints(
                        options => setupDelegate(context, endpointOptions),
                        context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> containing <see cref="MetricEndpointsOptions" /></param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsEndpoints(
            this IWebHostBuilder hostBuilder,
            IConfiguration configuration,
            Action<MetricEndpointsOptions> optionsDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                services =>
                {
                    services.AddMetricsEndpoints(optionsDelegate, configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureAppMetricsHostingConfiguration(
            this IWebHostBuilder hostBuilder,
            Action<MetricsEndpointsHostingOptions> setupHostingConfiguration)
        {
            var metricsEndpointHostingOptions = new MetricsEndpointsHostingOptions();
            setupHostingConfiguration(metricsEndpointHostingOptions);

            var ports = new List<int>();

            if (metricsEndpointHostingOptions.AllEndpointsPort.HasValue)
            {
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsEndpoint} on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsTextEndpoint} endpoint on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.PingEndpoint} on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.EnvironmentInfoEndpoint} endpoint on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");

                ports.Add(metricsEndpointHostingOptions.AllEndpointsPort.Value);
            }
            else
            {
                if (metricsEndpointHostingOptions.MetricsEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsEndpoint} on port {metricsEndpointHostingOptions.MetricsEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.MetricsEndpointPort.Value);
                }

                if (metricsEndpointHostingOptions.MetricsTextEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsTextEndpoint} endpoint on port {metricsEndpointHostingOptions.MetricsTextEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.MetricsTextEndpointPort.Value);
                }

                if (metricsEndpointHostingOptions.PingEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.PingEndpoint} on port {metricsEndpointHostingOptions.PingEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.PingEndpointPort.Value);
                }

                if (metricsEndpointHostingOptions.EnvironmentInfoEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.EnvironmentInfoEndpoint} endpoint on port {metricsEndpointHostingOptions.EnvironmentInfoEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.EnvironmentInfoEndpointPort.Value);
                }
            }

            if (ports.Any())
            {
                var existingUrl = hostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
                var additionalUrls = string.Join(";", ports.Distinct().Select(p => $"http://localhost:{p}/"));
                hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};{additionalUrls}");
            }

            hostBuilder.ConfigureServices(services => services.Configure(setupHostingConfiguration));

            return hostBuilder;
        }
    }
}