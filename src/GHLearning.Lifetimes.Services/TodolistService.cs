using GHLearning.Lifetimes.Core;
using GHLearning.Lifetimes.Core.Modesl;
using Microsoft.Extensions.Logging;
using SequentialGuid;

namespace GHLearning.Lifetimes.Services;

internal class TodolistService(
	Guid lifetimeId,
	ILogger<TodolistService> logger,
	ITodolistRepository todolistRepository) : ITodolistService
{
	public Guid GetLifetimeId() => lifetimeId;
	public Task AddAsync(TodolistAdd source, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());
		return todolistRepository.AddAsync(source, cancellationToken);
	}
	public Task CompletedAsync(Guid id, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());
		return todolistRepository.CompletedAsync(id, cancellationToken);
	}
	public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());
		return todolistRepository.DeleteAsync(id, cancellationToken);
	}
	public Task<TodolistInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());
		return todolistRepository.GetByIdAsync(id, cancellationToken);
	}
	public IAsyncEnumerable<TodolistInfo> QueryAsync(CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());
		return todolistRepository.QueryAsync(cancellationToken);
	}
	public IAsyncEnumerable<TodolistInfo> QueryAsync(TodolistStatus status, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());
		return todolistRepository.QueryAsync(status, cancellationToken);
	}
}
