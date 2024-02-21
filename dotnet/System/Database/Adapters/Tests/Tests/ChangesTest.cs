// <copyright file="ChangesTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ExtentTest type.</summary>

namespace Allors.Database.Adapters;

using System;
using System.Linq;
using Allors.Database.Domain;
using Meta;
using Xunit;

public abstract class ChangesTest : IDisposable
{
    protected abstract IProfile Profile { get; }

    protected ITransaction Transaction => this.Profile.Transaction;

    protected Action[] Markers => this.Profile.Markers;

    protected Action[] Inits => this.Profile.Inits;

    public abstract void Dispose();

    [Fact]
    public void CreateLifecycle()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var a = (C1)this.Transaction.Build(m.C1);
            var c = this.Transaction.Build(m.C3);
            this.Transaction.Commit();

            a = (C1)this.Transaction.Instantiate(a);
            var b = C2.Create(this.Transaction);
            this.Transaction.Instantiate(c);

            var changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Created);
            Assert.Contains(b, changes.Created.ToArray());

            this.Transaction.Rollback();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Created);

            b = C2.Create(this.Transaction);

            this.Transaction.Commit();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Created);
        }
    }

    [Fact]
    public void OnPostBuildCreateWithObjectType()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            foreach (var @class in ((MetaPopulation)m).Classes)
            {
                dynamic newObject = this.Transaction.Build(@class);
                Assert.True(newObject.onPostBuild);
            }
        }
    }

    [Fact]
    public void OnPostBuildCreateWithObjectTypeAndCount()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            foreach (var @class in ((MetaPopulation)m).Classes)
            {
                {
                    var newObjects = this.Transaction.Build(@class, 2);
                    foreach (dynamic newObject in newObjects)
                    {
                        Assert.True(newObject.onPostBuild);
                    }
                }
            }
        }
    }

    [Fact]
    public void OnPostBuildCreateWithGeneric()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var newObject = this.Transaction.Build<C1>();
            Assert.True(newObject.onPostBuild);
            Assert.Null(newObject.Name);
        }
    }

    [Fact]
    public void OnPostBuildCreateWithAction()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            Action<C1> nullBuilder = null;
            Action<C1> builderA = v => v.Name += "A";

            {
                var newObject = this.Transaction.Build(builderA);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("A", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Null(newObject.Name);
            }
        }
    }

    [Fact]
    public void OnPostBuildCreateWithActionParams()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            Action<C1> nullBuilder = null;
            Action<C1> builderA = v => v.Name += "A";
            Action<C1> builderB = v => v.Name += "B";

            {
                var newObject = this.Transaction.Build(builderA, builderB);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("AB", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(builderA, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("A", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilder, builderB);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("B", newObject.Name);
            }
        }
    }

    [Fact]
    public void OnPostBuildCreateWithEnumAndAction()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            Action<C1> nullBuilder = null;
            Action<C1> builderA = v => v.Name += "A";
            Action<C1> builderB = v => v.Name += "B";
            Action<C1> builderC = v => v.Name += "C";
            Action<C1> builderD = v => v.Name += "D";

            {
                var builderEnumeration = new[] { builderA, builderB };
                var newObject = this.Transaction.Build(builderEnumeration);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("AB", newObject.Name);
            }

            {
                var builderEnumeration = new[] { builderA, builderB };
                var newObject = this.Transaction.Build(builderEnumeration, builderC);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("ABC", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { nullBuilder, builderB };
                var newObject = this.Transaction.Build(builderWithNullEnumeration);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("B", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { builderA, nullBuilder };
                var newObject = this.Transaction.Build(builderWithNullEnumeration);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("A", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { nullBuilder, builderB };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, builderC);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("BC", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { builderA, nullBuilder };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, builderC);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("AC", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { nullBuilder, builderB };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("B", newObject.Name);
            }

            {
                var builderEnumeration = new[] { builderA, builderB };
                var newObject = this.Transaction.Build(builderEnumeration, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("AB", newObject.Name);
            }

            Action<C1>[] nullBuilderEnumeration = null;

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration);
                Assert.True(newObject.onPostBuild);
                Assert.Null(newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration, builderC);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("C", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Null(newObject.Name);
            }
        }
    }

    [Fact]
    public void OnPostBuildCreateWithEnumAndActionParams()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            Action<C1> nullBuilder = null;

            Action<C1> builderA = v => v.Name += "A";
            Action<C1> builderB = v => v.Name += "B";
            Action<C1> builderC = v => v.Name += "C";
            Action<C1> builderD = v => v.Name += "D";

            {
                var builderEnumeration = new[] { builderA, builderB };
                var newObject = this.Transaction.Build(builderEnumeration, builderC, builderD);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("ABCD", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { nullBuilder, builderB };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, builderC, builderD);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("BCD", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { builderA, nullBuilder };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, builderC, builderD);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("ACD", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { nullBuilder, builderB };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, builderC, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("BC", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { builderA, nullBuilder };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, nullBuilder, builderD);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("AD", newObject.Name);
            }

            {
                var builderWithNullEnumeration = new[] { builderA, nullBuilder };
                var newObject = this.Transaction.Build(builderWithNullEnumeration, nullBuilder, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("A", newObject.Name);
            }

            Action<C1>[] nullBuilderEnumeration = null;

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration, builderC, builderD);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("CD", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration, builderC, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("C", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration, nullBuilder, builderD);
                Assert.True(newObject.onPostBuild);
                Assert.Equal("D", newObject.Name);
            }

            {
                var newObject = this.Transaction.Build(nullBuilderEnumeration, nullBuilder, nullBuilder);
                Assert.True(newObject.onPostBuild);
                Assert.Null(newObject.Name);
            }
        }
    }


    //[Fact]
    //public void CreateWithBuilder()
    //{
    //    foreach (var init in this.Inits)
    //    {
    //        init();
    //        var m = this.Transaction.Database.Context().M;

    //        Action<C1> nullBuilder = null;
    //        Action<C1> builderA = v => v.Name = "A";
    //        Action<C1> builderB = v => v.Name += "B";


    //        {
    //            var newObject = this.Transaction.Create<C1>(v => v.Name = "A");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("A", newObject.Name);
    //        }

    //        {
    //            var newObject = this.Transaction.Create(nullBuilder);
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Null(newObject.Name);
    //        }

    //        {
    //            var newObject = this.Transaction.Create<C1>(builderA, builderB);
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("AB", newObject.Name);
    //        }

    //        {
    //            var newObject = this.Transaction.Create<C1>(builderA, nullBuilder);
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("A", newObject.Name);
    //        }

    //        {
    //            var newObject = this.Transaction.Create<C1>(nullBuilder, builderB);
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("B", newObject.Name);
    //        }

    //        {
    //            var builderEnumeration = new[] { builderA, builderB };
    //            var newObject = this.Transaction.Create<C1>(builderEnumeration, v => v.Name += "C");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("ABC", newObject.Name);
    //        }

    //        {
    //            Action<C1> builderA = v => v.Name = "A";
    //            Action<C1> builderB = v => v.Name += "B";
    //            var builderEnumeration = new[] { builderA, builderB };
    //            var newObject = this.Transaction.Create<C1>(builderEnumeration, v => v.Name += "C", v => v.Name += "D");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("ABCD", newObject.Name);
    //        }

    //        {
    //            Action<C1>[] nullBuilderEnumeration = null;
    //            var newObject = this.Transaction.Create<C1>(nullBuilderEnumeration, v => v.Name += "C");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("C", newObject.Name);
    //        }

    //        {
    //            Action<C1>[] nullBuilderEnumeration = null;
    //            var newObject = this.Transaction.Create<C1>(nullBuilderEnumeration, v => v.Name += "C", nullBuilder);
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("C", newObject.Name);
    //        }

    //        {
    //            Action<C1>[] nullBuilderEnumeration = null;
    //            var newObject = this.Transaction.Create<C1>(nullBuilderEnumeration, nullBuilder, v => v.Name += "C");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("C", newObject.Name);
    //        }

    //        {
    //            Action<C1> builderB = v => v.Name += "B";
    //            var builderWithNullEnumeration = new[] { nullBuilder, builderB };
    //            var newObject = this.Transaction.Create<C1>(builderWithNullEnumeration, v => v.Name += "C", v => v.Name += "D");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("BCD", newObject.Name);
    //        }

    //        {
    //            Action<C1> builderB = v => v.Name += "B";
    //            var builderWithNullEnumeration = new[] { nullBuilder, builderB };
    //            var newObject = this.Transaction.Create<C1>(builderWithNullEnumeration, v => v.Name += "C", v => v.Name += "D");
    //            Assert.True(newObject.onPostBuild);
    //            Assert.Equal("BCD", newObject.Name);
    //        }

    //        foreach (var @class in m.Classes)
    //        {
    //            {
    //                dynamic newObject = this.Transaction.Create(@class);
    //                Assert.True(newObject.onPostBuild);
    //            }

    //            {
    //                var newObjects = this.Transaction.Create(@class, 2);
    //                foreach (dynamic newObject in newObjects)
    //                {
    //                    Assert.True(newObject.onPostBuild);
    //                }
    //            }
    //        }
    //    }
    //}

    [Fact]
    public void StringRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var a = (C1)this.Transaction.Build(m.C1);
            var c = this.Transaction.Build(m.C3);
            this.Transaction.Commit();

            a = (C1)this.Transaction.Instantiate(a);
            var b = C2.Create(this.Transaction);
            this.Transaction.Instantiate(c);

            a.RemoveC1AllorsString();
            b.RemoveC2AllorsString();

            var changeSet = this.Transaction.Checkpoint();

            var associations = changeSet.Associations;
            var roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(roles);

            a.C1AllorsString = "a changed";
            b.C2AllorsString = "b changed";

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Equal(2, associations.Count());
            Assert.Contains(a, associations.ToArray());
            Assert.Contains(b, associations.ToArray());

            Assert.Equal("a changed", a.C1AllorsString);
            Assert.Equal("b changed", b.C2AllorsString);

            Assert.Single(changeSet.GetRoleTypes(a));
            Assert.Equal(m.C1.C1AllorsString, changeSet.GetRoleTypes(a).First());

            Assert.Single(changeSet.GetRoleTypes(b));
            Assert.Equal(m.C2.C2AllorsString, changeSet.GetRoleTypes(b).First());

            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            a.C1AllorsString = "a changed";
            b.C2AllorsString = "b changed";

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(roles);

            a.C1AllorsString = "a changed again";
            b.C2AllorsString = "b changed again";

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Equal(2, associations.Count());
            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(a));

            Assert.Single(changeSet.GetRoleTypes(a));
            Assert.Equal(m.C1.C1AllorsString, changeSet.GetRoleTypes(a).First());

            Assert.Single(changeSet.GetRoleTypes(b));
            Assert.Equal(m.C2.C2AllorsString, changeSet.GetRoleTypes(b).First());

            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            a.RemoveC1AllorsString();
            b.RemoveC2AllorsString();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Equal(2, associations.Count());
            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(a));

            Assert.Single(changeSet.GetRoleTypes(a));
            Assert.Equal(m.C1.C1AllorsString, changeSet.GetRoleTypes(a).First());

            Assert.Single(changeSet.GetRoleTypes(b));
            Assert.Equal(m.C2.C2AllorsString, changeSet.GetRoleTypes(b).First());

            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));

            this.Transaction.Rollback();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));

            a.C1AllorsString = "a changed";

            this.Transaction.Commit();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));

            a.RemoveC1AllorsString();
            a.C1AllorsString = "a changed";

            this.Transaction.Commit();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
        }
    }

    [Fact]
    public void BooleanRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var a = (C1)this.Transaction.Build(m.C1);
            var c = this.Transaction.Build(m.C3);
            this.Transaction.Commit();

            a = (C1)this.Transaction.Instantiate(a);
            var b = C2.Create(this.Transaction);
            this.Transaction.Instantiate(c);

            a.RemoveC1AllorsBoolean();
            b.RemoveC2AllorsBoolean();

            var changeSet = this.Transaction.Checkpoint();

            var associations = changeSet.Associations;
            var roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(roles);

            a.C1AllorsBoolean = true;
            b.C2AllorsBoolean = false;

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Equal(2, associations.Count());
            Assert.Contains(a, associations.ToArray());
            Assert.Contains(b, associations.ToArray());

            Assert.Equal(true, a.C1AllorsBoolean);
            Assert.Equal(false, b.C2AllorsBoolean);

            Assert.Single(changeSet.GetRoleTypes(a));
            Assert.Equal(m.C1.C1AllorsBoolean, changeSet.GetRoleTypes(a).First());

            Assert.Single(changeSet.GetRoleTypes(b));
            Assert.Equal(m.C2.C2AllorsBoolean, changeSet.GetRoleTypes(b).First());

            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            a.C1AllorsBoolean = true;
            b.C2AllorsBoolean = false;

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(roles);

            a.C1AllorsBoolean = false;
            b.C2AllorsBoolean = true;

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Equal(2, associations.Count());
            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(a));

            Assert.Single(changeSet.GetRoleTypes(a));
            Assert.Equal(m.C1.C1AllorsBoolean, changeSet.GetRoleTypes(a).First());

            Assert.Single(changeSet.GetRoleTypes(b));
            Assert.Equal(m.C2.C2AllorsBoolean, changeSet.GetRoleTypes(b).First());

            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            a.RemoveC1AllorsBoolean();
            b.RemoveC2AllorsBoolean();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Equal(2, associations.Count());
            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(a));

            Assert.Single(changeSet.GetRoleTypes(a));
            Assert.Equal(m.C1.C1AllorsBoolean, changeSet.GetRoleTypes(a).First());

            Assert.Single(changeSet.GetRoleTypes(b));
            Assert.Equal(m.C2.C2AllorsBoolean, changeSet.GetRoleTypes(b).First());

            Assert.True(associations.Contains(a));
            Assert.True(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
            Assert.False(roles.Contains(c));

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));

            this.Transaction.Rollback();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));

            a.C1AllorsBoolean = true;

            this.Transaction.Commit();

            changeSet = this.Transaction.Checkpoint();

            associations = changeSet.Associations;
            roles = changeSet.Roles;

            Assert.Empty(associations);
            Assert.Empty(changeSet.GetRoleTypes(a));
            Assert.Empty(changeSet.GetRoleTypes(b));

            Assert.False(associations.Contains(a));
            Assert.False(associations.Contains(b));
            Assert.False(associations.Contains(c));

            Assert.False(roles.Contains(a));
            Assert.False(roles.Contains(b));
        }
    }

    [Fact]
    public void One2OneRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var c1a = (C1)this.Transaction.Build(m.C1);
            var c1b = (C1)this.Transaction.Build(m.C1);
            var c2a = (C2)this.Transaction.Build(m.C2);
            var c2b = (C2)this.Transaction.Build(m.C2);

            c1a.Name = "c1a";
            c1b.Name = "c1b";
            c2a.Name = "c2a";
            c2b.Name = "c2b";

            this.Transaction.Commit();

            var changes = this.Transaction.Checkpoint();

            // Reset empty role
            c1a.C1C2one2one = null;

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove empty role
            c1a.RemoveC1C2one2one();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Set role
            c1a.C1C2one2one = c2b;

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2b, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2one, changes.GetRoleTypes(c1a).First());

            // Set same role
            c1a.C1C2one2one = c2b;

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Set different role
            c1a.C1C2one2one = c2a;

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Equal(2, changes.Roles.Count());
            Assert.Contains(c2b, changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2one, changes.GetRoleTypes(c1a).First());

            // Remove role
            c1a.RemoveC1C2one2one();

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2one, changes.GetRoleTypes(c1a).First());

            // Add and Remove
            c1a.C1C2one2one = c2a;
            c1a.RemoveC1C2one2one();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            c1a.C1C2one2one = c2a;
            this.Transaction.Checkpoint();

            // Remove and Add
            c1a.RemoveC1C2one2one();
            c1a.C1C2one2one = c2a;

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);
        }
    }

    [Fact]
    public void Many2OneRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var c1a = (C1)this.Transaction.Build(m.C1);
            var c1b = (C1)this.Transaction.Build(m.C1);
            var c2a = (C2)this.Transaction.Build(m.C2);
            var c2b = C2.Create(this.Transaction);

            c1a.Name = "c1a";
            c1b.Name = "c1b";
            c2a.Name = "c2a";
            c2b.Name = "c2b";

            this.Transaction.Commit();

            // Reset empty role
            c1a.C1C2many2one = null;

            var changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove empty role
            c1a.RemoveC1C2many2one();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Set role
            c1a.C1C2many2one = c2b;

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2b, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2one, changes.GetRoleTypes(c1a).First());

            // Set same role
            c1a.C1C2many2one = c2b;

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Set different role
            c1a.C1C2many2one = c2a;

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Equal(2, changes.Roles.Count());
            Assert.Contains(c2b, changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2one, changes.GetRoleTypes(c1a).First());

            // Remove role
            c1a.RemoveC1C2many2one();

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2one, changes.GetRoleTypes(c1a).First());

            // Add and Remove
            c1b.C1C2many2one = c2a;
            c1b.RemoveC1C2many2one();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            c1b.C1C2many2one = c2a;
            this.Transaction.Checkpoint();

            // Remove and add
            c1b.RemoveC1C2many2one();
            c1b.C1C2many2one = c2a;

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);
        }
    }

    [Fact]
    public void One2ManyRoles()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var c1a = (C1)this.Transaction.Build(m.C1);
            var c1b = (C1)this.Transaction.Build(m.C1);
            var c2a = (C2)this.Transaction.Build(m.C2);
            var c2b = C2.Create(this.Transaction);

            c1a.Name = "c1a";
            c1b.Name = "c1b";
            c2a.Name = "c2a";
            c2b.Name = "c2b";

            this.Transaction.Commit();

            // Reset empty role
            c1a.C1C2one2manies = null;

            var changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove role
            c1a.RemoveC1C2one2manies();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove non existing
            c1a.RemoveC1C2one2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Add element
            c1a.AddC1C2one2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2b, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2manies, changes.GetRoleTypes(c1a).First());

            // Add same element
            c1a.AddC1C2one2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Set same element
            c1a.C1C2one2manies = [c2b];

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Add another element
            c1a.AddC1C2one2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2manies, changes.GetRoleTypes(c1a).First());

            // Remove element
            c1a.RemoveC1C2one2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2manies, changes.GetRoleTypes(c1a).First());

            // Remove other element
            c1a.RemoveC1C2one2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2b, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2one2manies, changes.GetRoleTypes(c1a).First());

            c1a.AddC1C2one2many(c2a);
            this.Transaction.Checkpoint();

            // Add same to other
            c1b.AddC1C2one2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Equal(2, changes.Associations.Count());
            Assert.Contains(c1a, changes.Associations);
            Assert.Contains(c1b, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            // Remove and add same
            c1b.RemoveC1C2one2many(c2a);
            c1b.AddC1C2one2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove all and add same
            c1b.RemoveC1C2one2manies();
            c1b.AddC1C2one2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            c1b.RemoveC1C2one2manies();
            this.Transaction.Checkpoint();

            // Add and remove all
            c1b.AddC1C2one2many(c2a);
            c1b.RemoveC1C2one2manies();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Add and remove
            c1b.AddC1C2one2many(c2a);
            c1b.RemoveC1C2one2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);
        }
    }

    [Fact]
    public void Many2ManyRoles()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var c1a = (C1)this.Transaction.Build(m.C1);
            var c1b = (C1)this.Transaction.Build(m.C1);
            var c2a = (C2)this.Transaction.Build(m.C2);
            var c2b = C2.Create(this.Transaction);

            c1a.Name = "c1a";
            c1b.Name = "c1b";
            c2a.Name = "c2a";
            c2b.Name = "c2b";

            this.Transaction.Commit();

            // Reset empty role
            c1a.C1C2many2manies = null;

            var changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove role
            c1a.RemoveC1C2many2manies();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove non existing
            c1a.RemoveC1C2many2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Add element
            c1a.AddC1C2many2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2b, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2manies, changes.GetRoleTypes(c1a).First());

            // Add same element
            c1a.AddC1C2many2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Set same element
            c1a.C1C2many2manies = [c2b];

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Add another element
            c1a.AddC1C2many2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2manies, changes.GetRoleTypes(c1a).First());

            // Remove element
            c1a.RemoveC1C2many2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2a, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2manies, changes.GetRoleTypes(c1a).First());

            // Remove other element
            c1a.RemoveC1C2many2many(c2b);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Contains(c1a, changes.Associations);

            Assert.Single(changes.Roles);
            Assert.Contains(c2b, changes.Roles);

            Assert.Single(changes.GetRoleTypes(c1a));
            Assert.Equal(m.C1.C1C2many2manies, changes.GetRoleTypes(c1a).First());

            // Add same to other
            c1a.AddC1C2many2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Single(changes.Associations);
            Assert.Single(changes.Roles);

            c1b.AddC1C2many2many(c2a);
            this.Transaction.Checkpoint();

            // Remove and add same
            c1b.RemoveC1C2many2manies();
            c1b.AddC1C2many2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Remove all and add same
            c1b.RemoveC1C2many2manies();
            c1b.AddC1C2many2many(c2a);

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            c1b.RemoveC1C2many2manies();
            this.Transaction.Checkpoint();

            // Add and remove all
            c1b.AddC1C2many2many(c2a);
            c1b.RemoveC1C2many2manies();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);

            // Add and remove
            c1b.AddC1C2many2many(c2a);
            c1b.RemoveC1C2many2manies();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Associations);
            Assert.Empty(changes.Roles);
        }
    }

    [Fact]
    public void Delete()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Context().M;

            var a = (C1)this.Transaction.Build(m.C1);
            var c = this.Transaction.Build(m.C3);
            this.Transaction.Commit();

            a = (C1)this.Transaction.Instantiate(a);
            var b = C2.Create(this.Transaction);
            this.Transaction.Instantiate(c);

            a.Strategy.Delete();
            b.Strategy.Delete();

            var changes = this.Transaction.Checkpoint();

            Assert.Equal(2, changes.Deleted.Count());
            Assert.Contains(a.Strategy, changes.Deleted.ToArray());
            Assert.Contains(b.Strategy, changes.Deleted.ToArray());

            this.Transaction.Rollback();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Deleted);

            a.Strategy.Delete();

            this.Transaction.Commit();

            changes = this.Transaction.Checkpoint();

            Assert.Empty(changes.Deleted);
        }
    }
}
