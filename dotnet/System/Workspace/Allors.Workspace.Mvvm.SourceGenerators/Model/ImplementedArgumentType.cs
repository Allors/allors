namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;

public class ImplementedArgumentType
{
    public ImplementedArgumentType(ITypeSymbol symbol)
    {
        this.Symbol = symbol;
    }

    public ITypeSymbol Symbol { get; }

    public string Name => this.Symbol.Name;

    public string FullName => this.Symbol.ToDisplayString();
}
