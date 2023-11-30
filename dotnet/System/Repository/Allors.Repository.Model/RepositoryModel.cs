namespace Allors.Repository.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Graph;
using Allors.Repository;
using Allors.Repository.Domain;
using NLog;

public class RepositoryModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly Dictionary<RepositoryObject, RepositoryObjectModel> mapping;

    public RepositoryModel(Repository repository)
    {
        this.Repository = repository;

        this.mapping = new Dictionary<RepositoryObject, RepositoryObjectModel>();

        foreach (var @object in this.Repository.Objects)
        {
            switch (@object)
            {
                case Domain domain:
                    this.mapping.Add(domain, new DomainModel(this, domain));
                    break;
                case Unit unit:
                    this.mapping.Add(unit, new UnitModel(this, unit));
                    break;
                case Interface @interface:
                    this.mapping.Add(@interface, new InterfaceModel(this, @interface));
                    break;
                case Class @class:
                    this.mapping.Add(@class, new ClassModel(this, @class));
                    break;
                case Property property:
                    this.mapping.Add(property, new PropertyModel(this, property));
                    break;
                case Method method:
                    this.mapping.Add(method, new MethodModel(this, method));
                    break;
                default:
                    throw new Exception($"Missing mapping for {@object}");
            }
        }

        this.Objects = this.Repository.Objects.Select(this.Map).ToArray();
        this.Units = this.Objects.OfType<UnitModel>().ToArray();
        this.Classes = this.Objects.OfType<ClassModel>().ToArray();

        Array.Sort(this.Objects);
        Array.Sort(this.Units);
        Array.Sort(this.Classes);

        this.Domains = new Graph<DomainModel>(this.Objects.OfType<DomainModel>(), v => v.DirectSuperdomains);
        this.Composites = new Graph<CompositeModel>(this.Objects.OfType<CompositeModel>(), v => v.Interfaces);
        this.Interfaces = new Graph<InterfaceModel>(this.Objects.OfType<InterfaceModel>(), v => v.Interfaces);

        // Validations
        var ids = new HashSet<Guid>();

        foreach (var composite in this.Objects.OfType<CompositeModel>())
        {
            this.CheckId(ids, composite.Id, $"{composite}", "id");
        }

        foreach (var property in this.Objects.OfType<PropertyModel>().Where(v => v.DefiningProperty == null))
        {
            this.CheckId(ids, property.Id, $"{property}", "id");
        }

        foreach (var method in this.Objects.OfType<MethodModel>().Where(v => v.DefiningMethod == null))
        {
            this.CheckId(ids, method.Id, $"{method}", "id");
        }
    }

    public bool HasErrors { get; set; }

    public Repository Repository { get; }

    public RepositoryObjectModel[] Objects { get; }

    public Graph<DomainModel> Domains { get; }

    public UnitModel[] Units { get; }

    public Graph<CompositeModel> Composites { get; }

    public Graph<InterfaceModel> Interfaces { get; }

    public ClassModel[] Classes { get; }

    private void CheckId(ISet<Guid> ids, string id, string name, string key)
    {
        if (!Guid.TryParse(id, out var idGuid))
        {
            this.HasErrors = true;
            Logger.Error($"{name} has a non GUID {key}: {id}");
        }

        this.CheckId(ids, idGuid, name, key);
    }

    private void CheckId(ISet<Guid> ids, Guid id, string name, string key)
    {
        if (ids.Contains(id))
        {
            this.HasErrors = true;
            Logger.Error($"{name} has a duplicate {key}: {id}");
        }

        ids.Add(id);
    }

    #region Mappers
    public RepositoryObjectModel Map(RepositoryObject v) => v != null ? this.mapping[v] : null;

    public ObjectTypeModel Map(ObjectType v) => v != null ? (ObjectTypeModel)this.mapping[v] : null;

    public CompositeModel Map(Composite v) => v != null ? (CompositeModel)this.mapping[v] : null;

    public InterfaceModel Map(Interface v) => v != null ? (InterfaceModel)this.mapping[v] : null;

    public ClassModel Map(Class v) => v != null ? (ClassModel)this.mapping[v] : null;

    public UnitModel Map(Unit v) => v != null ? (UnitModel)this.mapping[v] : null;

    public DomainModel Map(Domain v) => v != null ? (DomainModel)this.mapping[v] : null;

    public PropertyModel Map(Property v) => v != null ? (PropertyModel)this.mapping[v] : null;

    public MethodModel Map(Method v) => v != null ? (MethodModel)this.mapping[v] : null;
    #endregion
}
