# property-editor
Small (windows) GUI for editing spring boot application.properties files. 

It was written to offer a more tech-agnostic approach for editing application.properties files. It tries to do so by showing suitable GUI controls for boolean values, text, arrays, SQL connection strings, numeric values, a set of allowed values and so on. You can also use jasypt to en- and decrypt text values. Entries that are logically bound to each other can be grouped and being presented together. 

These information (type of data, defaults, etc.) can be stored in XML files and then be referenced in your application.properties file. A basic schema definition would look like this:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ApplicationPropertiesSchema>
  <Entry IsArray="false" ValueType="Text" Group="Server">
    spring.profiles.active
  </Entry>
  <Entry IsArray="false" ValueType="Text" Encrypted="True" Group="Certificate">
    server.ssl.key-store-password
  </Entry>
</ApplicationPropertiesSchema>
```
Each line must be described with an <Entry> node whose content is the name of the configuration entry within the application.properties file. Lines that aren't described won't be shown if the GUI is in 'schema mode' (see: Configuration). Each node can have multiple attributes which contain meta data for the property: 

* IsArray: a true/false value which indicates if the property is an array value.
*	ValueType: the type of data that is expected for the property. Controls the presentation within the GUI/editor. Valid types are [ Text, Number, Float, Boolean, Jdbc ].
*	Group: optional value for grouping properties.
*	Encrypted: Indicates if the value has been encrypted with jasypt. The GUI detects this automatically if the encrypted value is encapsulated between ENC([...]).
*	Defaults: a comma-seperated list of valid values for the property. Will be presented as a combobox.

The XML schema definition file(s) must reside within the same directory the executable is stored. The tool searches the application.properties files's comments for a line like this
#SCHEMA: <xml schema file name without extension>
whereas <xml schema [...]> is the file name you saved the XML to. If the file was stored as my-cool-schema.xml the comment line for the application.properties file would be #SCHEMA: my-cool-schema.
Properties that aren't defined in the schema file won't be displayed!

## Configuration
There are three settings that can be made/altered and are accessible via the settings dialog:
*	Jasypt Directory: The directory where the jasypt binaries are found. En- and decryption won't be possible if the setting does not point to the correct directory.
*	Use schema file: Indicates if schema files are used. Otherwise all entries will be shown as text entries.
*	Create backup on save: Makes a backup of the current file before overwriting it. The backup is placed within the same directory the application.properties file resides.
