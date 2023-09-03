namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;

public class AttributeInstance
{
    public AttributeInstance(Field field, AttributeData attributeData)
    {
        this.Field = field;
        this.AttributeData = attributeData;
        this.Name = this.AttributeData.AttributeClass.Name;
    }

    public Field Field { get; }

    public AttributeData AttributeData { get; }

    public string Name { get; }

    public void Build()
    {

    }
}
