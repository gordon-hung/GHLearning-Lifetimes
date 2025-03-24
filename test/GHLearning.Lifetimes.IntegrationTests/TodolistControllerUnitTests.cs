using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using GHLearning.Lifetimes.ApiService.ViewModels;
using NSubstitute;
using GHLearning.Lifetimes.Core;
using GHLearning.Lifetimes.Core.Modesl;
using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.Lifetimes.IntegrationTests;

public class TodolistControllerUnitTests
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	[Fact]
	public async Task Add()
	{
		var source = new TodolistAddViewModel
		(
			Title: "Title",
			Content: "Content"
		);

		var web = new ApiServiceApplicationTests(builder =>
		{
			var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
			var fakeTodolistService = Substitute.For<ITodolistService>();
			var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

			_ = fakeTodolistService.AddAsync(
						source: Arg.Is<TodolistAdd>(compare => compare.Title == source.Title && compare.Content == source.Content),
						cancellationToken: Arg.Any<CancellationToken>());

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSequentialGuidGenerator));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistService));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistRepository));
		});

		var httpClient = web.CreateDefaultClient();

		var jsonContent = JsonContent.Create(source);
		var httpResponseMessage = await httpClient.PostAsync(
			"/api/Todolist",
			jsonContent);

		_ = httpResponseMessage.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task Query()
	{
		var info = new TodolistInfo(
			Id: Guid.NewGuid(),
			Title: "Title",
			Content: "Content",
			Status: TodolistStatus.Pending,
			CreatedAt: DateTimeOffset.UtcNow,
			UpdatedAt: DateTimeOffset.UtcNow);

		var web = new ApiServiceApplicationTests(builder =>
		{
			var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
			var fakeTodolistService = Substitute.For<ITodolistService>();
			var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

			_ = fakeTodolistService.QueryAsync(
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { info }.ToAsyncEnumerable());

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSequentialGuidGenerator));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistService));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistRepository));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.GetAsync($"/api/Todolist");

		var actual = JsonSerializer.Deserialize<IEnumerable<TodolistViewModel>>(json: await httpResponseMessage.Content.ReadAsStringAsync(), options: _SerializerOptions);

		Assert.NotNull(actual);
		Assert.Equal(info.Id, actual.ElementAt(0).Id);
		Assert.Equal(info.Title, actual.ElementAt(0).Title);
		Assert.Equal(info.Content, actual.ElementAt(0).Content);
		Assert.Equal(info.Status, actual.ElementAt(0).Status);
		Assert.Equal(info.CreatedAt, actual.ElementAt(0).CreatedAt);
		Assert.Equal(info.UpdatedAt, actual.ElementAt(0).UpdatedAt);
	}

	[Fact]
	public async Task Completed()
	{
		var id = Guid.NewGuid();

		var web = new ApiServiceApplicationTests(builder =>
		{
			var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
			var fakeTodolistService = Substitute.For<ITodolistService>();
			var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

			_ = fakeTodolistService.CompletedAsync(
				id: Arg.Is(id),
				cancellationToken: Arg.Any<CancellationToken>());

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSequentialGuidGenerator));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistService));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistRepository));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.PatchAsync(
			requestUri: $"/api/Todolist/{id}/Completed",
			content: null);

		_ = httpResponseMessage.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task Delete()
	{
		var id = Guid.NewGuid();

		var web = new ApiServiceApplicationTests(builder =>
		{
			var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
			var fakeTodolistService = Substitute.For<ITodolistService>();
			var fakeTodolistRepository = Substitute.For<ITodolistRepository>();

			_ = fakeTodolistService.DeleteAsync(
				id: Arg.Is(id),
				cancellationToken: Arg.Any<CancellationToken>());

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSequentialGuidGenerator));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistService));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeTodolistRepository));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.DeleteAsync(
			requestUri: $"/api/Todolist/{id}");

		_ = httpResponseMessage.EnsureSuccessStatusCode();
	}
}
