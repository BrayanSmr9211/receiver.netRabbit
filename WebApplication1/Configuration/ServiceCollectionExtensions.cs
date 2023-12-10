namespace Microsoft.Extensions.DependencyInjection
{
    using Domain.Infrastructure.Abstract;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.DataProtection.Infrastructure;
    using Domain.Abstract;
    using Infrastructure;
    using Domain.Student.Query;
    using Domain.Configuration;
    using Domain.Infrastructure.Abstract.InterfaceC;
    using Domain.User.Command.Delete;
    using Domain.User.Command.Put;
    using Domain.JsonToken.Query;
    using Domain.RabbitWork;
    using Domain.Infrastructure.Abstract.Rabbit;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection service)
        {
            var sp = service.BuildServiceProvider();
            var configuration = sp.GetService<IConfiguration>();

            // Add framework services.

            service.AddTransient<IInternalService>();
            service.AddTransient<IPostCommand, PostCommand>();
            service.AddTransient<IGetListUserQuery, GetListUserQuery>();
            service.AddTransient<IDeletCommand, DeleteComman>();
            service.AddTransient<IPutCommand, PutCommand>();
            service.AddTransient<IPostCommandJwt, GetjsonToken>();
            service.AddTransient<ITokenJwt, ValidaJWToken>();
            service.AddTransient<IRabbitworkers, Rabbitworker>();
            service.AddTransient<IQueueWork, consumptionQueue>();
            service.AddTransient<IRabbitcons, SigninWorker>();
            



            // Add IdentityAppOptions
            // TODO: Validate Options
            service.Configure<AppOption>(configuration.GetSection("UrlApps"));
            service.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<AppOption>>().Value);
            return service;

        }
    }
}
