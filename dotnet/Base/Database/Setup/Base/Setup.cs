// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Meta;

    public partial class Setup
    {
        private void BaseOnPrePrepare()
        {
        }

        private void BaseOnPostPrepare()
        {
        }

        private void BaseOnPreSetup()
        {
        }

        private void BaseOnPostSetup()
        {
        }

        private void BaseOnCreated(IObject @object)
        {
            IClass @class = @object.Strategy.Class;
            if (@class.KeyRoleType != null)
            {
                if (this.Config.ResourceSetByCultureInfoByClass.TryGetValue(@class, out var resourceSetByCultureInfo))
                {
                    var key = (string)@object.Strategy.GetUnitRole(@class.KeyRoleType);
                    foreach (var (cultureInfo, resourceSet) in resourceSetByCultureInfo)
                    {
                        var value = resourceSet.GetString(key);
                        if (value != null)
                        {
                            var locales = new LocaleByName(this.transaction);
                            var locale = locales[cultureInfo.TwoLetterISOLanguageName];

                            // TODO:
                        }
                    }
                }
            }
        }
    }
}
