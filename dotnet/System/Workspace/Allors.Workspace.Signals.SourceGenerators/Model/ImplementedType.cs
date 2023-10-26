namespace Allors.Workspace.Mvvm.Generator;

using System.Linq;
using Microsoft.CodeAnalysis;

public class ImplementedType
{
    public ImplementedType(INamedTypeSymbol symbol)
    {
        this.Symbol = symbol;
        this.ArgumentTypes = this.Symbol.TypeArguments.ToArray().Select(v => new ImplementedArgumentType(v)).ToArray();
    }

    public INamedTypeSymbol Symbol { get; }

    public ImplementedArgumentType[] ArgumentTypes { get; }

    public string Name => this.Symbol.Name;

    public bool IsCompositeRoleWrapper => this.Name.Equals("ICompositeRoleWrapper");

    public bool IsCompositesRoleWrapper => this.Name.Equals("ICompositesRoleWrapper");
}
