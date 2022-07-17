using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Worker.PeriodicTask;
using CrossLang.Worker.PeriodicTask.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddTransient<IDBContext, DBContext>();
        services.AddTransient<IMongoDBContext, MongoContext>();
        services.AddSingleton<PackageExpJob>();
    })
    .Build();

await host.RunAsync();

