using System;
using System.Globalization;
using System.Windows.Data;

namespace ApplicationProperties.Converters {

    /// <summary>
    /// Converts an <see cref="Models.ApplicationPropertiesEntry.EntryTypeEnum"/> to a text and vice versa.
    /// </summary>
    public class EntryTypeToTextConverter : IValueConverter {
        
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(null == value) { return string.Empty; }
            if(value.GetType() != typeof(Models.ApplicationPropertiesEntry.EntryTypeEnum)) { return string.Empty; }
            var retVal = (Models.ApplicationPropertiesEntry.EntryTypeEnum)value;
            return retVal.ToString().Encapsultate("(",")");
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if(null == value) { return Models.ApplicationPropertiesEntry.EntryTypeEnum.Unknown; }
            if (value.GetType() != typeof(string)) { return Models.ApplicationPropertiesEntry.EntryTypeEnum.Unknown; }
            var stringValue = (value as string).Uncapsulate("(",")");
            switch (stringValue) {
                case "Unknown":
                    return Models.ApplicationPropertiesEntry.EntryTypeEnum.Unknown;
                case "Text":
                    return Models.ApplicationPropertiesEntry.EntryTypeEnum.Text;
                case "Number":
                    return Models.ApplicationPropertiesEntry.EntryTypeEnum.Number;
                case "Float":
                    return Models.ApplicationPropertiesEntry.EntryTypeEnum.Float;
                case "Boolean":
                    return Models.ApplicationPropertiesEntry.EntryTypeEnum.Boolean;
                default:
                    return Models.ApplicationPropertiesEntry.EntryTypeEnum.Unknown;
            }
        }
    }
}
