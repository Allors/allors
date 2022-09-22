namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public class FieldType : MetaIdentifiableObject, IFieldType
{
    private string[] derivedWorkspaceNames;

    public FieldType(Record record, Guid id, string name, DataType dataType, bool isMany)
    : base(record.MetaPopulation, id)
    {
        this.Record = record;
        this.Name = name;
        this.DataType = dataType;
        this.IsMany = isMany;

        this.MetaPopulation.OnFieldTypeCreated(this);
    }

    IRecord IFieldType.Record => this.Record;

    public Record Record { get; }

    public string Name { get; }

    IDataType IFieldType.DataType => this.DataType;

    public DataType DataType { get; }

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    public bool IsOne
    {
        get => !this.IsMany;
    }

    public bool IsMany { get; }

    internal void DeriveWorkspaceNames() => this.derivedWorkspaceNames = this.DataType != null
        ? this.Record.WorkspaceNames.Intersect(this.DataType.WorkspaceNames).ToArray()
        : Array.Empty<string>();
}
