using ApplicationProperties.Events;
using ApplicationProperties.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Implementation of <see cref="IStatusService"/>.
    /// </summary>
    public class StatusService : BindableBase, IStatusService {
        private readonly Dictionary<Guid, string> _statusList = new Dictionary<Guid, string>();
        private string _appStatus;
        private readonly IEventAggregator _events;
        private Models.ApplicationPropertiesEntry _selectedEntry;
        private string _file;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusService"/> class.
        /// </summary>
        /// <param name="events">The application's internal event bus (PubSub).</param>
        public StatusService(IEventAggregator events) {
            ApplicationStatus = "Ready.";
            _events = events;
            _events.GetEvent<NewFileSelectedEvent>().Subscribe(OnNewFileSelected);
        }

        /// <inheritdoc/>
        public string ApplicationStatus { get => _appStatus; set => SetProperty(ref _appStatus, value); }

        /// <inheritdoc/>
        public string FilePath {
            get => _file; set {
                _file = value;
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        /// <inheritdoc/>
        public ApplicationPropertiesEntry SelectedEntry {
            get => _selectedEntry; set {
                if(SetProperty(ref _selectedEntry, value)) {
                    _events.GetEvent<Events.EntrySelectedEvent>().Publish(new EntrySelectedEventArgs { SelectedEntry = value }); ;
                }
            }
        }

        /// <inheritdoc/>
        public bool RemoveStatus(Guid id) {
            if (!_statusList.ContainsKey(id)) { return false; }
            var retVal = _statusList.Remove(id);
            ApplicationStatus = _statusList.Count < 1 
                ? "Ready."
                : _statusList.Last().Value;
            return retVal;
        }

        /// <inheritdoc/>
        public Guid SetStatus(string message) {
            var newId = Guid.NewGuid();
            _statusList.Add(newId, message);
            ApplicationStatus = message;
            return newId;
        }

        private void OnNewFileSelected(NewFileSelectedEventArgs args) {
            FilePath = args.FullName;
            SelectedEntry = null;
        }

    }
}
