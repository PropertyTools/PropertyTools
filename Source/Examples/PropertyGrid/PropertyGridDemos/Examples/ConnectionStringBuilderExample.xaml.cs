// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Reproduces issue #288: PropertyGrid breaks when bound to a class derived from DbConnectionStringBuilder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using System.ComponentModel;
    using System.Data.Common;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for ConnectionStringBuilderExample.
    /// Demonstrates the broken PropertyGrid behaviour when binding to a <see cref="DbConnectionStringBuilder"/>
    /// subclass (issue #288, originally reported against FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder).
    /// </summary>
    public partial class ConnectionStringBuilderExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringBuilderExample" /> class.
        /// </summary>
        public ConnectionStringBuilderExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// View-model for the <see cref="ConnectionStringBuilderExample"/> window.
    /// </summary>
    public class ConnectionStringBuilderExampleViewModel : Observable
    {
        private FakeConnectionStringBuilder builder = new FakeConnectionStringBuilder();

        /// <summary>
        /// Gets the connection-string builder shown in the PropertyGrid.
        /// </summary>
        public FakeConnectionStringBuilder Builder
        {
            get => this.builder;
        }
    }

    /// <summary>
    /// A minimal connection-string builder that reproduces the PropertyGrid bugs reported in issue #288.
    /// It mirrors the structure of <c>FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder</c>:
    /// the class inherits from <see cref="DbConnectionStringBuilder"/> (which implements
    /// <see cref="ICustomTypeDescriptor"/>) and exposes typed properties whose values are stored
    /// as strings in the underlying dictionary.
    ///
    /// Observed bugs when binding to PropertyGrid:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <b>bool properties</b> – checkboxes show an indeterminate (null) state even though no
    ///       property is declared as <c>bool?</c>.  The root cause is that
    ///       <c>DbConnectionStringBuilderDescriptor.GetValue</c> calls <c>TryGetValue</c> on the
    ///       internal dictionary, which returns the raw string (or <c>null</c> when not set).
    ///       WPF's binding engine cannot convert that <c>null</c> / string to <c>bool</c>, so the
    ///       checkbox ends up in an indeterminate state.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <b>enum properties</b> – radio-button groups start with nothing selected; selecting one
    ///       causes all buttons to vanish.  Same root cause: the binding receives <c>null</c> instead
    ///       of an enum value.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <b>byte[] property</b> – the control is visually broken and produces a StringFormat
    ///       exception when clicked.
    ///     </description>
    ///   </item>
    /// </list>
    /// </summary>
    public class FakeConnectionStringBuilder : DbConnectionStringBuilder
    {
        // ---- string properties ----------------------------------------------------------------

        [Category("Security")]
        [DisplayName("User ID")]
        [Description("The user name used to connect to the data source.")]
        [DefaultValue("")]
        public string UserID
        {
            get => this.TryGetValue("user id", out var v) ? (string)v : string.Empty;
            set => this["user id"] = value;
        }

        [Category("Security")]
        [DisplayName("Password")]
        [Description("The password used to connect to the data source.")]
        [PasswordPropertyText(true)]
        [DefaultValue("")]
        public string Password
        {
            get => this.TryGetValue("password", out var v) ? (string)v : string.Empty;
            set => this["password"] = value;
        }

        [Category("Source")]
        [DisplayName("Data Source")]
        [Description("The hostname or IP address of the server.")]
        [DefaultValue("")]
        public string DataSource
        {
            get => this.TryGetValue("data source", out var v) ? (string)v : string.Empty;
            set => this["data source"] = value;
        }

        // ---- int property ---------------------------------------------------------------------

        [Category("Source")]
        [DisplayName("Port")]
        [Description("TCP/IP port number used for the connection.")]
        [DefaultValue(3050)]
        public int Port
        {
            get => this.TryGetValue("port", out var v) && int.TryParse(v?.ToString(), out var i) ? i : 3050;
            set => this["port"] = value;
        }

        // ---- bool properties (reproduce the null-checkbox bug) --------------------------------

        [Category("Pooling")]
        [DisplayName("Pooling")]
        [Description("When true, the connection is drawn from a connection pool.")]
        [DefaultValue(true)]
        public bool Pooling
        {
            get => this.TryGetValue("pooling", out var v) && bool.TryParse(v?.ToString(), out var b) ? b : true;
            set => this["pooling"] = value;
        }

        [Category("Advanced")]
        [DisplayName("No Database Triggers")]
        [Description("Disables database triggers for this connection.")]
        [DefaultValue(false)]
        public bool NoDatabaseTriggers
        {
            get => this.TryGetValue("no db triggers", out var v) && bool.TryParse(v?.ToString(), out var b) && b;
            set => this["no db triggers"] = value;
        }

        [Category("Advanced")]
        [DisplayName("Compression")]
        [Description("Enables or disables wire compression.")]
        [DefaultValue(false)]
        public bool Compression
        {
            get => this.TryGetValue("compress", out var v) && bool.TryParse(v?.ToString(), out var b) && b;
            set => this["compress"] = value;
        }

        // ---- enum property (reproduce the broken radio-button bug) ----------------------------

        [Category("Source")]
        [DisplayName("Server Type")]
        [Description("The type of Firebird server to connect to.")]
        [DefaultValue(FakeServerType.Default)]
        public FakeServerType ServerType
        {
            get
            {
                if (this.TryGetValue("server type", out var v) &&
                    int.TryParse(v?.ToString(), out var i) &&
                    System.Enum.IsDefined(typeof(FakeServerType), i))
                {
                    return (FakeServerType)i;
                }

                return FakeServerType.Default;
            }
            set => this["server type"] = (int)value;
        }

        // ---- byte[] property (reproduce the StringFormat exception) ---------------------------

        [Category("Advanced")]
        [DisplayName("Crypt Key")]
        [Description("Key used for database decryption.")]
        public byte[] CryptKey
        {
            get => this.TryGetValue("crypt key", out var v) ? (byte[])v : System.Array.Empty<byte>();
            set => this["crypt key"] = value;
        }
    }

    /// <summary>
    /// Fake server-type enumeration, standing in for <c>FirebirdSql.Data.FirebirdClient.FbServerType</c>.
    /// </summary>
    public enum FakeServerType
    {
        /// <summary>Default (TCP/IP) server.</summary>
        Default = 0,

        /// <summary>Embedded server (in-process).</summary>
        Embedded = 1,
    }
}
