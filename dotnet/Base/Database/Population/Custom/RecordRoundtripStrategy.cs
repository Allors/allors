namespace Allors.Database.Population
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Meta;
    using Database.Population;
    using Meta.Configuration;

    public class RecordRoundtripStrategy : IRecordRoundtripStrategy
    {
        private readonly IDatabase database;
        private readonly IDictionary<IClass, Record[]> existingRecordsByClass;

        public RecordRoundtripStrategy(IDatabase database, IDictionary<IClass, Record[]> existingRecordsByClass)
        {
            this.database = database;
            this.existingRecordsByClass = existingRecordsByClass;
        }

        public IEnumerable<IObject> Objects()
        {
            using var transaction = this.database.CreateTransaction();

            var objects = this.existingRecordsByClass.Keys
                .Where(v => v.KeyRoleType != null)
                .SelectMany(transaction.Extent);
            return objects;
        }

        public Func<IStrategy, Handle> HandleResolver()
        {
            var m = this.database.Services.Get<M>();

            var excluded = new HashSet<IClass> { m.Country, m.Currency, m.Language };
            var fromExisting = HandleResolvers.FromExisting(this.existingRecordsByClass);
            var fromKey = HandleResolvers.PascalCaseKey();
            return strategy =>
            {
                if (excluded.Contains(strategy.Class))
                {
                    return null;
                }

                return fromExisting(strategy) ?? fromKey(strategy);
            };
        }

        public Func<IStrategy, IRoleType, bool> RoleFilter() => (strategy, roleType) =>
            strategy.ExistRole(roleType) &&
            roleType.ObjectType.IsUnit &&
            !roleType.RelationType.IsDerived &&
            !roleType.SingularName.Equals("ExternalPrimaryKey");
    }
}
