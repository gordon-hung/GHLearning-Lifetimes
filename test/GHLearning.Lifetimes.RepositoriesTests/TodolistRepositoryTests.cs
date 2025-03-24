using GHLearning.Lifetimes.Core;
using GHLearning.Lifetimes.Core.Modesl;
using GHLearning.Lifetimes.Repositories;
using GHLearning.Lifetimes.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace GHLearning.Lifetimes.RepositoriesTests;

public class TodolistRepositoryTests
{
	[Fact]
	public async Task Add()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Add)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var fakeContext = new SampleContext(options);
		_ = fakeContext.Database.EnsureDeleted();

		var fakeLog = NullLogger<TodolistRepository>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();

		var source = new TodolistAdd(
			Title: "Title",
			Content: "Content");

		var currentTime = DateTimeOffset.UtcNow;
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var id = Guid.NewGuid();
		_ = fakeSequentialGuidGenerator.NewIdAsync()
			.Returns(id);

		var sut = new TodolistRepository(
			Guid.NewGuid(),
			fakeLog,
			fakeTimeProvider,
			fakeContext,
			fakeSequentialGuidGenerator);

		await sut.AddAsync(source);
	}

	[Fact]
	public async Task Query()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Query)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var fakeContext = new SampleContext(options);
		_ = fakeContext.Database.EnsureDeleted();

		var entity = new Todolist
		{
			Id = Guid.NewGuid(),
			Title = "Title",
			Content = "Content",
			Status = (int)TodolistStatus.Pending,
			CreatedAt = DateTimeOffset.UtcNow.Date,
			UpdatedAt = DateTimeOffset.UtcNow.Date
		};

		_ = await fakeContext.Todolists.AddAsync(entity);
		_ = await fakeContext.SaveChangesAsync();

		var fakeLog = NullLogger<TodolistRepository>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();

		var sut = new TodolistRepository(
			Guid.NewGuid(),
			fakeLog,
			fakeTimeProvider,
			fakeContext,
			fakeSequentialGuidGenerator);

		var actual = await sut.QueryAsync().ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal(entity.Id, actual.ElementAt(0).Id);
		Assert.Equal(entity.Title, actual.ElementAt(0).Title);
		Assert.Equal(entity.Content, actual.ElementAt(0).Content);
		Assert.Equal(entity.Status, (int)actual.ElementAt(0).Status);
		Assert.Equal(entity.CreatedAt, actual.ElementAt(0).CreatedAt);
		Assert.Equal(entity.UpdatedAt, actual.ElementAt(0).UpdatedAt);
	}

	[Fact]
	public async Task Query_TodolistStatus()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Query)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var fakeContext = new SampleContext(options);
		_ = fakeContext.Database.EnsureDeleted();

		var entity = new Todolist
		{
			Id = Guid.NewGuid(),
			Title = "Title",
			Content = "Content",
			Status = (int)TodolistStatus.Pending,
			CreatedAt = DateTimeOffset.UtcNow.Date,
			UpdatedAt = DateTimeOffset.UtcNow.Date
		};

		_ = await fakeContext.Todolists.AddAsync(entity);
		_ = await fakeContext.SaveChangesAsync();

		var fakeLog = NullLogger<TodolistRepository>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();

		var sut = new TodolistRepository(
			Guid.NewGuid(),
			fakeLog,
			fakeTimeProvider,
			fakeContext,
			fakeSequentialGuidGenerator);

		var actual = await sut.QueryAsync(TodolistStatus.Pending).ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal(entity.Id, actual.ElementAt(0).Id);
		Assert.Equal(entity.Title, actual.ElementAt(0).Title);
		Assert.Equal(entity.Content, actual.ElementAt(0).Content);
		Assert.Equal(entity.Status, (int)actual.ElementAt(0).Status);
		Assert.Equal(entity.CreatedAt, actual.ElementAt(0).CreatedAt);
		Assert.Equal(entity.UpdatedAt, actual.ElementAt(0).UpdatedAt);
	}

	[Fact]
	public async Task GetById()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Query)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var fakeContext = new SampleContext(options);
		_ = fakeContext.Database.EnsureDeleted();

		var entity = new Todolist
		{
			Id = Guid.NewGuid(),
			Title = "Title",
			Content = "Content",
			Status = (int)TodolistStatus.Pending,
			CreatedAt = DateTimeOffset.UtcNow.Date,
			UpdatedAt = DateTimeOffset.UtcNow.Date
		};

		_ = await fakeContext.Todolists.AddAsync(entity);
		_ = await fakeContext.SaveChangesAsync();

		var fakeLog = NullLogger<TodolistRepository>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();

		var sut = new TodolistRepository(
			Guid.NewGuid(),
			fakeLog,
			fakeTimeProvider,
			fakeContext,
			fakeSequentialGuidGenerator);

		var actual = await sut.GetByIdAsync(entity.Id);

		Assert.NotNull(actual);
		Assert.Equal(entity.Id, actual.Id);
		Assert.Equal(entity.Title, actual.Title);
		Assert.Equal(entity.Content, actual.Content);
		Assert.Equal(entity.Status, (int)actual.Status);
		Assert.Equal(entity.CreatedAt, actual.CreatedAt);
		Assert.Equal(entity.UpdatedAt, actual.UpdatedAt);
	}

	[Fact]
	public async Task Completed()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Query)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var fakeContext = new SampleContext(options);
		_ = fakeContext.Database.EnsureDeleted();

		var entity = new Todolist
		{
			Id = Guid.NewGuid(),
			Title = "Title",
			Content = "Content",
			Status = (int)TodolistStatus.Pending,
			CreatedAt = DateTimeOffset.UtcNow.Date,
			UpdatedAt = DateTimeOffset.UtcNow.Date
		};

		_ = await fakeContext.Todolists.AddAsync(entity);
		_ = await fakeContext.SaveChangesAsync();

		var fakeLog = NullLogger<TodolistRepository>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();

		var sut = new TodolistRepository(
			Guid.NewGuid(),
			fakeLog,
			fakeTimeProvider,
			fakeContext,
			fakeSequentialGuidGenerator);

		await sut.CompletedAsync(entity.Id);

		var todolist = await fakeContext.Todolists.SingleAsync(x => x.Id == entity.Id);

		Assert.NotNull(todolist);
		Assert.Equal((int)TodolistStatus.Completed, todolist.Status);
	}

	[Fact]
	public async Task Delete()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Query)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var fakeContext = new SampleContext(options);
		_ = fakeContext.Database.EnsureDeleted();

		var entity = new Todolist
		{
			Id = Guid.NewGuid(),
			Title = "Title",
			Content = "Content",
			Status = (int)TodolistStatus.Pending,
			CreatedAt = DateTimeOffset.UtcNow.Date,
			UpdatedAt = DateTimeOffset.UtcNow.Date
		};

		_ = await fakeContext.Todolists.AddAsync(entity);
		_ = await fakeContext.SaveChangesAsync();

		var fakeLog = NullLogger<TodolistRepository>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();

		var sut = new TodolistRepository(
			Guid.NewGuid(),
			fakeLog,
			fakeTimeProvider,
			fakeContext,
			fakeSequentialGuidGenerator);

		await sut.DeleteAsync(entity.Id);

		var todolist = await fakeContext.Todolists.SingleAsync(x => x.Id == entity.Id);

		Assert.NotNull(todolist);
		Assert.Equal((int)TodolistStatus.Deleted, todolist.Status);
	}
}
