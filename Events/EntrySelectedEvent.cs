using Prism.Events;

namespace ApplicationProperties.Events {

    /// <summary>
    /// Fired when a new entry is selected.
    /// </summary>
    public class EntrySelectedEvent : PubSubEvent<EntrySelectedEventArgs> { }

    /// <summary>
    /// Argument for the <see cref="EntrySelectedEvent"/>.
    /// </summary>
    public class EntrySelectedEventArgs {

        /// <summary>
        /// The currently selected entry.
        /// </summary>
        public Models.ApplicationPropertiesEntry SelectedEntry { get; set; }
    }

}
