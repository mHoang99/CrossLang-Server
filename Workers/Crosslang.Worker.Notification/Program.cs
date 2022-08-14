using Crosslang.Worker.Notification;
using Crosslang.Worker.Notification.Hubs;
using Crosslang.Worker.Notification.Jobs;
using Crosslang.Worker.Notification.Services;
using CrossLang.DBHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.SetIsOriginAllowed(_ => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));

builder.Services.AddSingleton<SyncJob>();

builder.Services.AddSingleton<IDBContext, DBContext>();

builder.Services.AddSingleton<INotificationService, NotificationService>();

builder.Services.AddSignalR(cfg => cfg.EnableDetailedErrors = true);

builder.Services.AddHostedService<Worker>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001); // to listen for incoming http connection on port 5001
    options.ListenAnyIP(7001, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
});

var app = builder.Build();

app.UseCors("MyPolicy");

app.MapHub<NotificationHub>("/hubs/notification");

await app.RunAsync();