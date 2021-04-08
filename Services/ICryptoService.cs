using System.ComponentModel;
using System.Security;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Interface for doing crypto stuff.
    /// </summary>
    public interface ICryptoService : INotifyPropertyChanged {
        
        /// <summary>
        /// Indicates if the crypt module/service can be used.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// The password for the symmetric encryption.
        /// Set by <see cref="SetPassword(string)"/>.
        /// </summary>
        SecureString Password { get; }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="password"></param>
        ICryptoService SetPassword(string password);

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="password"></param>
        ICryptoService SetPassword(SecureString password);

        /// <summary>
        /// Encrypts a string with the <see cref="Password"/>.
        /// </summary>
        /// <param name="plainText">The plain text string to encrypt.</param>
        /// <returns>The cipher for the plain text.</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts a cipher with the <see cref="Password"/>.
        /// </summary>
        /// <param name="cipher">The cipher to decrypt.</param>
        /// <returns>The decrypted plain text.</returns>
        string Decrypt(string cipher);

        /// <summary>
        /// The crypto module status.
        /// </summary>
        string Status { get; }
    }
}
