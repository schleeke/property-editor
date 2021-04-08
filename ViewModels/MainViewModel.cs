using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// The view model for the <see cref="Views.MainView"/>.
    /// </summary>
    public class MainViewModel : BindableBase {
        private readonly Services.IStatusService _appStatus;
        private readonly Services.ICryptoService _crypto;
        private string _currentStatus;
        private string _cryptoStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="appStatus">The application status.</param>
        /// <param name="crypto">The crypto service</param>
        public MainViewModel(Services.IStatusService appStatus, Services.ICryptoService crypto) {
            var title = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            _appStatus = appStatus;
            _crypto = crypto;
            _currentStatus = _appStatus.ApplicationStatus;
            _cryptoStatus = _crypto.Status;
            _appStatus.PropertyChanged += OnCurrentStatusChanged;
            _crypto.PropertyChanged += OnCryptoChanged;
            Title = $"{title} v{version}";
            OnLoadedCommand = new DelegateCommand(OnLoaded);
        }

        /// <summary>
        /// The application's current status.
        /// </summary>
        public Services.IStatusService ApplicationStatus { get; }

        /// <summary>
        /// The current status of the application.
        /// </summary>
        public string CurrentStatus { get => _currentStatus; set => SetProperty(ref _currentStatus, value); }

        /// <summary>
        /// The status of the crypto service.
        /// </summary>
        public string CryptoStatus { get => _cryptoStatus; set => SetProperty(ref _cryptoStatus, value); }

        /// <summary>
        /// The title of the application window.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Executerd when window is loaded.
        /// </summary>
        public ICommand OnLoadedCommand { get; }

        private void OnCurrentStatusChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("ApplicationStatus")) { return; }
            CurrentStatus = _appStatus.ApplicationStatus;
        }

        private void OnLoaded() {
            if(null == App.CommandLineArguments) { return; }
            if(App.CommandLineArguments.Length < 1) { return; }
            var fileName = App.CommandLineArguments[0];            
            if(!System.IO.File.Exists(fileName)) { return; }
            _appStatus.FilePath = fileName;
        }

        private void OnCryptoChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("Status")) { return; }
            CryptoStatus = _crypto.Status;
        }

    }
}
