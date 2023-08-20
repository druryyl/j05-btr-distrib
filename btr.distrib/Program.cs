using btr.application;
using btr.distrib.Browsers;
using btr.distrib.PrintDocs;
using btr.distrib.SharedForm;
using btr.infrastructure;
using btr.infrastructure.Helpers;
using btr.nuna.Application;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace btr.distrib
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            ConfigureServices(services, configuration);
            var form = new MainForm(services);
            Application.Run(form);

            //using (var sp = services.BuildServiceProvider())
            //{
            //    var form = sp.GetRequiredService<MainForm>();
            //    Application.Run(form);
            //}
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(typeof(ApplicationAssemblyAnchor).Assembly));
            services.AddValidatorsFromAssembly(Assembly.Load("btr.application"));
            services.AddScoped<INunaCounterBL, NunaCounterBL>();
            services.AddScoped<DateTimeProvider, DateTimeProvider>();

            services
                .Scan(selector => selector
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaWriter<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaBuilder<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime());

            services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SECTION_NAME));
            services.Configure<PrinterOptions>(configuration.GetSection(PrinterOptions.SECTION_NAME));
            services.AddScoped<INunaCounterDal, ParamNoDal>();

            services
                .Scan(selector => selector
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IInsert<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IUpdate<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IDelete<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IGetData<,>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IListData<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IListData<,>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IListData<,,>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaService<,>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<InfrastructureAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaService<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime());

            services
                .Scan(selector => selector
                    .FromAssemblyOf<WinformAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(Form)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelf()
                        .WithTransientLifetime()
                    .FromAssemblyOf<WinformAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IPrintDoc<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<WinformAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IBrowser)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<WinformAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IBrowser<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<WinformAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IBrowseEngine<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<WinformAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(IQueryBrowser<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime());
        }
    }
}
