using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace LoopStats.Common.ApiConfiguration;

public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    public OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = "1.0.0",
        Title = "Loopring Statistics",
        Description = "API for getting multiple types of statistics from the loopring protocol. Using data from juanmardefago (https://api.thegraph.com/subgraphs/name/juanmardefago/loopring36)",
        TermsOfService = new Uri("https://github.com/danielsolistensvik/LoopStats"),
        Contact = new OpenApiContact()
        {
            Name = "Daniel Soli Stensvik",
            Email = "daniel.soli.stensvik@gmail.com",
            Url = new Uri("https://github.com/danielsolistensvik/LoopStats/issues"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>()
    {
        new OpenApiServer() { Url = "https://loopystats.azurewebsites.net/api/swagger/ui"},
        new OpenApiServer() { Url = "https://loopy.sv2web.no/api/swagger/ui"}
    };

    public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;
    public bool ForceHttp { get; set; } = true;
    public bool ForceHttps { get; set; } = true;
    public bool IncludeRequestingHostName { get; set; } = true;
}
