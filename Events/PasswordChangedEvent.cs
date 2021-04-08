using Prism.Events;
using System.Security;

namespace ApplicationProperties.Events {

    /// <summary>
    /// Raised when the crypto password was changed.
    /// </summary>
    public class PasswordChangedEvent : PubSubEvent<SecureString> { }
}
