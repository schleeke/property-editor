using Prism.Events;

namespace ApplicationProperties.Events {

    /// <summary>
    /// Event for adding a new item to the selected array.
    /// </summary>
    public class ArrayItemAddedEvent : PubSubEvent<ArrayItemAddedEventArgs> { }

    /// <summary>
    /// Arguments for the <see cref="ArrayItemAddedEvent"/>.
    /// </summary>
    public class ArrayItemAddedEventArgs {
        
        /// <summary>
        /// The name/text of the new item.
        /// </summary>
        public string NewItem { get; set; }
    }

}
