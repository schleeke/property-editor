using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ApplicationProperties.Models {

    /// <summary>
    /// Represents a schema file for an application.properties file.
    /// Defines the properties and their types.
    /// </summary>
    public class ApplicationPropertiesSchema {
        private readonly Services.IConfigurationService _cfg;

        /// <summary>
        /// The full path to the schema XML file.
        /// </summary>
        /// <param name="fullName">The full path to the XML file.</param>
        /// <param name="config">The configuration/settings</param>
        public ApplicationPropertiesSchema(string fullName, Services.IConfigurationService config) {
            FullName = fullName;
            _cfg = config;
        }

        /// <summary>
        /// The full path to the schema XML file.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Returns the entry definitions within the schema file.
        /// </summary>
        /// <returns>A list of entry definitions.</returns>
        public IEnumerable<ApplicationPropertiesEntry> GetEntries() => !System.IO.File.Exists(FullName) ? null : GetEntriesInternally();

        private IEnumerable<ApplicationPropertiesEntry> GetEntriesInternally() {
            var xml = new XmlDocument();
            xml.Load(FullName);
            foreach (XmlNode node in xml.DocumentElement.ChildNodes) {
                if (!node.Name.Equals("Entry")) { continue; }
                var id = node.InnerText;
                var isArray = false;
                var grp = "Default";
                var encrypted = false;
                var defaults = new List<string>();
                var t = ApplicationPropertiesEntry.EntryTypeEnum.Unknown;
                foreach (XmlAttribute attribute in node.Attributes) {
                    if (attribute.Name.Equals("IsArray") && attribute.Value.Equals("True", StringComparison.InvariantCultureIgnoreCase)) {
                        isArray = true;
                    }
                    if (attribute.Name.Equals("Encrypted") && attribute.Value.Equals("True", StringComparison.InvariantCultureIgnoreCase)) {
                        encrypted = true;
                    }
                    if (attribute.Name.Equals("ValueType")) {
                        switch (attribute.Value) {
                            case "Text":
                                t = ApplicationPropertiesEntry.EntryTypeEnum.Text;
                                break;
                            case "Number":
                                t = ApplicationPropertiesEntry.EntryTypeEnum.Number;
                                break;
                            case "Float":
                                t = ApplicationPropertiesEntry.EntryTypeEnum.Float;
                                break;
                            case "Boolean":
                                t = ApplicationPropertiesEntry.EntryTypeEnum.Boolean;
                                break;
                            case "Jdbc":
                                t = ApplicationPropertiesEntry.EntryTypeEnum.Jdbc;
                                break;
                            default:
                                t = ApplicationPropertiesEntry.EntryTypeEnum.Unknown;
                                break;
                        }
                    }
                    if (attribute.Name.Equals("Defaults")) {
                        var items = attribute.Value.Split(',').Where(i => i.Length > 0).Select(i => i.Trim()).ToList();
                        if (items.Count > 0) { defaults = items; }
                    }
                    if (attribute.Name.Equals("Group")) { grp = attribute.Value; }
                }
                var entry = new ApplicationPropertiesEntry(new ApplicationPropertiesFile(FullName, _cfg)) {
                    Comment = string.Empty,
                    Defaults = new System.Collections.ObjectModel.ObservableCollection<string>(defaults),
                    EntryType = t,
                    Identifier = id,
                    Group = grp,
                    IsArray = isArray,
                    IsEncrypted = encrypted,
                    Value = string.Empty
                };
                yield return entry;
            }

        }

    }
}
