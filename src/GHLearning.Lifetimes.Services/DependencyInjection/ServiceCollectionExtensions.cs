using GHLearning.Lifetimes.Core;
using Microsoft.Extensions.DependencyInjection;
using SequentialGuid;

namespace GHLearning.Lifetimes.Services.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServices(
		this IServiceCollection services)
		=> services.AddScoped<ITodolistService>((sp) => ActivatorUtilities.CreateInstance<TodolistService>(sp, Guid.NewGuid()));
}