namespace GHLearning.Lifetimes.Core.Modesl;

public record TodolistInfo(
	 Guid Id,
	 string Title,
	 string Content,
	 TodolistStatus Status,
	 DateTimeOffset CreatedAt,
	 DateTimeOffset UpdatedAt);
