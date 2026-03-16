using System.Reflection;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    internal sealed class Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        public IWebHostEnvironment Environment { get; } = environment;

        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).GetTypeInfo().Assembly));

            services.AddAuthentication(TestServerDefaults.AuthenticationScheme)
                .AddTestServer();

            services.AddControllers(options =>
                {
                    ApiConfiguration.ConfigureControllers(options);

                    // Clear all formaters, we only need to validate the host and http response codes
                    // This avoid that System.Text.Json serializes the error and breaks the TestHost.
                    // System.InvalidOperationException: The PipeWriter 'ResponseBodyPipeWriter' does not implement PipeWriter.UnflushedBytes.
                    options.OutputFormatters.Clear();

                    // Add our custom formatter to validate the host and http response codes
                    options.OutputFormatters.Add(new NoOpFormatter());
                })
                .WithApiControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Force JSON serialization errors to be plain text.
                    // This avoid that System.Text.Json serializes the error and breaks the TestHost.
                    // System.InvalidOperationException: The PipeWriter 'ResponseBodyPipeWriter' does not implement PipeWriter.UnflushedBytes.
                    options.InvalidModelStateResponseFactory = context => new Microsoft.AspNetCore.Mvc.ContentResult
                    {
                        StatusCode = 400,
                        Content = "Model validation failed (JSON bypassed)",
                        ContentType = "text/plain"
                    };
                });

            services.AddBaseInfrastructure(true);

            // Register UseCases mocks
            services.AddScoped(_ => new Mock<IUseCase<CreateVehicleInput>>().Object);
            services.AddScoped(_ => new Mock<IUseCase<GetAvailableVehiclesInput>>().Object);
        }
    }
}
