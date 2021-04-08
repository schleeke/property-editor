using System;
using System.Globalization;
using System.Windows.Data;

namespace ApplicationProperties.Converters {

    /// <summary>
    /// Converts an <see cref="Models.ApplicationPropertiesEntry"/> into its editor type value.
    /// </summary>
    public class ValueTypeToEditorTypeConverter : IValueConverter {

        /// <summary>
        /// Specifies the type of editor to use for a value.
        /// </summary>
        public enum EditorTypeEnum {
            /// <summary>
            /// Unknown edotor type.
            /// </summary>
            Unknown = 0,
            
            /// <summary>
            /// Use a textbox for editing.
            /// </summary>
            Textbox = 1,
            
            /// <summary>
            /// Use a Combobox for editing.
            /// </summary>
            Combobox = 2,
            
            /// <summary>
            /// Use the array editor for editing.
            /// </summary>
            Array = 3,
            
            /// <summary>
            /// Use the number picker for editing.
            /// </summary>
            Number = 4,

            /// <summary>
            /// Use the editor for boolean values.
            /// </summary>
            Boolean = 5,

            /// <summary>
            /// Use the editor for SQL connection strings.
            /// </summary>
            Jdbc = 6
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(null == value) { return EditorTypeEnum.Unknown; }
            if(value.GetType() != typeof(Models.ApplicationPropertiesEntry)) { return EditorTypeEnum.Unknown; }
            var entry = value as Models.ApplicationPropertiesEntry;

            if(entry.Defaults.Count > 0) { return EditorTypeEnum.Combobox; }
            if(entry.IsArray) { return EditorTypeEnum.Array; }
            if(entry.EntryType == Models.ApplicationPropertiesEntry.EntryTypeEnum.Number) { return EditorTypeEnum.Number; }
            if (entry.EntryType == Models.ApplicationPropertiesEntry.EntryTypeEnum.Boolean) { return EditorTypeEnum.Boolean; }
            if(entry.EntryType == Models.ApplicationPropertiesEntry.EntryTypeEnum.Jdbc) { return EditorTypeEnum.Jdbc; }
            return EditorTypeEnum.Textbox;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
