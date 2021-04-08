using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Removes an item from the array of the selected entry.
    /// </summary>
    public class RemoveArrayItemCommand : ICommand {
        private readonly Services.IContentService _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveArrayItemCommand"/> class.
        /// </summary>
        /// <param name="content">The file's content.</param>
        public RemoveArrayItemCommand(Services.IContentService content) {
            _content = content;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            if(null == parameter) { return; }
            if (string.IsNullOrEmpty(parameter as string)) { return; }
            _content.SelectedEntry.ArrayValues.Remove(parameter as string);
            _content.SelectedEntry.Value = string.Join(", ", _content.SelectedEntry.ArrayValues);
        }
    }
}
