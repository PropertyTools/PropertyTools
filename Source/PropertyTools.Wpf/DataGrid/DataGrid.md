# DataGrid

DataGrid grid layout (all the background and selection elements are not included here)

```
+-PART_Grid ---------------------------------------------------------------+
| +- PART_TopLeft --------------------+- PART_ColumnScrollViewer --------+ |
| |                                   | +- PART_ColumnGrid ------------+ | |
| |                                   | |                              | | |
| |                                   | +------------------------------+ | |
| +- PART_RowScrollViewer ------------+- PART_SheetScrollViewer ---------+ |
| | +-PART_RowGrid -----------------+ | +- PART_SheetGrid -------------+ | |
| | |                               | | |                              | | |
| | +-------------------------------+ | +------------------------------+ | |
| +-----------------------------------+----------------------------------+ |
+--------------------------------------------------------------------------+
```

- PART_SheetGrid is where all the cells are added.
- PART_RowGrid contains the row headers
- PART_ColumnGrid contains the column headers
- The PART_SheetScrollViewer has auto scrollbars, the row and column scrollviewers have hidden scrollbars.
- The column and row definitions of the ColumnGrid and RowGrid are synchronized with the respective SheetGrid definitions.

## DataGrid Lifetime events

- OnApplyTemplate
  - UpdateGridContent
- SizeChanged
  - UpdateGridSize (doing nothing size the control is not loaded!)
- ItemsSourceChanged
  - UpdateGridContent
    - UpdateRows
    - UpdateColumns
    - UpdateCells
    - Dispatcher.Invoke(UpdateGridSize)
- Loaded
  - UpdateGridSize
- Dispatcher 
  - UpdateGridSize
    - calls UpdateLayout on child elements


https://docs.microsoft.com/en-us/dotnet/framework/wpf/app-development/wpf-windows-overview?redirectedfrom=MSDN
