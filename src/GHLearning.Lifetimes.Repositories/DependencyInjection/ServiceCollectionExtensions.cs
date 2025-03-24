using GHLearning.Lifetimes.Core;
using GHLearning.Lifetimes.Repositories;
using GHLearning.Lifetimes.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRepositories(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptionsBuilder)
		=> services
		.AddDbContextFactory<SampleContext>(dbContextOptionsBuilder)
		.AddTransient<ITodolistRepository>((sp) => ActivatorUtilities.CreateInstance<TodolistRepository>(sp, Guid.NewGuid()));
}
