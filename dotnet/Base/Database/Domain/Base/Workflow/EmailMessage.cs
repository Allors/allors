﻿// <copyright file="EmailMessage.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class EmailMessage
    {
        public static void Send(ITransaction transaction, string defaultSender)
        {
            var m = transaction.Database.Services.Get<Meta.M>();

            var mailer = transaction.Database.Services.Get<IMailer>();
            var emailMessages = transaction.Extent<EmailMessage>();
            emailMessages.Filter.AddNot().AddExists(m.EmailMessage.DateSending);
            emailMessages.Filter.AddNot().AddExists(m.EmailMessage.DateSent);

            foreach (EmailMessage emailMessage in emailMessages)
            {
                try
                {
                    emailMessage.DateSending = transaction.Now();

                    transaction.Derive();
                    transaction.Commit();

                    mailer.Send(emailMessage, defaultSender);
                    emailMessage.DateSent = transaction.Now();

                    transaction.Derive();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    transaction.Rollback();
                    break;
                }
            }
        }

        public void CoreOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistDateCreated)
            {
                this.DateCreated = this.Strategy.Transaction.Now();
            }
        }
    }
}
