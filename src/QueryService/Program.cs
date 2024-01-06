using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using RS.MF.MoodTracking.Application.Queries;
using RS.MF.MoodTracking.Infrastructure.QueryHandlers;

namespace RS.MF.ServiceName.HostService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiBuilder = new ApiPiplineBuilderOptions
            {
                RequiresImpersonation = false,
                EnableGraphQl = true,
                EnableFileLogging = true,
                EnableConsoleLogging = true,
                EnableSeriLogging = true,
                UseJwtBearerAuthentication = true,
                CommandLineArguments = args,
                AddApplicationServices = AddApplicationServices,
                AddApplicationConfigure = Configure,
                RDMSMigrationAssemblyName = typeof(Program).Assembly.FullName
            };
            var host = WebHostApiPipelineBuilder.BuildWebApiPipeline(apiBuilder);
            host.Build().Run();
        }

        private static void AddApplicationServices(IServiceCollection services, IAppSettings appSettings, IConfiguration Configuration)
        {
            services.AddSingleton(appSettings);

            services.AddSingleton<IMoodTrackingQueryService, MoodTrackingQueryService>();
            services.AddSingleton<IMoodTrackingRepository, MoodTrackingRepository>();

            services.AddApiDocument(new SwaggerOptions()
            {
                Description = "Use the api to sign electronic documents",
                ApplicableEnvironments = new string[] { "dev", "dev-az", "prod-az", "stg", "stg-az" }
            }, appSettings);

            services.AddControllers()
               .AddFluentValidation(s =>
               {
                   //s.RegisterValidatorsFromAssemblyContaining<UserQuery>();
                   s.DisableDataAnnotationsValidation = true;
               });

            services.AddCore(
                typeof(Producer),
                typeof(UserMoodQuery),
                typeof(MoodTrakingQueryHandler)
                );
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.InstallQueryServicesFromAssembly(Configuration);
        }

        public static void Configure(IApplicationBuilder app)
        {
            //app.ConfigureGraphQl();
        }
    }
}