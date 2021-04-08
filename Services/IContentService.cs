using System.ComponentModel;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Field definitions for the <see cref="IContentService"/>.
    /// </summary>
    public enum PropertyTypeEnum {
        
        /// <summary>
        /// Unknown field.
        /// </summary>
        Unknown,
        
        /// <summary>
        /// The selected entry.
        /// </summary>
        SelectedEntry,
        
        /// <summary>
        /// The file's content.
        /// </summary>
        Entries
    }


    /// <summary>
    /// Interface for accessing the content.
    /// </summary>
    public interface IContentService : INotifyPropertyChanged {

        /// <summary>
        /// The currently selected entry.
        /// </summary>
        Models.ApplicationPropertiesEntry SelectedEntry { get; set; }

        /// <summary>
        /// The content.
        /// </summary>
        System.Collections.ObjectModel.ObservableCollection<Models.ApplicationPropertiesEntry> Entries { get; }

        /// <summary>
        /// Indicates if the content has been changed.
        /// </summary>
        bool HasChanges { get; set; }

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged"/> for the specified property.
        /// </summary>
        /// <param name="property">The property to raise the change for.</param>
        void SetChanged(PropertyTypeEnum @property);
    }
}
