// <copyright file="Security.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Meta;

    public partial class Security
    {
        public void GrantOperations(IObjectType objectType, params Operations[] operations) => this.Grant(Role.OperationsId, objectType, operations);

        public void GrantOperations(IObjectType objectType, IOperandType operandType, params Operations[] operations) => this.Grant(Role.OperationsId, objectType, operandType, operations);

        public void GrantProcurement(IObjectType objectType, params Operations[] operations) => this.Grant(Role.ProcurementId, objectType, operations);

        public void GrantProcurement(IObjectType objectType, IOperandType operandType, params Operations[] operations) => this.Grant(Role.ProcurementId, objectType, operandType, operations);

        public void GrantSales(IObjectType objectType, params Operations[] operations) => this.Grant(Role.SalesId, objectType, operations);

        public void GrantSales(IObjectType objectType, IOperandType operandType, params Operations[] operations) => this.Grant(Role.SalesId, objectType, operandType, operations);

        private void CustomOnPostSetup()
        {
            // Default access policy
            var security = new Security(this.transaction);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            foreach (var @class in this.transaction.Database.MetaPopulation.Classes)
            {
                security.GrantAdministrator(@class, full);
                security.GrantCreator(@class, full);
                security.GrantGuest(@class, Operations.Read);
            }
        }

        private void CustomOnPreSetup()
        {
        }
    }
}
