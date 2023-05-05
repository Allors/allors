namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Meta;
    using Xunit;

    public abstract class Context
    {
        protected Context(Test test, string name)
        {
            this.Test = test;
            this.Name = name;

        }

        public Test Test { get; }

        public string Name { get; }


        public IWorkspace Workspace1 { get; protected set; }


        public IWorkspace Workspace2 { get; protected set; }


        public void Deconstruct(out IWorkspace workspace1, out IWorkspace workspace2)
        {
            workspace1 = this.Workspace1;
            workspace2 = this.Workspace2;
        }

        public async Task<T> Create<T>(IWorkspace workspace, DatabaseMode mode) where T : class, IObject
        {
            T result;
            switch (mode)
            {
                case DatabaseMode.NoPush:
                    result = workspace.Create<T>();
                    break;
                case DatabaseMode.Push:
                    var pushObject = workspace.Create<T>();
                    await workspace.PushAsync();
                    result = pushObject;
                    break;
                case DatabaseMode.PushAndPull:
                    result = workspace.Create<T>();
                    var pushResult = await workspace.PushAsync();
                    Assert.False(pushResult.HasErrors);
                    await workspace.PullAsync(new Pull { Object = result });
                    break;
                case DatabaseMode.SharedDatabase:
                    var sharedDatabaseObject = this.Workspace1.Create<T>();
                    await this.Workspace1.PushAsync();
                    var sharedResult = await workspace.PullAsync(new Pull { Object = sharedDatabaseObject });
                    result = (T)sharedResult.Objects.Values.First();
                    break;
                case DatabaseMode.ExclusiveDatabase:
                    var exclusiveDatabaseObject = this.Workspace2.Create<T>();
                    await this.Workspace2.PushAsync();
                    var exclusiveResult = await workspace.PullAsync(new Pull { Object = exclusiveDatabaseObject });
                    result = (T)exclusiveResult.Objects.Values.First();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, $@"Mode [{string.Join(", ", Enum.GetNames(typeof(DatabaseMode)))}]");
            }

            Assert.NotNull(result);
            return result;
        }

        public override string ToString() => this.Name;
    }
}
