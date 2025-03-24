using GHLearning.Lifetimes.Migrations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GHLearning.Lifetimes.Migrations;

public class MigrationsContextFactory : IDesignTimeDbContextFactory<SampleContext>
{
	public SampleContext CreateDbContext(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureHostConfiguration(builder => builder
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables())
			.ConfigureServices(services => services
				.AddDbContext<SampleContext>(
				dbOptions => dbOptions
				.UseMySql(
					connectionString: services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetConnectionString("MySql"),
					serverVersion: ServerVersion.AutoDetect(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetConnectionString("MySql")),
					mySqlOptionsAction: option => option
					.MigrationsHistoryTable("__migrations_history")
					.MigrationsAssembly("GHLearning.Lifetimes.Migrations"))))
			.Build();

		var dbContext = host.Services.GetRequiredService<SampleContext>();

		return dbContext;
	}
}
