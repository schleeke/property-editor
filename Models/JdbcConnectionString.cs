using Prism.Mvvm;
using System;
using System.Linq;

namespace ApplicationProperties.Models {

    /// <summary>
    /// Represents a JDBC connection string.
    /// </summary>
    public class JdbcConnectionString : BindableBase {
        private string _srv;
        private string _db;
        private bool _intSec;

        /// <summary>
        /// Initializes a new instance of the <see cref="JdbcConnectionString"/> class.
        /// </summary>
        public JdbcConnectionString() :this(string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JdbcConnectionString"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <exception cref="FormatException">Thrown if the connection string has a bad format.</exception>
        public JdbcConnectionString(string connectionString) {
            if (connectionString.IsNullOrEmpty()) {
                IntegratedSecurity = false;
                SqlServer = "localhost";
                Database = "db";
                return;
            }
            Parse(connectionString);
        }

        /// <summary>
        /// The name of the SQL server (instance).
        /// </summary>
        public string SqlServer { get => _srv; set => SetProperty(ref _srv, value); }

        /// <summary>
        /// The name of the database.
        /// </summary>
        public string Database { get => _db; set => SetProperty(ref _db, value); }

        /// <summary>
        /// Indicates if the connection uses integrated security;
        /// </summary>
        public bool IntegratedSecurity { get => _intSec; set => SetProperty(ref _intSec, value); }

        /// <inheritdoc/>
        public override string ToString() {
            var retVal = $"jdbc:sqlserver://{SqlServer.Replace("\\", "\\\\")};databaseName={Database}";
            if(IntegratedSecurity) { retVal = $"{retVal};integratedSecurity=true"; }
            return retVal;
        }

        private void Parse(string connectionString) {
            if(!connectionString.StartsWith("jdbc:", StringComparison.InvariantCultureIgnoreCase)) { throw new FormatException("The connection string has a bad format."); }
            connectionString = connectionString.Substring("jdbc:".Length);
            if(!connectionString.Contains(";")) { throw new FormatException("The connection string has a bad format."); }
            var parts = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length < 2) { throw new FormatException("The connection string has a bad format."); }
            foreach (var part in parts) {
                if(part.StartsWith("sqlserver://", StringComparison.InvariantCultureIgnoreCase)) {
                    connectionString = part.Substring("sqlserver://".Length);
                    SqlServer = connectionString.Replace("\\\\", "\\");
                }
                else if (part.Contains("=")) {
                    var keyValue = part.Split(new char[] { '=' }, 2).Select(val => val.Trim()).ToArray();
                    switch (keyValue[0].ToLower()) {
                        case "databasename":
                            Database = keyValue[1];
                            break;
                        case "integratedsecurity":
                            IntegratedSecurity = keyValue[1].Equals("True", StringComparison.InvariantCultureIgnoreCase);
                            break;
                    }
                }
            }
            if(SqlServer.IsNullOrEmpty() || Database.IsNullOrEmpty()) { throw new FormatException("The connection string has a bad format."); }
        }
    }
}
