using Prism.Events;

namespace ApplicationProperties.Events {

    /// <summary>
    /// Event for signalling the selection of a new application.properties file.
    /// </summary>
    public class NewFileSelectedEvent : PubSubEvent<NewFileSelectedEventArgs> { }

    /// <summary>
    /// Event arguments for the <see cref="NewFileSelectedEvent"/>.
    /// </summary>
    public class NewFileSelectedEventArgs {

        /// <summary>
        /// The full name/path of the application.properties file.
        /// </summary>
        public string FullName { get; set; }
    }


}
