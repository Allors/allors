namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public class FieldType : MetaIdentifiableObject, IFieldType
{
    private string[] derivedWorkspaceNames;
    private string name;
    private DataType dataType;
    private bool isOne;

    public FieldType(Record record, Guid id)
    : base(record.MetaPopulation, id)
    {
        this.Record = record;

        this.MetaPopulation.OnFieldTypeCreated(this);
    }

    public Record Record
    {
        get;
    }

    public string Name
    {
        get => this.name;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.name = value;
            this.MetaPopulation.Stale();
        }
    }

    public DataType DataType
    {
        get => this.dataType;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.dataType = value;
            this.MetaPopulation.Stale();
        }
    }

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
        get => this.isOne;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.isOne = value;
            this.MetaPopulation.Stale();
        }
    }

    public bool IsMany
    {
        get => !this.isOne;

        set => this.IsOne = !value;
    }

    IRecord IFieldType.Record => this.Record;

    IDataType IFieldType.DataType => this.DataType;

    internal void DeriveWorkspaceNames() => this.derivedWorkspaceNames = this.DataType != null
        ? this.Record.WorkspaceNames.Intersect(this.DataType.WorkspaceNames).ToArray()
        : Array.Empty<string>();
}
