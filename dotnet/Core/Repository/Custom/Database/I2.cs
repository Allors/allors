// <copyright file="I2.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Attributes;


#region Allors
[Id("19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0")]
#endregion
public interface I2 : Object, I12
{
    #region Allors
    [Id("01d9ff41-d503-421e-93a6-5563e1787543")]
    [Indexed]
    
    #endregion

    I2 I2I2Many2One { get; set; }

    #region Allors
    [Id("1f763206-c575-4e34-9e6b-997d434d3f42")]
    [Indexed]
    
    #endregion

    C1 I2C1Many2One { get; set; }

    #region Allors
    [Id("23e9c15f-097f-4452-9bac-d7cf2a65134a")]
    [Indexed]
    
    #endregion

    I12 I2I12Many2One { get; set; }

    #region Allors
    [Id("35040d7c-ab7f-4a99-9d09-e01e24ca3cb9")]
    
    #endregion

    bool I2AllorsBoolean { get; set; }

    #region Allors
    [Id("40b8edb3-e8c4-46c0-855b-4b18e0e8d7f3")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    C1[] I2C1One2Manies { get; set; }

    #region Allors
    [Id("49736daf-d0bd-4216-97fa-958cfa21a4f0")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    C1 I2C1One2One { get; set; }

    #region Allors
    [Id("4f095abd-8803-4610-87f0-2847ddd5e9f4")]
    [Precision(19)]
    [Scale(2)]
    
    #endregion

    decimal I2AllorsDecimal { get; set; }

    #region Allors
    [Id("5ebbc734-23dd-494f-af2d-8e75caaa3e26")]
    [Indexed]
    
    #endregion

    I2[] I2I2Many2Manies { get; set; }

    #region Allors
    [Id("62a8a93d-3744-49de-9f9a-9997b6ef4da6")]
    [Size(-1)]
    
    #endregion

    byte[] I2AllorsBinary { get; set; }

    #region Allors
    [Id("663559c4-ef64-4e78-89b4-bfa00691c627")]
    
    #endregion

    Guid I2AllorsUnique { get; set; }

    #region Allors
    [Id("6bb406bc-627b-444c-9c16-df9878e05e9c")]
    [Indexed]
    
    #endregion

    I1 I2I1Many2One { get; set; }

    #region Allors
    [Id("81d9eb2f-55a7-4d1c-853d-4369eb691ba5")]
    
    #endregion

    DateTime I2AllorsDateTime { get; set; }

    #region Allors
    [Id("83dc0581-e04a-4f51-a44e-4fef63d44356")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    I12[] I2I12One2Manies { get; set; }

    #region Allors
    [Id("87499e99-ed77-44c1-89d6-b4f570b6f217")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    I12 I2I12One2One { get; set; }

    #region Allors
    [Id("92fdb313-0b90-48f6-b054-a4ab38f880ba")]
    [Indexed]
    
    #endregion

    C2[] I2C2Many2Manies { get; set; }

    #region Allors
    [Id("9bed0518-1946-4e23-9d4b-e4cda439984c")]
    [Indexed]
    
    #endregion

    I1[] I2I1Many2Manies { get; set; }

    #region Allors
    [Id("9f361b97-0b04-496d-ac60-718760c2a4e2")]
    [Indexed]
    
    #endregion

    C2 I2C2Many2One { get; set; }

    #region Allors
    [Id("9f91841c-f63f-4ffa-bee6-62e100f3cd15")]
    [Size(256)]
    
    #endregion

    string I2AllorsString { get; set; }

    #region Allors
    [Id("b39fdd23-d7dd-473f-9705-df2f29be5ffe")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    C2[] I2C2One2Manies { get; set; }

    #region Allors
    [Id("b640bf16-0dc0-4203-aa76-f456371239ae")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    I1 I2I1One2One { get; set; }

    #region Allors
    [Id("bbb01166-2671-4ca1-8b1e-12e6ae8aeb03")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    I1[] I2I1One2Manies { get; set; }

    #region Allors
    [Id("cb9f21e0-a841-45de-8ba4-991b4ceca616")]
    [Indexed]
    
    #endregion

    I12[] I2I12Many2Manies { get; set; }

    #region Allors
    [Id("cc4c704c-ab7e-45d4-baa9-b67cfff9448e")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    I2 I2I2One2One { get; set; }

    #region Allors
    [Id("d30dd036-6d28-48df-873b-3a76da8c029e")]
    
    #endregion

    int I2AllorsInteger { get; set; }

    #region Allors
    [Id("deb9cbd3-386f-4599-802c-be50945b9f1d")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    I2[] I2I2One2Manies { get; set; }

    #region Allors
    [Id("f364c9fe-ad36-4305-80fd-4921451c70a5")]
    [Indexed]
    
    #endregion

    C1[] I2C1Many2Manies { get; set; }

    #region Allors
    [Id("f85c2d97-10b9-478d-9b82-2700d95d5cb1")]
    [SingleAssociation]
    [Indexed]
    
    #endregion

    C2 I2C2One2One { get; set; }

    #region Allors
    [Id("fbad33e7-ede1-41fc-97e9-ddf33a0f6459")]
    
    #endregion

    double I2AllorsDouble { get; set; }
}
