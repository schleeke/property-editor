using System;
using System.Globalization;
using System.Windows.Data;

namespace ApplicationProperties.Converters {

    /// <summary>
    /// Converts an integer number to a boolean depending on its value.
    /// Returns true if the number is greater than zero (0).
    /// </summary>
    public class NumberLargerZeroConverter : IValueConverter {
        
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(null == value) { return false; }
            if(!int.TryParse(value.ToString(), out int result)) { return false; }
            return result > 0;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return 0;
        }
    }
}
