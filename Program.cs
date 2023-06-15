var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDbContext<DbContext, DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("App"));
});

builder.Services.AddHangfire(configuration => configuration
    .UseSerilogLogProvider()
    .UseInMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddJobServices();

var app = builder.Build();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "Jobs Dashboard",
});

Installer.InitSerilog();
app.ScheduleJobs();

app.Run();
