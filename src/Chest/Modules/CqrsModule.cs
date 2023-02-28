using System;
using System.Linq;
using Autofac;
using Chest.Projections;
using Chest.Settings;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Cqrs.Configuration.BoundedContext;
using Lykke.Cqrs.Configuration.Routing;
using Lykke.Cqrs.Middleware.Logging;
using Lykke.Messaging.Serialization;
using Lykke.Snow.Common.Startup;
using Lykke.Snow.Cqrs;
using MarginTrading.AssetService.Contracts.Currencies;
using MarginTrading.AssetService.Contracts.ProductCategories;
using Microsoft.Extensions.Logging;

namespace Chest.Modules
{
    internal class CqrsModule : Module
    {
        private const string DefaultRoute = "self";
        private readonly long _defaultRetryDelayMs;
        private readonly CqrsSettings _settings;
        private readonly CqrsContextNamesSettings _contextNames;

        public CqrsModule(CqrsSettings settings)
        {
            _settings = settings;
            _contextNames = settings.ContextNames;
            _defaultRetryDelayMs = (long) _settings.RetryDelay.TotalMilliseconds;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(GetType().Assembly).Where(t =>
                new[] {"Saga", "CommandsHandler", "Projection"}.Any(ending => t.Name.EndsWith(ending))).AsSelf();

            builder.Register(CreateEngine)
                .As<ICqrsEngine>()
                .SingleInstance();
        }

        private CqrsEngine CreateEngine(IComponentContext ctx)
        {
            var rabbitMqSettings = new RabbitMQ.Client.ConnectionFactory
            {
                Uri = new Uri(_settings.ConnectionString, UriKind.Absolute)
            };
            
            var rabbitMqConventionEndpointResolver = new RabbitMqConventionEndpointResolver(
                "RabbitMq",
                SerializationFormat.MessagePack,
                environment: _settings.EnvironmentName);

            var log = new LykkeLoggerAdapter<CqrsModule>(ctx.Resolve<ILogger<CqrsModule>>());
            
            var engine = new RabbitMqCqrsEngine(
                log,
                ctx.Resolve<IDependencyResolver>(),
                new DefaultEndpointProvider(),
                rabbitMqSettings.Endpoint.ToString(),
                rabbitMqSettings.UserName,
                rabbitMqSettings.Password,
                true,
                Register.DefaultEndpointResolver(rabbitMqConventionEndpointResolver),
                RegisterContext(),
                Register.CommandInterceptors(new DefaultCommandLoggingInterceptor(log)),
                Register.EventInterceptors(new DefaultEventLoggingInterceptor(log)));

            return engine;
        }

        private IRegistration RegisterContext()
        {
            var contextRegistration = Register.BoundedContext(_contextNames.Chest)
                .FailedCommandRetryDelay(_defaultRetryDelayMs)
                .ProcessingOptions(DefaultRoute).MultiThreaded(8).QueueCapacity(1024);

            RegisterCurrenciesProjection(contextRegistration);
            RegisterProductCategoriesProjection(contextRegistration);

            return contextRegistration;
        }

        private void RegisterCurrenciesProjection(
            ProcessingOptionsDescriptor<IBoundedContextRegistration> contextRegistration)
        {
            contextRegistration.ListeningEvents(
                    typeof(CurrencyChangedEvent))
                .From(_settings.ContextNames.AssetService).On(nameof(CurrencyChangedEvent))
                .WithProjection(
                    typeof(CurrencyProjection), _settings.ContextNames.AssetService);
        }

        private void RegisterProductCategoriesProjection(
            ProcessingOptionsDescriptor<IBoundedContextRegistration> contextRegistration)
        {
            contextRegistration.ListeningEvents(
                    typeof(ProductCategoryChangedEvent))
                .From(_settings.ContextNames.AssetService).On(nameof(ProductCategoryChangedEvent))
                .WithProjection(
                    typeof(ProductCategoryProjection), _settings.ContextNames.AssetService);
        }
    }
}