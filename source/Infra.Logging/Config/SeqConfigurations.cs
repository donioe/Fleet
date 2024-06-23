using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infra.Logging.Config;

public class SeqConfigurations
{
    public const string SeqConfiguration = "SeqConfiguration";
    public const string IsSeqEnabledString = "IsSeqEnabled";
    public const string UrlString = "Url";
    
    public static void ConfigureSeq(HostBuilderContext hostBuilderContext, LoggerConfiguration loggerConfiguration)
    {
        var isSeqEnabled = hostBuilderContext.Configuration.GetSection(SeqConfiguration).GetValue<bool>(IsSeqEnabledString);
        if (isSeqEnabled)
        {
            var seqUrl = hostBuilderContext.Configuration.GetSection(SeqConfiguration).GetValue<string>(UrlString);
            if (string.IsNullOrEmpty(seqUrl))
            {
                throw new ArgumentNullException(nameof(seqUrl), "Seq url is not configured in settings");
            }

            loggerConfiguration.WriteTo.Seq(seqUrl);
        }
    }


}