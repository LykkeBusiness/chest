﻿// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Autofac;
using Chest.Data.Repositories;
using Chest.Extensions;
using Chest.Modules;
using Chest.Settings;
using EFCoreSecondLevelCacheInterceptor;
using JetBrains.Annotations;
using Lykke.Cqrs;
using Lykke.SettingsReader.SettingsTemplate;
using Lykke.Snow.Common.Startup;
using Lykke.Snow.Common.Startup.ApiKey;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Chest
{
    using CacheManager.Core;
    using Data;
    using Mappers;
    using Services;
    using Lykke.Middlewares;
    using Lykke.Middlewares.Mappers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static string ApiTitle => "Chest API";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) => options
                .UseSqlServer(_configuration.GetConnectionString("Chest"))
                .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));

            services
                .AddControllers()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                            {NamingStrategy = new CamelCaseNamingStrategy()};
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddSingleton<IHttpStatusCodeMapper, HttpStatusCodeMapper>();
            services.AddSingleton<ILogLevelMapper, DefaultLogLevelMapper>();

            if (!TimeSpan.TryParse(_configuration.GetValue<string>("CacheExpiration"), out var cacheExpiration))
            {
                cacheExpiration = TimeSpan.FromMinutes(5);
            }

            var cacheManagerConfiguration = new CacheManager.Core.ConfigurationBuilder()
                .WithJsonSerializer()
                .WithMicrosoftMemoryCacheHandle()
                .WithExpiration(ExpirationMode.Sliding, cacheExpiration)
                .Build();

            services.AddEFSecondLevelCache(options =>
                options.UseCacheManagerCoreProvider().DisableLogging(true).UseCacheKeyPrefix("EF_")
            );
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration), cacheManagerConfiguration);
            


            // Configure versions
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(2, 0);
            });

            var clientSettings = _configuration.GetSection("ChestClientSettings").Get<ClientSettings>();
            services.AddApiKeyAuth(clientSettings);

            // Configure swagger
            services.AddSwaggerGen(options =>
            {
                // Specifying versions
                options.SwaggerDoc("v2", CreateInfoForApiVersion("v2", false));

                // This call remove version from parameter, without it we will have version as parameter for all endpoints in swagger UI
                options.OperationFilter<RemoveVersionFromParameter>();

                // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc, i.e. v1
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                
                // This exclude endpoint not specified in swagger version, i.e. MapToApiVersion("99")
                options.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out var methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var maps = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();
                    
                    return versions.Any(v => $"v{v.ToString()}" == version) && (maps.Length == 0 || maps.Any(v => $"v{v.ToString()}" == version));
                });

                if (!string.IsNullOrWhiteSpace(clientSettings?.ApiKey))
                {
                    options.AddApiKeyAwareness();
                }
            }).AddSwaggerGenNewtonsoftSupport();
            
            // Default settings for NewtonSoft Serializer
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                };

                settings.Converters.Add(new StringEnumConverter());

                return settings;
            };

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IDataService, DataService>();
            services.AddScoped<ILocalizedValuesRepository, LocalizedValuesRepository>();
            services.AddScoped<ILocalizedValuesService, LocalizedValuesService>();
            services.AddScoped<ILocalesRepository, LocalesRepository>();
            services.AddScoped<ILocalesService, LocalesService>();

            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddSettingsTemplateGenerator();
        }
        
        [UsedImplicitly]
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            var cqrsSettings = _configuration.GetSection("CqrsSettings").Get<CqrsSettings>();

            builder.RegisterModule(new CqrsModule(cqrsSettings));
            builder.RegisterModule(new MsSqlModule(_configuration));
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.AddSettingsTemplateEndpoint();
                endpoints.MapControllers();
            });
            
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((doc, req) => doc.Servers = new List<OpenApiServer>
                {
                    new OpenApiServer
                    {
                        Url = $"{req.Scheme}://{req.Host.Value}"
                    }
                });
            });
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "swagger/ui";
                x.SwaggerEndpoint($"/swagger/v2/swagger.json", $"{ApiTitle} v2");
                x.EnableValidator(null);
            });

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
                try
                {
                    app.ApplicationServices.GetRequiredService<ICqrsEngine>().StartSubscribers();
                }
                catch (Exception e)
                {
                    Log.Fatal(e, "Failed to start CQRS engine");
                    applicationLifetime.StopApplication();
                    return;
                }
                logger.LogInformation("Application started");
            });
            
            app.InitializeDatabase();
        }

        private OpenApiInfo CreateInfoForApiVersion(string apiVersion, bool isObsolete)
        {
            var info = new OpenApiInfo
            {
                Title = $"{ApiTitle}",
                Version = apiVersion,
                Description = ApiTitle,
                Contact = new OpenApiContact(),
                License = new OpenApiLicense {Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT")}
            };

            if (isObsolete)
            {
                info.Description += ". This API version is obsolete and will be discontinued soon.";
            }

            return info;
        }
    }
}
