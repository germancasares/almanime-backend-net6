using API.Repositories;
using API.Utils;
using Jobs;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Jobs
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("AlmanimeConnection");

            builder.Services
                .AddDbContext<AlmanimeContext>(options => options.UseSqlServer(connectionString))
                .AddRepositories()
                .AddServices();
        }
    }
}
