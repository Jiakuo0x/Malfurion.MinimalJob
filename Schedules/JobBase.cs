namespace Schedules;
public abstract class JobBase
{
    private static Dictionary<string, Serilog.Core.Logger> _JobLoggersDic = new Dictionary<string, Serilog.Core.Logger>();
    protected readonly Serilog.Core.Logger Logger;
    public JobBase()
    {
        if (_JobLoggersDic.ContainsKey(JobName))
        {
            Logger = _JobLoggersDic[JobName];
            return;
        }

        Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File($"logs\\{DateTime.Now.Year}\\{DateTime.Now.Month}\\{JobName}.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        _JobLoggersDic.Add(JobName, Logger);
    }

    public abstract string JobName { get; }
    public abstract string CronExpression { get; }
    public async Task Execute()
    {
        Logger.Information($"[{JobName} Job] Start");
        try
        {
            await DoWork();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            throw ex;
        }
        Logger.Information($"[{JobName} Job] End");
    }

    public abstract Task DoWork();
}