using RS.MF.MoodTracking.Application.ServicesContracts.Validator;
using RS.MF.MoodTracking.Infrastructure.Services.Validation;

namespace RS.MF.ServiceName.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiBuilder = new ApiPiplineBuilderOptions
            {
                RequiresImpersonation = false,
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
            services.AddApiDocument(new SwaggerOptions()
            {
                Description = "Use the api to sign electronic documents",
                ApplicableEnvironments = new string[] { "dev", "dev-az", "prod-az", "stg", "stg-az" }
            }, appSettings);
            services.AddSingleton<IUamServiceClient, UamServiceClient>();

            services.AddSingleton<IMoodActivityRepository, MoodActivityRepository>();
            services.AddSingleton<IMoodEntryRepository, MoodEntryRepository>();
            services.AddSingleton<IMoodTrackingRepository, MoodTrackingRepository>();
            services.AddSingleton<IValidatorQueryService, ValidatorQueryService>();
            services.AddSingleton<IMoodTrackingService, MoodTrackingService>();

            services.AddEventStore<MoodAdministrativeAggregate>(Configuration);
            services.AddEventStore<MoodTrackingAggregate>(Configuration);

            services.AddMessageBrokerEvent(Configuration, EnableMessageBrokerConsumerEnum.Event, typeof(MoodTrackingEventHandler).Assembly, eventAssemblies: new System.Collections.Generic.List<System.Reflection.Assembly> { typeof(MoodTrackingEvent).Assembly, typeof(UpdateUserEvent).Assembly });

            services.AddOutbox(Configuration);

            services.AddControllers()
               .AddFluentValidation(s =>
               {
                   s.DisableDataAnnotationsValidation = true;
               });

            services.AddCore(
                typeof(Producer),
                typeof(MoodTrackingCommand),
                typeof(MoodTrackingCommandValidator),
                typeof(MoodTrackingCommandHandler),
                typeof(MoodTrackingEvent),
                typeof(MoodTrackingEventHandler)
                );
            services.InstallMutationServicesFromAssembly(Configuration);
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.ConfigureGraphQl();
        }
    }
}