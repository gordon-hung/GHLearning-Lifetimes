using GHLearning.Lifetimes.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCore(
		this IServiceCollection services)
		=> services.AddSingleton<ISequentialGuidGenerator>((sp) => ActivatorUtilities.CreateInstance<SequentialGuidGenerator>(sp, Guid.NewGuid()));
}
