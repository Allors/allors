// <copyright file="ServicesTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Mvvm.Adapters;
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

            var propertyChange = new PropertyChange();

            static string ReactiveFunc(C1 v, IDependencyTracker tracker) => v.C1C1One2One.Track(tracker).Value.C1C1One2One.Track(tracker).Value.C1AllorsString.Track(tracker).Value;
            var reactiveExpression = new ReactiveExpression<C1, string>(c1a, ReactiveFunc);

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
        public async void ExpressionAdapterWithReactiveFuncBuilderTest()
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

            var events = new List<PropertyChangedEventArgs>();

            Expression<Func<C1, string>> expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString.Value;

            var reactiveFunc = reactiveFuncBuilder.Build(expression);

            var reactiveExpression = reactiveExpressionBuilder.Build(c1a, reactiveFunc);

            reactiveExpression.PropertyChanged += (_, e) => events.Add(e);

            Assert.Equal("Hello", reactiveExpression.Value);
            Assert.Empty(events);

            events.Clear();

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.Single(events);
            Assert.Equal("Hello Again", reactiveExpression.Value);
            Assert.Single(events);

            events.Clear();

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.Empty(events);
            Assert.Equal("Hello Again", reactiveExpression.Value);
            Assert.Empty(events);

            events.Clear();

            c1b.C1C1One2One.Value = c1d;

            Assert.Single(events);
            Assert.Equal("Another Hello", reactiveExpression.Value);
        }

        [Fact]
        public async void RoleExpressionAdapterTest()
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

            var propertyChange = new PropertyChange();

            Expression<Func<C1, IUnitRole<string>>> expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString;
            var reactiveFunc = reactiveFuncBuilder.Build(expression);
            var reactiveExpression = reactiveExpressionBuilder.Build(c1a, reactiveFunc);

            var adapter = new RoleExpressionAdapter<C1, String>(propertyChange, reactiveExpression, "String");

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

        private class PropertyChange : IPropertyChange
        {
            public IList<PropertyChangedEventArgs> Events { get; } = new List<PropertyChangedEventArgs>();

            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                this.Events.Add(e);
            }
        }
    }
}
