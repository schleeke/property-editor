using Prism.Events;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Jasypt-Implementation of the <see cref="ICryptoService"/> interface.
    /// </summary>
    public class JasyptCryptoService : BindableBase, ICryptoService {
        private static IConfigurationService _cfg;
        private readonly IEventAggregator _events;
        private SecureString _pwd;
        private bool _ready;
        private string _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="JasyptCryptoService"/> class.
        /// </summary>
        /// <param name="events">The event bus.</param>
        /// <param name="config">The configuration/settings.</param>
        public JasyptCryptoService(IEventAggregator events, IConfigurationService config) {
            _events = events;
            _cfg = config;
            _cfg.PropertyChanged += OnConfigurationChanged;
            _events.GetEvent<Events.PasswordChangedEvent>().Subscribe(OnPasswordChanged);
            Status = ToString();
        }

        private void OnConfigurationChanged(object sender, PropertyChangedEventArgs e) {
            if(!e.PropertyName.Equals("CryptoBinariesDirectory")) { return; }
            IsReady = GetIsReady();
            Status = ToString();
        }

        private bool GetIsReady() => GetJavaPresent() && GetJasyptPresent() && Password != null;

        /// <inheritdoc/>
        public bool IsReady { get => _ready; set => SetProperty(ref _ready, value); }

        /// <inheritdoc/>
        public SecureString Password { get => _pwd; private set => SetProperty(ref _pwd, value); }

        /// <inheritdoc/>
        public string Status { get => _status; private set => SetProperty(ref _status, value); }

        /// <inheritdoc/>
        public string Decrypt(string cipher) => ExecuteCrypto("decrypt.bat", cipher);

        /// <inheritdoc/>
        public string Encrypt(string plainText) => ExecuteCrypto("encrypt.bat", plainText);

        /// <inheritdoc/>
        public ICryptoService SetPassword(string password) {
            var secStr = new SecureString();
            foreach (var c in password.ToCharArray()) { secStr.AppendChar(c); }
            return SetPassword(secStr);
        }

        /// <inheritdoc/>
        public ICryptoService SetPassword(SecureString password) {
            Password = password;
            IsReady = GetIsReady();
            return this;
        }

        /// <inheritdoc/>
        public override string ToString() {
            var isJavaPresent = GetJavaPresent();
            var isJasyptPresent = GetJasyptPresent();
            var hasPwd = Password != null;
            if (hasPwd && isJasyptPresent && isJavaPresent) { return "Jasypt crypto is ready."; }
            if (!isJavaPresent) { return "Jasypt crypto is not ready: java is missing."; }
            if (!isJasyptPresent) { return "Jasypt crypto is not ready: cannot find jasypt binaries."; }
            return "Jasypt crypto is not ready: password not set.";
        }

        private bool GetJavaPresent() {
            var fileName = "java.exe";
            if (System.IO.File.Exists(fileName)) { return true; }
            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(System.IO.Path.PathSeparator)) { if (System.IO.File.Exists(System.IO.Path.Combine(path, fileName))) {  return true; } }
            return false;
        }

        private bool GetJasyptPresent() {
            if (string.IsNullOrEmpty(_cfg.CryptoBinariesDirectory)) { _cfg.Load(); }
            var encExists = System.IO.File.Exists(System.IO.Path.Combine(_cfg.CryptoBinariesDirectory, "encrypt.bat"));
            var decExists = System.IO.File.Exists(System.IO.Path.Combine(_cfg.CryptoBinariesDirectory, "decrypt.bat"));
            return encExists & decExists;
        }

        private string ExecuteCrypto(string executable, string input) {
            if (!IsReady) { throw new InvalidOperationException("The crypto service cannot be used."); }
            var plain = string.Empty;
            var outputReached = false;
            var startInfo = new ProcessStartInfo {
                FileName = System.IO.Path.Combine(_cfg.CryptoBinariesDirectory, executable),
                UseShellExecute = false,
                WorkingDirectory = _cfg.CryptoBinariesDirectory,
                Arguments = $"input=\"{input}\" password=\"{GetPassword()}\"",
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            var proc = new Process() { StartInfo = startInfo };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                var line = proc.StandardOutput.ReadLine();
                if (line.StartsWith("----OUTPUT")) {
                    outputReached = true;
                    continue;
                }
                if (!outputReached) { continue; }
                if (string.IsNullOrEmpty(line.Trim())) { continue; }
                plain = line.Trim();
            }
            proc.WaitForExit();
            return plain;
        }

        private string GetPassword() {
            var valuePtr = IntPtr.Zero;
            try {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(Password);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private void OnPasswordChanged(SecureString password) => SetPassword(password);
    }
}
