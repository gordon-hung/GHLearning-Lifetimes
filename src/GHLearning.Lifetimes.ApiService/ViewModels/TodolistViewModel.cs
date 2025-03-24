using GHLearning.Lifetimes.Core;

namespace GHLearning.Lifetimes.ApiService.ViewModels;

public record TodolistViewModel(
	 Guid Id,
	 string Title,
	 string Content,
	 TodolistStatus Status,
	 DateTimeOffset CreatedAt,
	 DateTimeOffset UpdatedAt);
