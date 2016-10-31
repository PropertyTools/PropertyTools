// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridCommands.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a standard set of DataGrid related commands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows.Input;

    /// <summary>
    /// Provides a standard set of <see cref="DataGrid" /> related commands.
    /// </summary>
    public static class DataGridCommands
    {
        /// <summary>
        /// Initializes static members of the <see cref="DataGridCommands"/> class.
        /// </summary>
        static DataGridCommands()
        {
            InsertRows = new RoutedCommand("InsertRows", typeof(DataGrid));
            DeleteRows = new RoutedCommand("DeleteRows", typeof(DataGrid));
            InsertColumns = new RoutedCommand("InsertColumns", typeof(DataGrid));
            DeleteColumns = new RoutedCommand("DeleteColumns", typeof(DataGrid));
            SortAscending = new RoutedCommand("SortAscending", typeof(DataGrid));
            SortDescending = new RoutedCommand("SortDescending", typeof(DataGrid));
            ClearSort = new RoutedCommand("ClearSort", typeof(DataGrid));
        }

        /// <summary>
        /// Gets the delete columns command.
        /// </summary>
        /// <value>The delete columns command.</value>
        public static ICommand DeleteColumns { get; }

        /// <summary>
        /// Gets the delete rows command.
        /// </summary>
        /// <value>The delete rows command.</value>
        public static ICommand DeleteRows { get; }

        /// <summary>
        /// Gets the insert columns command.
        /// </summary>
        /// <value>The insert columns command.</value>
        public static ICommand InsertColumns { get; }

        /// <summary>
        /// Gets the insert rows command.
        /// </summary>
        /// <value>The insert rows command.</value>
        public static ICommand InsertRows { get; }

        /// <summary>
        /// Gets the sort ascending command.
        /// </summary>
        /// <value>The sort ascending command.</value>
        public static ICommand SortAscending { get; }

        /// <summary>
        /// Gets the sort descending command.
        /// </summary>
        /// <value>The sort descending command.</value>
        public static ICommand SortDescending { get; }

        /// <summary>
        /// Gets the clear sort command.
        /// </summary>
        /// <value>The clear sort command.</value>
        public static ICommand ClearSort { get; }
    }
}