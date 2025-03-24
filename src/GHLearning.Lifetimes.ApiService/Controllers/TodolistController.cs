using GHLearning.Lifetimes.ApiService.ViewModels;
using GHLearning.Lifetimes.Core;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.Lifetimes.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodolistController(
	ILogger<TodolistController> logger) : ControllerBase
{
	[HttpGet]
	public async IAsyncEnumerable<TodolistViewModel> QueryAsync(
		[FromServices] ISequentialGuidGenerator sequentialGuidGenerator,
		[FromServices] ITodolistService todolistService,
		[FromServices] ITodolistRepository todolistRepository)
	{
		logger.LogInformation("AddSingleton {nameof} -> {id}", nameof(ISequentialGuidGenerator), sequentialGuidGenerator.GetLifetimeId());
		logger.LogInformation("AddScoped {nameof} -> {id}", nameof(ITodolistService), todolistService.GetLifetimeId());
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());

		var todolists = await todolistService
			.QueryAsync(HttpContext.RequestAborted)
			.ToArrayAsync(HttpContext.RequestAborted)
			.ConfigureAwait(false);

		foreach (var todolist in todolists)
		{
			yield return new TodolistViewModel(
				Id: todolist.Id,
				Title: todolist.Title,
				Content: todolist.Content,
				Status: todolist.Status,
				CreatedAt: todolist.CreatedAt,
				UpdatedAt: todolist.UpdatedAt);
		}
	}

	[HttpPost]
	public Task AddAsync(
		[FromServices] ISequentialGuidGenerator sequentialGuidGenerator,
		[FromServices] ITodolistService todolistService,
		[FromServices] ITodolistRepository todolistRepository,
		[FromBody] TodolistAddViewModel source)
	{
		logger.LogInformation("AddSingleton {nameof} -> {id}", nameof(ISequentialGuidGenerator), sequentialGuidGenerator.GetLifetimeId());
		logger.LogInformation("AddScoped {nameof} -> {id}", nameof(ITodolistService), todolistService.GetLifetimeId());
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());

		return todolistService.AddAsync(
			source: new Core.Modesl.TodolistAdd(
				Title: source.Title,
				Content: source.Content),
			cancellationToken: HttpContext.RequestAborted);
	}

	[HttpPatch("{id}/Completed")]
	public Task CompletedAsync(
		Guid id,
		[FromServices] ISequentialGuidGenerator sequentialGuidGenerator,
		[FromServices] ITodolistService todolistService,
		[FromServices] ITodolistRepository todolistRepository)
	{
		logger.LogInformation("AddSingleton {nameof} -> {id}", nameof(ISequentialGuidGenerator), sequentialGuidGenerator.GetLifetimeId());
		logger.LogInformation("AddScoped {nameof} -> {id}", nameof(ITodolistService), todolistService.GetLifetimeId());
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());

		return todolistService.CompletedAsync(
			id: id,
			cancellationToken: HttpContext.RequestAborted);
	}

	[HttpDelete("{id}")]
	public Task DeleteAsync(
		Guid id,
		[FromServices] ISequentialGuidGenerator sequentialGuidGenerator,
		[FromServices] ITodolistService todolistService,
		[FromServices] ITodolistRepository todolistRepository)
	{
		logger.LogInformation("AddSingleton {nameof} -> {id}", nameof(ISequentialGuidGenerator), sequentialGuidGenerator.GetLifetimeId());
		logger.LogInformation("AddScoped {nameof} -> {id}", nameof(ITodolistService), todolistService.GetLifetimeId());
		logger.LogInformation("AddTransient {nameof} -> {id}", nameof(ITodolistRepository), todolistRepository.GetLifetimeId());

		return todolistService.DeleteAsync(
			id: id,
			cancellationToken: HttpContext.RequestAborted);
	}
}
