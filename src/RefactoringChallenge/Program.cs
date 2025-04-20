using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RefactoringChallenge.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RefactoringChallenge.Persistence.Database;
using RefactoringChallenge.Abstractions;
using RefactoringChallenge.Abstractions.Repository;
using RefactoringChallenge.Persistence.Implementation.Repository;

namespace RefactoringChallenge
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .Build();

                AddServices(services, configuration)         
                    .BuildServiceProvider();
            })
            .Build();

            var logger = builder.Services.GetService<ILogger<Program>>();
            logger.LogInformation("Starting...");

            CreateDbAndMigrateDatabase(builder);

            // TODO
            // for testing purposes
            using var scope = builder.Services.CreateScope();
            var customerOrderProcessor = scope.ServiceProvider.GetRequiredService<ICustomerOrderProcessor>();
            try
            {
                var processed = await customerOrderProcessor.ProcessCustomerOrdersAsync(1);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Caught exception while calling ProcessCustomerOrdersAsync");
             //   throw;
            }            
            // 

            builder.Run();                      

            logger.LogInformation("App quit.");           
        }

        private static IServiceCollection AddServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddConsole();
            })
            .AddDbContext<RefactoringDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly("RefactoringChallenge.Persistence"));
            })
            .AddScoped<ICustomerOrderProcessor, CustomerOrderProcessor>()
            .AddScoped<IDiscountCalculator, DiscountCalculator>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IOrderLogsRepository, OrderLogsRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IInventoryRepository, InventoryRepository>();

            return services;
        }

        private static void CreateDbAndMigrateDatabase(IHost builder)
        {
            using var scope = builder.Services.CreateScope();
            RefactoringDbContext context = scope.ServiceProvider.GetRequiredService<RefactoringDbContext>();

            context.Database.Migrate();
        }
    }
}
