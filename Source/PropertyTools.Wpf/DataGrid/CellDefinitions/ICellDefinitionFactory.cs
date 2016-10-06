namespace PropertyTools.Wpf
{
    /// <summary>
    /// Defines the cell definition factories.
    /// </summary>
    public interface ICellDefinitionFactory
    {
        /// <summary>
        /// Creates the cell definition for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The cell definition
        /// </returns>
        CellDefinition CreateCellDefinition(
            DataGrid owner,
            CellRef cell,
            object item);
    }
}