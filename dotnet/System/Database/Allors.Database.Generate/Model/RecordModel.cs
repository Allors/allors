namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Allors.Database.Meta;
using Database.Population;

public class RecordModel
{
    public RecordModel(Model model, Record record)
    {
        this.Model = model;
        this.Record = record;
    }

    public Model Model { get; }

    public Record Record { get; }

    public ClassModel ClassType => this.Model.Map(this.Record.Class);

    public HandleModel Handle => this.Model.Map(this.Record.Handle);

    private IDictionary<RoleTypeModel, object> ValueByRoleType => this.Record.ValueByRoleType.ToDictionary(kvp => this.Model.Map(kvp.Key), kvp => kvp.Value);
}
