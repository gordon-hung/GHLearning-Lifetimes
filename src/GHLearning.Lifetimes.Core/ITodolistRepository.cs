using GHLearning.Lifetimes.Core.Modesl;

namespace GHLearning.Lifetimes.Core;

public interface ITodolistRepository
{
	/// <summary>
	/// Gets the lifetime identifier.
	/// </summary>
	/// <returns></returns>
	Guid GetLifetimeId();

	/// <summary>
	/// Adds the asynchronous.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	Task AddAsync(TodolistAdd source, CancellationToken cancellationToken = default);

	/// <summary>
	/// Queries the asynchronous.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	IAsyncEnumerable<TodolistInfo> QueryAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the by identifier asynchronous.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	Task<TodolistInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Completeds the asynchronous.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	Task CompletedAsync(Guid id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes the asynchronous.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Queries the asynchronous.
	/// </summary>
	/// <param name="status">The status.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns></returns>
	IAsyncEnumerable<TodolistInfo> QueryAsync(TodolistStatus status, CancellationToken cancellationToken = default);
}
