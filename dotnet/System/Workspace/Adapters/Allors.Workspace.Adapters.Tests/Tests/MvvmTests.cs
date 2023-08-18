// <copyright file="ServicesTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Mvvm;
    using Mvvm.Adapters;
    using Org.BouncyCastle.Asn1.X509.Qualified;
    using Xunit;

    public abstract class MvvmTests : Test
    {
        protected MvvmTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void ExpressionAdapterTest()
        {
            var workspace = this.Profile.Workspace;
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

            Expression<Func<C1, string>> expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString.Value;

            var reactiveFunc = reactiveFuncBuilder.Build(expression);

            var reactiveExpression = reactiveExpressionBuilder.Build(c1a, reactiveFunc);

            var adapter = new ExpressionAdapter<C1, String>(propertyChange, reactiveExpression, "String");

            Assert.Equal("Hello", adapter.Value);
            Assert.Empty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.Single(propertyChange.Events);
            Assert.Equal("Hello Again", adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.Empty(propertyChange.Events);
            Assert.Equal("Hello Again", adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.Single(propertyChange.Events);
            Assert.Equal("Another Hello", adapter.Value);
        }

        [Fact]
        public async void UnitRoleExpressionAdapterTest()
        {
            var workspace = this.Profile.Workspace;
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
            Assert.Equal("Hello", adapter.Value);
            Assert.Empty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.Single(propertyChange.Events);
            Assert.Equal("Hello Again", adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.Empty(propertyChange.Events);
            Assert.Equal("Hello Again", adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.Single(propertyChange.Events);
            Assert.Equal("Another Hello", adapter.Value);

            // Value Set
            propertyChange.Events.Clear();

            adapter.Value = "Hello from Value Set";

            Assert.Single(propertyChange.Events);
            Assert.Equal("Hello from Value Set", adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1AllorsString.Value = "Hello from Value Set";

            Assert.Empty(propertyChange.Events);
            Assert.Equal("Hello from Value Set", adapter.Value);
            Assert.Empty(propertyChange.Events);
        }

        [Fact]
        public async void CompositeRoleExpressionAdapterTest()
        {
            var workspace = this.Profile.Workspace;
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
            Assert.Equal(c1e, adapter.Value);
            Assert.Empty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1C1One2One.Value = c1f;

            Assert.Single(propertyChange.Events);
            Assert.Equal(c1f, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1g;

            Assert.Empty(propertyChange.Events);
            Assert.Equal(c1f, adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.Single(propertyChange.Events);
            Assert.Equal(c1g, adapter.Value);

            // Value Set
            propertyChange.Events.Clear();

            adapter.Value = c1f;

            Assert.Single(propertyChange.Events);
            Assert.Equal(c1f, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1f;

            Assert.Empty(propertyChange.Events);
            Assert.Equal(c1f, adapter.Value);
            Assert.Empty(propertyChange.Events);
        }

        [Fact]
        public async void ViewModelCompositeRoleExpressionAdapterTest()
        {
            var workspace = this.Profile.Workspace;
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
            Assert.Equal(c1eViewModel, adapter.Value);
            Assert.Empty(propertyChange.Events);

            propertyChange.Events.Clear();

            c1c.C1C1One2One.Value = c1f;

            Assert.Single(propertyChange.Events);
            Assert.Equal(c1fViewModel, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1g;

            Assert.Empty(propertyChange.Events);
            Assert.Equal(c1fViewModel, adapter.Value);

            propertyChange.Events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.Single(propertyChange.Events);
            Assert.Equal(c1gViewModel, adapter.Value);

            // Value Set
            propertyChange.Events.Clear();

            adapter.Value = c1fViewModel;

            Assert.Single(propertyChange.Events);
            Assert.Equal(c1fViewModel, adapter.Value);

            propertyChange.Events.Clear();

            c1d.C1C1One2One.Value = c1f;

            Assert.Empty(propertyChange.Events);
            Assert.Equal(c1fViewModel, adapter.Value);
            Assert.Empty(propertyChange.Events);
        }


        private class ViewModel : IViewModel
        {
            public IList<PropertyChangedEventArgs> Events { get; } = new List<PropertyChangedEventArgs>();

            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                this.Events.Add(e);
            }
        }

        private class C1ViewModel : IObjectViewModel<C1>
        {
            public C1ViewModel(C1 model)
            {
                this.Model = model;
            }

            public C1 Model { get; }

            public IList<PropertyChangedEventArgs> Events { get; } = new List<PropertyChangedEventArgs>();

            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                this.Events.Add(e);
            }
        }

        private class Resolver : IViewModelResolver
        {
            private readonly ViewModelFactory factory;
            private readonly IDictionary<IObject, IViewModel> viewModelByObject;

            public Resolver(ViewModelFactory factory)
            {
                this.factory = factory;
                this.viewModelByObject = new ConcurrentDictionary<IObject, IViewModel>();
            }

            public IViewModel Resolve(IObject @object)
            {
                if (!viewModelByObject.TryGetValue(@object, out var viewModel))
                {
                    viewModel = this.factory(@object);
                    viewModelByObject[@object] = viewModel;
                }

                return viewModel;
            }
        }
    }
}
