using Prism.Events;

namespace ApplicationProperties.Events {

    /// <summary>
    /// Fired when a value within the file changes.
    /// </summary>
    public class FileChangedEvent : PubSubEvent<FileChangedEventArgs> {}

    /// <summary>
    /// Arguments for the <see cref="FileChangedEvent"/>.
    /// </summary>
    public class FileChangedEventArgs {}

}
