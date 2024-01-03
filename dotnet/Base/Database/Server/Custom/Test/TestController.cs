// <copyright file="TestController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Allors.Services;
    using Domain;
    using Meta;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Population;
    using Population.Resx;
    using Services;

    public class TestController : Controller
    {
        public TestController(IDatabaseService databaseService) => this.DatabaseService = databaseService;

        public IDatabaseService DatabaseService { get; set; }

        private ILogger<TestController> Logger { get; set; }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Ready() => this.Ok();

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Init()
        {
            try
            {
                var database = this.DatabaseService.Database;
                database.Init();

                return this.Ok();
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Setup(string population)
        {
            try
            {
                var database = this.DatabaseService.Database;
                database.Init();

                var config = new Config
                {
                    RecordsByClass = new RecordsFromResource(database.MetaPopulation).RecordsByClass,
                    Translation = new TranslationsFromResource(database.MetaPopulation, new TranslationConfiguration())
                };

                new Setup(database, config).Apply();

                using (var transaction = database.CreateTransaction())
                {
                    transaction.Derive();
                    transaction.Commit();

                    var administrator = transaction.Build<Person>(v => v.UserName = "administrator");
                    transaction.Scoped<UserGroupByUniqueId>().Administrators.AddMember(administrator);
                    transaction.Services.Get<IUserService>().User = administrator;

                    new TestPopulation(transaction).Apply();
                    transaction.Derive();
                    transaction.Commit();
                }

                return this.Ok();
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Restart()
        {
            try
            {
                this.DatabaseService.Database = this.DatabaseService.Build();
                return this.Ok();
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            try
            {
                var timeService = this.DatabaseService.Database.Services.Get<ITime>();
                timeService.Shift = new TimeSpan(days, hours, minutes, seconds);
                return this.Ok();
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Exception");
                return this.BadRequest(e.Message);
            }
        }
    }
}
