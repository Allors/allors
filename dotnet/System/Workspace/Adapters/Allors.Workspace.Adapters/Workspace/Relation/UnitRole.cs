// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Adapters;
    using Meta;

    public class UnitRole<T> : IUnitRole<T>
    {
        public UnitRole(Strategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }

        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType { get; }

        object IRelationEnd.Value => this.Value;

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (T)value;
        }

        public T Value
        {
            get => (T)this.Object.GetUnitRole(this.RoleType);
            set => this.Object.SetUnitRole(this.RoleType, value);
        }

        public bool CanRead => this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.Object.CanWrite(this.RoleType);

        public bool Exist => this.Object.ExistRole(this.RoleType);

        public bool IsModified => this.Object.IsModified(this.RoleType);


        public void Restore()
        {
            this.Object.RestoreRole(this.RoleType);
        }

        public IDisposable Subscribe(IObserver<IOperand> observer)
        {
            return this.Object.Workspace.Subscribe(this, observer);
        }

        public IDisposable Subscribe(IObserver<IUnitRole> observer)
        {
            return this.Object.Workspace.Subscribe(this, (IObserver<IOperand>)observer);
        }

        public IDisposable Subscribe(IObserver<IRelationEnd<T>> observer)
        {
            return this.Object.Workspace.Subscribe(this, (IObserver<IOperand>)observer);
        }

        public IDisposable Subscribe(IObserver<IRole<T>> observer)
        {
            return this.Object.Workspace.Subscribe(this, (IObserver<IOperand>)observer);
        }

        public IDisposable Subscribe(IObserver<IUnitRole<T>> observer)
        {
            return this.Object.Workspace.Subscribe(this, (IObserver<IOperand>)observer);
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
