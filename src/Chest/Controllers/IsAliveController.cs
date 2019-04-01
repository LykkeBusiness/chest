﻿// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Ironclad.WebApi
{
    using System.Diagnostics;
    using System.Net;
    using System.Reflection;
    using Chest;
    using Chest.Models;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.SwaggerGen;

    [ApiVersion("1")]
    [ApiVersion("2")]
    [Route("api/isAlive")]
    public class RootController : Controller
    {
        private static readonly RootModel Version =
            new RootModel
            {
                Title = typeof(Program).Assembly.Attribute<AssemblyTitleAttribute>(attribute => attribute.Title),
                Version = typeof(Program).Assembly.Attribute<AssemblyInformationalVersionAttribute>(attribute => attribute.InformationalVersion),
                OS = System.Runtime.InteropServices.RuntimeInformation.OSDescription.TrimEnd(),
                ProcessId = Process.GetCurrentProcess().Id,
            };

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(RootModel))]
        public IActionResult Get() => this.Ok(Version);
    }
}
