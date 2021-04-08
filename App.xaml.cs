using Prism.Events;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace ApplicationProperties {

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication {
        
        /// <summary>
        /// The command line arguments. Can be NULL.
        /// </summary>
        public static string[] CommandLineArguments;

        /// <inheritdoc/>
        protected override Window CreateShell() => Container.Resolve<Views.MainView>();

        /// <inheritdoc/>
        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<Services.IConfigurationService>(() => { return new Services.JsonConfigurationService(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "app-settings.json")); });
            containerRegistry.RegisterSingleton<Services.IContentService, Services.ContentService>();
            containerRegistry.RegisterSingleton<Services.IStatusService, Services.StatusService>();
            containerRegistry.Register<Services.ICryptoService, Services.JasyptCryptoService>();
            containerRegistry.RegisterDialog<Views.AddArrayItemView>(Views.AddArrayItemView.DIALOG_NAME);
            containerRegistry.RegisterDialog<Views.SettingsView>(Views.SettingsView.DIALOG_NAME);
            containerRegistry.RegisterDialog<Views.SetPasswordView>(Views.SetPasswordView.DIALOG_NAME);
            containerRegistry.RegisterDialog<Views.AboutView>(Views.AboutView.DIALOG_NAME);
        }

        private void PrismApplication_Startup(object sender, StartupEventArgs e) => CommandLineArguments = e.Args;
    }
}
