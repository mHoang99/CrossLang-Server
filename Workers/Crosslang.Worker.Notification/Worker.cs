using Crosslang.Worker.Notification.Jobs;

namespace Crosslang.Worker.Notification;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private SyncJob _syncJob;

    private bool jobStarted = false;

    public Worker(ILogger<Worker> logger, SyncJob syncJob)
    {
        _logger = logger;
        _syncJob = syncJob;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            if (!jobStarted)
            {
                _syncJob.Start();
                jobStarted = true;
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}

