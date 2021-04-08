using Prism.Mvvm;
using System.ComponentModel;

namespace ApplicationProperties.Models {

    /// <summary>
    /// Base class for implementing change tracking.
    /// </summary>
    public class ChangeTrackingBase : BindableBase {
        private bool _trackingActive;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTrackingBase"/> class.
        /// </summary>
        public ChangeTrackingBase() => PropertyChanged += OnPropertyChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals(nameof(TrackingActive))) { return; }
            if (e.PropertyName.Equals(nameof(HasChanges))) { return; }
            if (!TrackingActive) { return; }
            HasChanges = true;
        }


        /// <summary>
        /// Indicates if property change tracking is active.
        /// </summary>
        public bool TrackingActive { get => _trackingActive; set => SetProperty(ref _trackingActive, value); }

        /// <summary>
        /// Indicates if properties have been changed.
        /// </summary>
        public bool HasChanges { get => _hasChanges; set => SetProperty(ref _hasChanges, value); }
    }
}
