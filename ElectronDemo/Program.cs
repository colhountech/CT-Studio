using ElectronNET.API;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseElectron(args);
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .RunAsync();

