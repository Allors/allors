namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public class Record : DataType, IRecord
{
    private string[] derivedWorkspaceNames;
    private FieldType[] derivedFieldTypes;

    public Record(MetaPopulation metaPopulation, Guid id, string name)
        : base(metaPopulation, id)
    {
        this.Name = name;

        metaPopulation.OnCreated(this);
    }

    IFieldType[] IRecord.FieldTypes => this.FieldTypes;

    public FieldType[] FieldTypes
    {
        get => this.derivedFieldTypes;
    }

    public override string Name { get; }

    public override IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    internal void Bind(Dictionary<string, Type> typeByTypeName) => this.BoundType = typeByTypeName[this.Name];

    internal void DeriveWorkspaceNames(IDictionary<Record, ISet<string>> workspaceNamesByRecord)
    {
        workspaceNamesByRecord.TryGetValue(this, out var workspaceNames);
        this.derivedWorkspaceNames = workspaceNames?.ToArray() ?? Array.Empty<string>();
    }

    internal void PrepareWorkspaceNames(IDictionary<Record, ISet<string>> workspaceNamesByRecord, ISet<Record> visited, string[] methodWorkspaceNames)
    {
        if (visited.Contains(this))
        {
            return;
        }

        visited.Add(this);

        if (!workspaceNamesByRecord.TryGetValue(this, out var workspaceNames))
        {
            workspaceNames = new HashSet<string>();
            workspaceNamesByRecord.Add(this, workspaceNames);
        }

        workspaceNames.UnionWith(methodWorkspaceNames);

        foreach (var fieldType in this.FieldTypes.Where(v => v.DataType is Record))
        {
            var record = (Record)fieldType.DataType;
            record.PrepareWorkspaceNames(workspaceNamesByRecord, visited, this.derivedWorkspaceNames);
        }
    }

    internal void StructuralDeriveFieldTypes(Dictionary<Record, FieldType[]> fieldTypesByRecord)
    {
        this.derivedFieldTypes = fieldTypesByRecord.TryGetValue(this, out var fieldTypes) ?
            fieldTypes :
            Array.Empty<FieldType>();
    }

    internal void Validate(ValidationLog log)
    {
    }
}
