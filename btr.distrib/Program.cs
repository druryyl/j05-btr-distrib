using btr.application;
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
using btr.distrib.Helpers;
using btr.application.SupportContext.TglJamAgg;
using btr.distrib.Browsers;
using btr.infrastructure.SupportContext.TglJamAgg;
using btr.distrib.SalesContext.FakturAgg;
using Microsoft.Win32;
using System.Threading;
using System.Collections.Generic;
using btr.distrib.SalesContext.RuteAgg;

namespace btr.distrib
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXdfc3VTQmhZWUF0WEI=");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalUnhandledExceptionHandler);
            //Application.ThreadException += new ThreadExceptionEventHandler(GlobalThreadExceptionHandler);


            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            ConfigureServices(services, configuration);

            if (!IsSuccessLogin(services, out var user))
                return;
            else
            {
                var form = GetMainForm(services, user);
                Application.Run(form);
            }
        }

        #region GLOBAL-CATCH-ARGUMENT-EXCEPTION and KEYNOTFOUND-EXCEPTION
        private static void GlobalThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception is Exception ex)
            {
                // Filter specific exceptions
                if (ex is ArgumentException || ex is KeyNotFoundException)
                {
                    MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // Filter specific exceptions
                if (ex is ArgumentException || ex is KeyNotFoundException)
                {
                    MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        #endregion

        private static Form GetMainForm(ServiceCollection services, string user)
        {
            var form = new MainForm(services);
            form.SetUser(user);

            return form;
        }

        private static bool IsSuccessLogin(ServiceCollection services, out string user)
        {
            user = string.Empty;
            var servicesProvider = services.BuildServiceProvider();
            var login = servicesProvider.GetRequiredService<LoginForm>();
            var server = GetServerDb();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            login.SetStatus(server, version.ToString());
            login.StartPosition = FormStartPosition.CenterScreen;
            var diagResult = login.ShowDialog();
            if (diagResult != DialogResult.OK) return false;
            user = login.UserId;
            return true;
        }
        
        public static string GetServerDb()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"DrurySoftware\BTRApp");

            if (key != null)
            {
                // Read the value; if it doesn't exist, return null
                string server = key.GetValue("Server") as string;
                string database = key.GetValue("Database") as string;

                // Close the key to release the resource
                key.Close();

                return $"{database ?? string.Empty}@{server ?? string.Empty}";
            }

            return $"[db]@[server]";
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(typeof(ApplicationAssemblyAnchor).Assembly));
            services.AddValidatorsFromAssembly(Assembly.Load("btr.application"));
            services.AddScoped<INunaCounterBL, NunaCounterBL>();
            services.AddScoped<DateTimeProvider, DateTimeProvider>();
            services.AddScoped<ITglJamDal, TglJamDal>();

            services
                .Scan(selector => selector
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaWriter<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaWriter2<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaService<,>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaService<>)))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                    .FromAssemblyOf<ApplicationAssemblyAnchor>()
                        .AddClasses(c => c.AssignableTo(typeof(INunaServiceVoid<>)))
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
                        .AddClasses(c => c.AssignableTo(typeof(IListData<,,,>)))
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
