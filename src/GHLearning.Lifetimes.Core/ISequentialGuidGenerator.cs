namespace GHLearning.Lifetimes.Core;

public interface ISequentialGuidGenerator
{
	/// <summary>
	/// Gets the lifetime identifier.
	/// </summary>
	/// <returns></returns>
	Guid GetLifetimeId();

	/// <summary>
	/// Creates new idasync.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	Task<Guid> NewIdAsync(CancellationToken cancellationToken = default);
}
