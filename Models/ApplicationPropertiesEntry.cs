using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ApplicationProperties.Models {

    /// <summary>
    /// Represents an entry within an application properties file.
    /// </summary>
    public class ApplicationPropertiesEntry : ChangeTrackingBase {
        /// <summary>
        /// Defines an entries value type.
        /// </summary>
        public enum EntryTypeEnum {
            /// <summary>
            /// Unknown type (default).
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Text.
            /// </summary>
            Text = 1,
            /// <summary>
            /// An integer number.
            /// </summary>
            Number = 2,
            /// <summary>
            /// Point number value.
            /// </summary>
            Float = 3,
            /// <summary>
            /// A boolean (true/false) value.
            /// </summary>
            Boolean = 4,
            /// <summary>
            /// URL for accessing an SQL server.
            /// </summary>
            Jdbc = 5
        }
        private string _grp;
        private string _id;
        private string _val;
        private string _comment;
        private bool _arr;
        private bool _enc;
        private EntryTypeEnum _t;
        private JdbcConnectionString _conStr;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPropertiesEntry"/> class.
        /// </summary>
        /// <param name="parent">The parent file containing this entry.</param>
        /// <exception cref="ArgumentNullException">thrown if the specified parent file is NULL.</exception>
        public ApplicationPropertiesEntry(ApplicationPropertiesFile parent) : base() {
            File = parent ?? throw new ArgumentNullException("The parent file for the entry is NULL.");
            Defaults = new ObservableCollection<string>();
            ArrayValues = new ObservableCollection<string>();
            Group = "Default";
        }

        /// <summary>
        /// The (unique) configuration entry identifier.
        /// </summary>
        public string Identifier { get => _id; set => SetProperty(ref _id, value); }

        /// <summary>
        /// The configuration entry value.
        /// </summary>
        public string Value {
            get => _val; set {
                if(SetProperty(ref _val, value)) {
                    if(EntryType == EntryTypeEnum.Text && value.IsEncapsulated("ENC(",")")) { IsEncrypted = true; }
                    if (EntryType == EntryTypeEnum.Text && value.IsEncapsulated("ENC(", ")") == false) { IsEncrypted = false; }
                    if (IsArray) {
                        ArrayValues.Clear();
                        var arrayVals = Value.Split(',').Select(val => val.Trim()).Where(val => val.IsNullOrEmpty() == false).ToList();
                        if(arrayVals.Count > 0) { ArrayValues.AddRange(arrayVals); }
                    }
                    if(EntryType == EntryTypeEnum.Jdbc) {
                        ConnectionString = new JdbcConnectionString(value);
                    }
                }
            }
        }

        /// <summary>
        /// An (optional) comment for the configuration entry.
        /// </summary>
        public string Comment { get => _comment; set => SetProperty(ref _comment, value); }

        /// <summary>
        /// The entries value type.
        /// </summary>
        public EntryTypeEnum EntryType { get => _t; set => SetProperty(ref _t, value); }

        /// <summary>
        /// Indicates if the entry can have multiple values.
        /// </summary>
        public bool IsArray { get => _arr; set => SetProperty(ref _arr, value); }

        /// <summary>
        /// Indicates if the entry is encrypted.
        /// </summary>
        public bool IsEncrypted { get => _enc; set => SetProperty(ref _enc, value); }

        /// <summary>
        /// The JDBC connection string.
        /// Only filled if <see cref="EntryType"/> equals <see cref="EntryTypeEnum.Jdbc"/>.
        /// </summary>
        public JdbcConnectionString ConnectionString {
            get => _conStr;
            set { if(SetProperty(ref _conStr, value)) { ConnectionString.PropertyChanged += OnConnectionStringChanged; } }
        }

        /// <summary>
        /// The name of the group for this entry/setting.
        /// </summary>
        public string Group { get => _grp; set => SetProperty(ref _grp, value); }

        /// <summary>
        /// A list of default/allowed values.
        /// </summary>
        public ObservableCollection<string> Defaults { get; set; }

        /// <summary>
        /// A reference to the parent file of the entry.
        /// </summary>
        public ApplicationPropertiesFile File { get; }

        /// <summary>
        /// The different values derived from the <see cref="Value"/> as an array.
        /// </summary>
        public ObservableCollection<string> ArrayValues { get; }

        /// <inheritdoc/>
        public override string ToString() => $"{Identifier} = {Value}";

        private void OnConnectionStringChanged(object sender, PropertyChangedEventArgs e) {
            RaisePropertyChanged(nameof(ConnectionString));
            Value = ConnectionString.ToString();
        }

    }

}
