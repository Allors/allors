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

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1AllorsString.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1b.C1AllorsString.Value = "Hello";
            
            var propertyChange = new PropertyChange();
            var weakReference = new WeakReference<IPropertyChange>(propertyChange);

            Expression<Func<IMetaC1, IRelationEndType>> expression = v => v.C1C1One2One.ObjectType.C1AllorsString;
            var path = expression.Node(this.M);
            var pathAdapters = new PathAdapter<string>(c1a, path, weakReference, "String");

            var result = pathAdapters.Value;

            Assert.Equal("Hello", result);

            //var c1bPropertyChanges = new List<string>();

            //c1a.C1C1One2One.PropertyChanged += (sender, args) =>
            //{
            //    c1bPropertyChanges.Add(args.PropertyName);
            //};

            //c1b.C1C1One2One.Value = c1c;

            //Assert.Empty(c1bPropertyChanges);


            //c1b.C1C1One2One.Value = c1c;

            //Assert.Empty(c1bPropertyChanges);

            //c1b.C1C1One2One.Value = c1b;

            //Assert.Equal(2, c1bPropertyChanges.Count);
            //Assert.Contains("Value", c1bPropertyChanges);
            //Assert.Contains("Exist", c1bPropertyChanges);
            //Assert.Single(c1bPropertyChanges);
            //Assert.Contains("Value", c1bPropertyChanges);
        }

        private class PropertyChange : IPropertyChange
        {
            public PropertyChange()
            {
                this.Events = new List<PropertyChangedEventArgs>();
            }

            public IList<PropertyChangedEventArgs> Events { get; }

            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                this.Events.Add(e);
            }
        }
    }
}
