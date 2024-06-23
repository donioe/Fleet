using Infra.Logging.Config;
using Microsoft.Extensions.Hosting;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog;

namespace Infra.Logging;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (hostingContext, loggerConfiguration) =>
        {
            BaseLoggerConfig(hostingContext, loggerConfiguration);
            SeqConfigurations.ConfigureSeq(hostingContext, loggerConfiguration);
        };

    private static void BaseLoggerConfig(HostBuilderContext hostBuilderContext, LoggerConfiguration loggerConfiguration)
    {
        var hostEnv = hostBuilderContext.HostingEnvironment;

        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", hostEnv.ApplicationName)
            .Enrich.WithSpan()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .Enrich.WithThreadId();

        loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
    }

}