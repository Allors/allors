namespace Allors.Workspace.Signals.Tests
{
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Data;
    using Domain;
    using Mvvm;
    using Mvvm.Adapters;

    public class UnitRoleTest : Test
    {
        [Test]
        public async Task UnitRoleExpressionAdapterTest()
        {
            await this.Login("jane@example.com");

            var workspace = this.Workspace;
            var reactiveFuncBuilder = workspace.Services.Get<IReactiveFuncBuilder>();
            var reactiveExpressionBuilder = workspace.Services.Get<IReactiveExpressionBuilder>();

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite || !c1c.C1AllorsString.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy }, new Pull { Object = c1c.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1AllorsString.Value = "Hello";

            var propertyChange = new ViewModel();

            Expression<Func<C1, IUnitRole<string>>> expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString;
            var reactiveFunc = reactiveFuncBuilder.Build(expression);
            var reactiveExpression = reactiveExpressionBuilder.Build(c1a, reactiveFunc);

            var adapter = new UnitRoleExpressionAdapter<C1, String>(propertyChange, reactiveExpression, "String");

            // Value Get
            Assert.AreEqual("Hello", adapter.Value);
            Assert.IsEmpty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual("Hello Again", adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.IsEmpty(propertyChange.Events);
            Assert.AreEqual("Hello Again", adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual("Another Hello", adapter.Value);

            // Value Set
            propertyChange.Events.Clear();

            adapter.Value = "Hello from Value Set";

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual("Hello from Value Set", adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1AllorsString.Value = "Hello from Value Set";

            Assert.IsEmpty(propertyChange.Events);
            Assert.AreEqual("Hello from Value Set", adapter.Value);
            Assert.IsEmpty(propertyChange.Events);
        }
    }
}
