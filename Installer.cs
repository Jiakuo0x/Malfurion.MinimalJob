using System.Reflection;

public static class Installer
{
    public static void UseSerilog(this WebApplication app)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File($"logs\\{DateTime.Now.Year}\\{DateTime.Now.Month}\\log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(JobBase)));

        foreach (var type in types)
        {
            services.AddTransient(type);
        }

        return services;
    }
    public static void UseJobs(this WebApplication app)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(JobBase)));

        foreach (var type in types)
        {
            var job = app.Services.GetService(type) as JobBase;
            if (job is null) continue;
            RecurringJob.AddOrUpdate(job.JobName, () => job.Execute(), job.CronExpression);
        }
    }
}