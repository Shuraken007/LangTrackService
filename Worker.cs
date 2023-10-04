namespace App.LangTracker;

public class Worker : BackgroundService
{
    private readonly LangTrack langTrack;

    public Worker()
    {
        this.langTrack = new LangTrack();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            langTrack.HandleCurrentLanguage();
            await Task.Delay(200, stoppingToken);
        }
    }
}
