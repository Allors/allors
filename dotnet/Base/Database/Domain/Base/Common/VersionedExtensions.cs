// <copyright file="VersionedExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Meta;

    public static partial class VersionedExtensions
    {
        public static void BaseOnPostDerive(this Versioned @this, ObjectOnPostDerive method)
        {
            // TODO: move parts to Meta
            var derivation = method.Derivation;
            var versionedClass = @this.Strategy.Class;
            var metaPopulation = versionedClass.MetaPopulation;
            var versionClass = (IClass)metaPopulation.FindCompositeByName(versionedClass.SingularName + "Version");
            var versionTypeRegex = new Regex(".+Version");

            var currentVersionRole = versionedClass.RoleTypes.First(v => v.Name.Equals("CurrentVersion"));
            var currentVersion = @this.Strategy.GetCompositeRole(currentVersionRole);

            var isNewVersion = currentVersion == null;
            if (!isNewVersion)
            {
                foreach (var versionRoleType in versionClass.RoleTypes.Where(v => versionTypeRegex.IsMatch(v.AssociationType.ObjectType.SingularName)))
                {
                    var versionedRoleType = versionedClass.RoleTypes.FirstOrDefault(v => v.Name.Equals(versionRoleType.Name));
                    if (versionedRoleType == null)
                    {
                        throw new Exception("Could not find versioned role " + versionRoleType.Name);
                    }

                    if (versionRoleType.IsMany)
                    {
                        var versionedRole = @this.Strategy.GetCompositesRole<IObject>(versionedRoleType);
                        var versionRole = currentVersion.Strategy.GetCompositesRole<IObject>(versionRoleType);
                        // TODO: use Numbers
                        if (!(!versionedRole.Any() && !versionRole.Any()) ||
                            versionedRole.Count() != versionRole.Count() ||
                            !versionedRole.ToArray().OrderBy(s => s)
                                .SequenceEqual(versionRole.ToArray().OrderBy(t => t)))
                        {
                            isNewVersion = true;
                            break;
                        }
                    }
                    else
                    {
                        var versionedRole = @this.Strategy.GetRole(versionedRoleType);
                        var versionRole = currentVersion.Strategy.GetRole(versionRoleType);
                        if (!Equals(versionedRole, versionRole))
                        {
                            isNewVersion = true;
                            break;
                        }
                    }
                }
            }

            if (isNewVersion)
            {
                var newVersion = (Version)@this.Transaction().Build(versionClass);
                newVersion.DerivationId = derivation.Id;
                newVersion.DerivationTimeStamp = derivation.TimeStamp;

                foreach (var versionRoleType in versionClass.RoleTypes.Where(v => versionTypeRegex.IsMatch(v.AssociationType.ObjectType.SingularName)))
                {
                    var versionedRoleType = versionedClass.RoleTypes.FirstOrDefault(v => v.Name.Equals(versionRoleType.Name));
                    if (versionedRoleType == null)
                    {
                        throw new Exception("Could not find versioned role " + versionRoleType.Name + " on type " + versionedClass.SingularName);
                    }

                    var versionedRole = @this.Strategy.GetRole(versionedRoleType);
                    newVersion.Strategy.SetRole(versionRoleType, versionedRole);
                }

                var allVersionsRole = versionedClass.RoleTypes.First(v => v.Name.Equals("AllVersions"));

                @this.Strategy.SetRole(currentVersionRole, newVersion);
                @this.Strategy.AddCompositesRole(allVersionsRole, newVersion);
            }
        }
    }
}
