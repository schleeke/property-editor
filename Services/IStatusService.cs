using System;
using System.ComponentModel;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Accessing the application's status.
    /// </summary>
    public interface IStatusService : INotifyPropertyChanged {
        
        /// <summary>
        /// Sets the application's current status and returns its id
        /// for later removal if the reason for the state change is obsolete.
        /// </summary>
        /// <param name="message">
        /// The message that describes the current status.
        /// Will be displayed in the status bar.
        /// </param>
        /// <returns>The id of the status for later removal.</returns>
        Guid SetStatus(string message);

        /// <summary>
        /// Removes a status from the status list.
        /// </summary>
        /// <param name="id">The id that was returned from <see cref="SetStatus(string)"/>.</param>
        bool RemoveStatus(Guid id);

        /// <summary>
        /// The application's current status.
        /// </summary>
        string ApplicationStatus { get; set; }

        /// <summary>
        /// The file path of the currently selected application.properties file.
        /// </summary>
        string FilePath { get; set; }
    }
}
