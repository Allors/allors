namespace Allors.Workspace.Mvvm.Generator;

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

public class Project
{
    private const string AttributeName = "Allors.Workspace.Mvvm.Generator.SignalPropertyAttribute";

    public Project(SourceProductionContext sourceProductionContext, Compilation compilation, Configuration configuration)
    {
        this.Context = sourceProductionContext;
        this.Compilation = compilation;
        this.Configuration = configuration;
        this.AttributeNamedTypeSymbol = this.Compilation.GetTypeByMetadataName(AttributeName);
    }

    public SourceProductionContext Context { get; }

    public Compilation Compilation { get; }

    public INamedTypeSymbol AttributeNamedTypeSymbol { get; }

    public Configuration Configuration { get; set; }

    public Source[] Sources { get; private set; }

    public IEnumerable<Class> Classes => this.Sources
        .SelectMany(v => v.Classes);

    public void Build()
    {
        this.Sources = this.Compilation.SyntaxTrees.Select(v => new Source(this, v)).ToArray();

        foreach (var sourceModel in this.Sources)
        {
            sourceModel.Build();
        }
    }

    public void Generate()
    {
        var classes = this.Classes
            .Where(v => v.SignalFields.Any());

        foreach (var @class in classes)
        {
            @class.Generate();
        }
    }
}
