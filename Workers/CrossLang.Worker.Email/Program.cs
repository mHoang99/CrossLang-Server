using CrossLang.Worker.Email;
using CrossLang.Worker.Email.Jobs;
using CrossLang.Worker.Email.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddSingleton<SyncJob>();

        services.AddSingleton<IEmailService, EmailService>();
    })
    .Build();

await host.RunAsync();

