namespace Schedules;

public class DemoJob : JobBase
{
    public override string JobName => nameof(DemoJob);
    public override string CronExpression => Cron.Minutely();

    private readonly DbContext _db;
    public DemoJob(DbContext db)
    {
        _db = db;
    }
    public override async Task Execute()
    {
        await Task.Run(() =>
        {
            Logger.Information("test add start");
            _db.Set<DemoModel>().Add(new DemoModel { Name = $"minimal job {Guid.NewGuid().ToString()}" });
            _db.SaveChanges();
            Logger.Information("test add end");
        });
    }
}