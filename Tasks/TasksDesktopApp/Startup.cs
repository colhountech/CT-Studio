using ElectronNET.API;
using CloudStorage;
using TasksServices.Repository;
using TasksServices.Services;
using TasksDesktopApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddRazorPages();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddTransient<IToDoItemEventService, ToDoItemEventService>();
        //services.AddTransient<IToDoItemRepository, ToDoItemFileRepository>();
        //services.AddTransient<IToDoItemRepository, ToDoItemAzureBlobRepository>();
        services.AddTransient<ICloudStorageRepository, AzureStorageRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseHsts();
        }
        else
        {
            app.UseMigrationsEndPoint(); // Apply EF pending migrations (dev only)
        }
        app.UseExceptionHandler("/Error");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAuthentication();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });

        Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());

    }
}
