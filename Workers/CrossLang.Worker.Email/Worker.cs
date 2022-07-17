using CrossLang.Worker.Email.Jobs;

namespace CrossLang.Worker.Email;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private SyncJob _syncJob;

    private bool jobStarted = false;

    public Worker(ILogger<Worker> logger, SyncJob syncJob)
    {
        _syncJob = syncJob;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if(!jobStarted)
            {
                _syncJob.Start();
                jobStarted = true;
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}

