namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;

public record Configuration(string? OnEffect)
{
    public string OnEffect { get; } = OnEffect ?? "OnEffect";
}
