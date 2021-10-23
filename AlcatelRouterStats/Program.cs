namespace AlcatelRouterStats
{
    using Microsoft.Extensions.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(b => Startup.BuildConfiguration(b, args))
                .ConfigureServices(Startup.ConfigureServices);
        }
    }
}
