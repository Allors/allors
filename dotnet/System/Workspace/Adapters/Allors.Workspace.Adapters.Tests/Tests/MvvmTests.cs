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
    using Meta;
    using Meta.Static;
    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
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
        public async void PathTest()
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
            var weakReference = new WeakReference<IPropertyChange>(propertyChange);

            // c1a.C1C1One2One.C1C1One2One.C1AllorsString
            Expression<Func<IMetaC1, IRelationEndType>> expression = v => v.C1C1One2One.ObjectType.C1C1One2One.ObjectType.C1AllorsString;
            var path = expression.Node(this.M);
            var adapter = new PathAdapter<string>(c1a.Strategy, path, weakReference, "String");

            Assert.Equal("Hello", adapter.Value);
            Assert.Empty(propertyChange.Events);

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.Single(propertyChange.Events);
            Assert.Equal("Hello Again", adapter.Value);

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.Single(propertyChange.Events);
            Assert.Equal("Hello Again", adapter.Value);

            c1b.C1C1One2One.Value = c1d;

            Assert.Equal(2, propertyChange.Events.Count);
            Assert.Equal("Another Hello", adapter.Value);
        }

        [Fact]
        public async void PathWithMissingStepsTest()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite || !c1c.C1AllorsString.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy }, new Pull { Object = c1c.Strategy });
            }

            var propertyChange = new PropertyChange();
            var weakReference = new WeakReference<IPropertyChange>(propertyChange);

            // c1a.C1C1One2One.C1C1One2One.C1AllorsString
            Expression<Func<IMetaC1, IRelationEndType>> expression = v => v.C1C1One2One.ObjectType.C1C1One2One.ObjectType.C1AllorsString;
            var path = expression.Node(this.M);
            var adapter = new PathAdapter<string>(c1a.Strategy, path, weakReference, "String");

            Assert.Null(adapter.Value);
            Assert.Empty(propertyChange.Events);

            c1a.C1C1One2One.Value = c1b;

            Assert.Null(adapter.Value);
            Assert.Empty(propertyChange.Events);

            c1b.C1C1One2One.Value = c1c;

            Assert.Null(adapter.Value);
            //Assert.Empty(propertyChange.Events);

            c1c.C1AllorsString.Value = "Hello";

            Assert.Equal("Hello", adapter.Value);
            //Assert.Single(propertyChange.Events);
        }

        [Fact]
        public async void ReactiveExpressionTest()
        {
            var workspace = this.Profile.Workspace;
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

            Expression<Func<C1, string>> expression = v => v.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString.Value;
            //Expression<Func<C1, string>> expression = v => v.C1C1One2One.Value.C1AllorsString.Value;

            var reactiveExpression = reactiveExpressionBuilder.Build(expression, c1a);

            Assert.Equal("Hello", reactiveExpression.Value);
            //Assert.Empty(propertyChange.Events);

            //c1c.C1AllorsString.Value = "Hello Again";

            //Assert.Single(propertyChange.Events);
            //Assert.Equal("Hello Again", reactiveExpression.Value);

            //c1d.C1AllorsString.Value = "Another Hello";

            //Assert.Single(propertyChange.Events);
            //Assert.Equal("Hello Again", reactiveExpression.Value);

            //c1b.C1C1One2One.Value = c1d;

            //Assert.Equal(2, propertyChange.Events.Count);
            //Assert.Equal("Another Hello", reactiveExpression.Value);
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
