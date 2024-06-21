using ShipApi;
using ShipApi.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddOptions<FleetSettings>();
builder.Services.Configure<FleetSettings>(builder.Configuration.GetSection(FleetSettings.FleetSettingsString));

var host = builder.Build();
host.Run();