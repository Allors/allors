namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public class FieldType : IFieldType
{
    private string[] derivedWorkspaceNames;
    private string name;
    private FieldObjectType fieldObjectType;
    private bool isOne;

    public FieldType(Record record, Guid id, string tag = null)
    {
        this.Record = record;
        this.Id = id;
        this.Tag = tag ?? id.Tag();

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

    public FieldObjectType FieldObjectType
    {
        get => this.fieldObjectType;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.fieldObjectType = value;
            this.MetaPopulation.Stale();
        }
    }

    public MetaPopulation MetaPopulation => this.Record.MetaPopulation;

    public IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    public Guid Id { get; }

    public string Tag { get; }

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

    IFieldObjectType IFieldType.FieldObjectType => this.FieldObjectType;

    IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

    internal void DeriveWorkspaceNames() => this.derivedWorkspaceNames = this.FieldObjectType != null
        ? this.Record.WorkspaceNames.Intersect(this.FieldObjectType.WorkspaceNames).ToArray()
        : Array.Empty<string>();
}
