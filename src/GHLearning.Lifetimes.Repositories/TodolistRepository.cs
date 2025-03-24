using GHLearning.Lifetimes.Core;
using GHLearning.Lifetimes.Core.Modesl;
using GHLearning.Lifetimes.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GHLearning.Lifetimes.Repositories;

internal class TodolistRepository(
	Guid lifetimeId,
	ILogger<TodolistRepository> logger,
	TimeProvider timeProvider,
	SampleContext sampleContext,
	ISequentialGuidGenerator sequentialGuidGenerator) : ITodolistRepository
{
	public Guid GetLifetimeId() => lifetimeId;

	public async Task AddAsync(TodolistAdd source, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("AddSingleton {nameof} -> {id}", nameof(ISequentialGuidGenerator), sequentialGuidGenerator.GetLifetimeId());

		var id = await sequentialGuidGenerator.NewIdAsync(cancellationToken).ConfigureAwait(false);

		var currentTime = timeProvider.GetUtcNow();

		var entity = new Todolist
		{
			Id = id,
			Title = source.Title,
			Content = source.Content,
			Status = (int)TodolistStatus.Pending,
			CreatedAt = currentTime.UtcDateTime,
			UpdatedAt = currentTime.UtcDateTime
		};

		await sampleContext.Todolists.AddAsync(entity, cancellationToken).ConfigureAwait(false);

		await sampleContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async Task CompletedAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await sampleContext.Todolists.SingleAsync(x => x.Id == id && x.Status == (int)TodolistStatus.Pending, cancellationToken).ConfigureAwait(false);

		if (entity.Status == (int)TodolistStatus.Completed)
		{
			throw new InvalidOperationException(nameof(TodolistStatus.Completed));
		}

		entity.Status = (int)TodolistStatus.Completed;

		sampleContext.Todolists.Update(entity);

		await sampleContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await sampleContext.Todolists.SingleAsync(x => x.Id == id && x.Status == (int)TodolistStatus.Pending, cancellationToken).ConfigureAwait(false);

		if (entity.Status == (int)TodolistStatus.Deleted)
		{
			throw new InvalidOperationException(nameof(TodolistStatus.Deleted));
		}

		entity.Status = (int)TodolistStatus.Deleted;

		sampleContext.Todolists.Update(entity);

		await sampleContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async Task<TodolistInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await sampleContext.Todolists.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

		return entity == null
			? null
			: new TodolistInfo(
				Id: entity.Id,
				Title: entity.Title,
				Content: entity.Content,
				Status: (TodolistStatus)entity.Status,
				CreatedAt: entity.CreatedAt,
				UpdatedAt: entity.UpdatedAt
			);
	}

	public IAsyncEnumerable<TodolistInfo> QueryAsync(CancellationToken cancellationToken = default)
	=> sampleContext.Todolists
		.Where(entity => entity.Status != (int)TodolistStatus.Deleted)
		.Select(entity => new TodolistInfo(
			entity.Id,
			entity.Title,
			entity.Content,
			(TodolistStatus)entity.Status,
			entity.CreatedAt,
			entity.UpdatedAt
			))
		.AsAsyncEnumerable();

	public IAsyncEnumerable<TodolistInfo> QueryAsync(TodolistStatus status, CancellationToken cancellationToken = default)
	=> sampleContext.Todolists
		.Where(entity => entity.Status != (int)TodolistStatus.Deleted && entity.Status == (int)status)
		.Select(entity => new TodolistInfo(
			entity.Id,
			entity.Title,
			entity.Content,
			(TodolistStatus)entity.Status,
			entity.CreatedAt,
			entity.UpdatedAt
			))
		.AsAsyncEnumerable();
}
