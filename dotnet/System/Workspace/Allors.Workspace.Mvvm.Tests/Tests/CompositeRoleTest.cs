namespace Allors.Workspace.Signals.Tests
{
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Data;
    using Domain;
    using Mvvm;
    using Mvvm.Adapters;

    public class CompositeRoleTest : Test
    {
        [Test]
        public async Task CompositeRoleExpressionAdapterTest()
        {
            await this.Login("jane@example.com");

            var workspace = this.Workspace;
            var reactiveFuncBuilder = workspace.Services.Get<IReactiveFuncBuilder>();
            var reactiveExpressionBuilder = workspace.Services.Get<IReactiveExpressionBuilder>();

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();
            var c1e = workspace.Create<C1>();
            var c1f = workspace.Create<C1>();
            var c1g = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite || !c1c.C1C1One2One.CanWrite || !c1d.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy }, new Pull { Object = c1c.Strategy }, new Pull { Object = c1d.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1C1One2One.Value = c1e;

            var propertyChange = new ViewModel();

            Expression<Func<C1, ICompositeRole<C1>>> expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1C1One2One;
            var reactiveFunc = reactiveFuncBuilder.Build(expression);
            var reactiveExpression = reactiveExpressionBuilder.Build(c1a, reactiveFunc);

            var adapter = new CompositeRoleExpressionAdapter<C1, C1>(propertyChange, reactiveExpression, "String");

            // Value Get
            Assert.AreEqual(c1e, adapter.Value);
            Assert.IsEmpty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1C1One2One.Value = c1f;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual(c1f, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1g;

            Assert.IsEmpty(propertyChange.Events);
            Assert.AreEqual(c1f, adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual(c1g, adapter.Value);

            // Value Set
            propertyChange.Events.Clear();

            adapter.Value = c1f;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual(c1f, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1f;

            Assert.IsEmpty(propertyChange.Events);
            Assert.AreEqual(c1f, adapter.Value);
            Assert.IsEmpty(propertyChange.Events);
        }

        [Test]
        public async Task ViewModelCompositeRoleExpressionAdapterTest()
        {
            await this.Login("jane@example.com");

            var workspace = this.Workspace;
            var reactiveFuncBuilder = workspace.Services.Get<IReactiveFuncBuilder>();
            var reactiveExpressionBuilder = workspace.Services.Get<IReactiveExpressionBuilder>();

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();
            var c1e = workspace.Create<C1>();
            var c1f = workspace.Create<C1>();
            var c1g = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite || !c1c.C1C1One2One.CanWrite ||
                !c1d.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy },
                    new Pull { Object = c1c.Strategy }, new Pull { Object = c1d.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1C1One2One.Value = c1e;

            var propertyChange = new ViewModel();

            Expression<Func<C1, ICompositeRole<C1>>>
                expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1C1One2One;
            var reactiveFunc = reactiveFuncBuilder.Build(expression);
            var reactiveExpression = reactiveExpressionBuilder.Build(c1a, reactiveFunc);

            var resolver = new Resolver(v => v switch
            {
                C1 c1 => new C1ViewModel(c1),
                _ => throw new ArgumentException()
            });

            var c1aViewModel = (C1ViewModel)resolver.Resolve(c1a);
            var c1bViewModel = (C1ViewModel)resolver.Resolve(c1b);
            var c1cViewModel = (C1ViewModel)resolver.Resolve(c1c);
            var c1dViewModel = (C1ViewModel)resolver.Resolve(c1d);
            var c1eViewModel = (C1ViewModel)resolver.Resolve(c1e);
            var c1fViewModel = (C1ViewModel)resolver.Resolve(c1f);
            var c1gViewModel = (C1ViewModel)resolver.Resolve(c1g);

            var adapter = new CompositeRoleExpressionViewModelAdapter<C1, C1ViewModel>(propertyChange, resolver, reactiveExpression, "String");

            // Value Get
            Assert.AreEqual(c1eViewModel, adapter.Value);
            Assert.IsEmpty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1C1One2One.Value = c1f;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual(c1fViewModel, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1g;

            Assert.IsEmpty(propertyChange.Events);
            Assert.AreEqual(c1fViewModel, adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual(c1gViewModel, adapter.Value);

            // Value Set
            propertyChange.Events.Clear();

            adapter.Value = c1fViewModel;

            Assert.AreEqual(1, propertyChange.Events.Count);
            Assert.AreEqual(c1fViewModel, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1f;

            Assert.IsEmpty(propertyChange.Events);
            Assert.AreEqual(c1fViewModel, adapter.Value);
            Assert.IsEmpty(propertyChange.Events);
        }
    }
}
