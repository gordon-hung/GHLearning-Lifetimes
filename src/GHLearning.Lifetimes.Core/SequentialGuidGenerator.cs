namespace GHLearning.Lifetimes.Core;

internal class SequentialGuidGenerator(
	Guid lifetimeId) : ISequentialGuidGenerator
{
	public Guid GetLifetimeId() => lifetimeId;

	public Task<Guid> NewIdAsync(CancellationToken cancellationToken = default) => Task.FromResult(SequentialGuid.SequentialGuidGenerator.Instance.NewGuid());
}
