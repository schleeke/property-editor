using ApplicationProperties.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Service for accessing the file's content.
    /// </summary>
    public class ContentService : BindableBase, IContentService {
        private readonly IStatusService _status;
        private readonly IConfigurationService _cfg;
        private ApplicationPropertiesEntry _selectedEntry;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentService"/> class.
        /// </summary>
        /// <param name="status">The application's status.</param>
        /// <param name="config">The configuration/settings.</param>
        public ContentService(IStatusService status, IConfigurationService config) {
            _status = status;
            _cfg = config;
            Entries = new ObservableCollection<ApplicationPropertiesEntry>();
            _status.PropertyChanged += OnStatusChanged;
            Entries.CollectionChanged += OnEntriesChanged;
        }


        /// <inheritdoc/>
        public ApplicationPropertiesEntry SelectedEntry {
            get => _selectedEntry; set {
                if(SetProperty(ref _selectedEntry, value)) {
                    if(null == _selectedEntry) { return; }
                    SelectedEntry.PropertyChanged += OnSelectedEntryChanged;
                }
            }
        }

        /// <inheritdoc/>
        public ObservableCollection<ApplicationPropertiesEntry> Entries { get; }

        /// <inheritdoc/>
        public bool HasChanges { get => _hasChanges; set => SetProperty(ref _hasChanges, value); }

        /// <inheritdoc/>
        public void SetChanged(PropertyTypeEnum property) {
            switch (property) {
                case PropertyTypeEnum.SelectedEntry:
                    RaisePropertyChanged(nameof(SelectedEntry));
                    break;
                case PropertyTypeEnum.Entries:
                    RaisePropertyChanged(nameof(Entries));
                    break;
            }
        }

        private void OnStatusChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("FilePath")) { return; }
            SelectedEntry = null;
            Entries.Clear();
            var newFile = new ApplicationPropertiesFile(_status.FilePath, _cfg);
            newFile.Read();
            Entries.AddRange(newFile.Entries);
            HasChanges = false;
            RaisePropertyChanged(nameof(Entries));
            foreach (var item in Entries) { item.TrackingActive = true; }
        }

        private void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e) {
            HasChanges = true;
        }

        private void OnSelectedEntryChanged(object sender, PropertyChangedEventArgs e) {
            if(null == SelectedEntry) { return; }
            var propertyName = SelectedEntry.Identifier;
            var index = Entries.IndexOf(Entries.Single(entry => entry.Identifier.Equals(propertyName)));
            Entries[index] = SelectedEntry;
        }

    }
}
