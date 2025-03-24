using GHLearning.Lifetimes.Core;
using GHLearning.Lifetimes.Core.Modesl;
using GHLearning.Lifetimes.Services;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace GHLearning.Lifetimes.ServicesTests;

public class TodolistServiceTests
{
	[Fact]
	public async Task Add()
	{
		var fakeLog = NullLogger<TodolistService>.Instance;
		var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

		var source = new TodolistAdd(
			Title: "Title",
			Content: "Content");

		var sut = new TodolistService(
			Guid.NewGuid(),
			fakeLog,
			fakeTodolistRepository);

		await sut.AddAsync(source);

		_ = fakeTodolistRepository
			.Received()
			.AddAsync(
			source: Arg.Is<TodolistAdd>(compare => compare.Title == source.Title && compare.Content == source.Content),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Query()
	{
		var fakeLog = NullLogger<TodolistService>.Instance;
		var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

		var info = new TodolistInfo(
			Id: Guid.NewGuid(),
			Title: "Title",
			Content: "Content",
			Status: TodolistStatus.Pending,
			CreatedAt: DateTimeOffset.UtcNow,
			UpdatedAt: DateTimeOffset.UtcNow);
		fakeTodolistRepository.QueryAsync(
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { info }.ToAsyncEnumerable());

		var sut = new TodolistService(
			Guid.NewGuid(),
			fakeLog,
			fakeTodolistRepository);

		var actual = await sut.QueryAsync().ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal(info.Id, actual.ElementAt(0).Id);
		Assert.Equal(info.Title, actual.ElementAt(0).Title);
		Assert.Equal(info.Content, actual.ElementAt(0).Content);
		Assert.Equal(info.Status, actual.ElementAt(0).Status);
		Assert.Equal(info.CreatedAt, actual.ElementAt(0).CreatedAt);
		Assert.Equal(info.UpdatedAt, actual.ElementAt(0).UpdatedAt);
	}

	[Fact]
	public async Task GetById()
	{
		var fakeLog = NullLogger<TodolistService>.Instance;
		var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

		var id = Guid.NewGuid();

		var info = new TodolistInfo(
			Id: id,
			Title: "Title",
			Content: "Content",
			Status: TodolistStatus.Pending,
			CreatedAt: DateTimeOffset.UtcNow,
			UpdatedAt: DateTimeOffset.UtcNow);
		fakeTodolistRepository.GetByIdAsync(
			id: id,
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(info);

		var sut = new TodolistService(
			Guid.NewGuid(),
			fakeLog,
			fakeTodolistRepository);

		var actual = await sut.GetByIdAsync(id);

		Assert.NotNull(actual);
		Assert.Equal(info.Id, actual.Id);
		Assert.Equal(info.Title, actual.Title);
		Assert.Equal(info.Content, actual.Content);
		Assert.Equal(info.Status, actual.Status);
		Assert.Equal(info.CreatedAt, actual.CreatedAt);
		Assert.Equal(info.UpdatedAt, actual.UpdatedAt);
	}

	[Fact]
	public async Task Completed()
	{
		var fakeLog = NullLogger<TodolistService>.Instance;
		var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

		var id = Guid.NewGuid();

		var sut = new TodolistService(
			Guid.NewGuid(),
			fakeLog,
			fakeTodolistRepository);

		await sut.CompletedAsync(id);

		_ = fakeTodolistRepository
			.Received()
			.CompletedAsync(
			Arg.Is(id),
			Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Delete()
	{
		var fakeLog = NullLogger<TodolistService>.Instance;
		var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

		var id = Guid.NewGuid();

		var sut = new TodolistService(
			Guid.NewGuid(),
			fakeLog,
			fakeTodolistRepository);

		await sut.DeleteAsync(id);

		_ = fakeTodolistRepository
			.Received()
			.DeleteAsync(
			Arg.Is(id),
			Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Query_TodolistStatus()
	{
		var fakeLog = NullLogger<TodolistService>.Instance;
		var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

		var info = new TodolistInfo(
			Id: Guid.NewGuid(),
			Title: "Title",
			Content: "Content",
			Status: TodolistStatus.Pending,
			CreatedAt: DateTimeOffset.UtcNow,
			UpdatedAt: DateTimeOffset.UtcNow);
		fakeTodolistRepository.QueryAsync(
			status: Arg.Is(TodolistStatus.Pending),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { info }.ToAsyncEnumerable());

		var sut = new TodolistService(
			Guid.NewGuid(),
			fakeLog,
			fakeTodolistRepository);

		var actual = await sut.QueryAsync(TodolistStatus.Pending).ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal(info.Id, actual.ElementAt(0).Id);
		Assert.Equal(info.Title, actual.ElementAt(0).Title);
		Assert.Equal(info.Content, actual.ElementAt(0).Content);
		Assert.Equal(info.Status, actual.ElementAt(0).Status);
		Assert.Equal(info.CreatedAt, actual.ElementAt(0).CreatedAt);
		Assert.Equal(info.UpdatedAt, actual.ElementAt(0).UpdatedAt);
	}
}
