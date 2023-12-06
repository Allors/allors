namespace Allors.Workspace.Mvvm.Generator;

public record Configuration(string? OnEffect)
{
    public string OnEffect { get; } = OnEffect ?? "OnEffect";
}
