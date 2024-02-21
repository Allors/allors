namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Allors.Database.Meta;
using Database.Population;

public class HandleModel
{
    public HandleModel(Model model, Handle handle)
    {
        this.Model = model;
        this.Handle = handle;
    }

    public Model Model { get; }

    public Handle Handle { get; }

    public RoleTypeModel RoleType => this.Model.Map(this.Handle.RoleType);

    public string Name => this.Handle.Name;

    public object Value => this.Handle.Value;
}
