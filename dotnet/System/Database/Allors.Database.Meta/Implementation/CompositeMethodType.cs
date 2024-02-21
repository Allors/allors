﻿namespace Allors.Database.Meta;

public sealed class CompositeMethodType : ICompositeMethodType
{
    public CompositeMethodType(IComposite composite, MethodType methodType)
    {
        this.Attributes = new MetaExtension();
        this.Composite = composite;
        this.MethodType = methodType;
    }

    public dynamic Attributes { get; }

    public IComposite Composite { get; }

    public IMethodType MethodType { get; }
}
