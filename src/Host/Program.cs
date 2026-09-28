using BigBang.WebServer.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddBigBangServer(options =>
{
    options.WithCustomServerCatalog("Sales")
        .DisplayName("فروش");
});
using var dispose = await builder.AddBigBangCoreWebApplicationAsync();
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
await app.UseBigBangCoreWebApplicationAsync();
app.Run();
