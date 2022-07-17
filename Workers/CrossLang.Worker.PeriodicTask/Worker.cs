using CrossLang.Worker.PeriodicTask.Jobs;

namespace CrossLang.Worker.PeriodicTask;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IConfiguration _configuration;

    private readonly PackageExpJob _packageExpJob;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, PackageExpJob packageExpJob)
    {
        _logger = logger;
        _configuration = configuration;
        _packageExpJob = packageExpJob;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            _packageExpJob.Start();

            await Task.Delay(300000, stoppingToken);
        }
    }
}

