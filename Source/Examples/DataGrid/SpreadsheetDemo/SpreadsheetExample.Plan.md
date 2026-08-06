# SpreadsheetExample — data model and UI implementation plan

Status: historical design document (phases 0-4 shipped; see §8)
Scope, as originally written for phases 0-4: `Source/Examples/DataGrid/DataGridDemo/Examples/SpreadsheetExample.*`
and a new `Examples/Spreadsheet/` folder (model + UI) inside the same `DataGridDemo` project — no new
`.csproj` or solution entries (see §4's "no new projects" reasoning, which was correct for that request).

**Later superseded**: the example was subsequently extracted into its own standalone WPF application
project, `Source/Examples/DataGrid/SpreadsheetDemo/SpreadsheetDemo.csproj` (namespace `SpreadsheetDemo`
instead of `DataGridDemo`), still discoverable from `DemoLauncher` via the same `*Example`-suffixed
`Window`-subclass reflection it always used — no launcher code changes were needed. The rest of this
document is left as-written, describing the reasoning and design at each phase; path/namespace
references below to `DataGridDemo` describe where things lived *at the time*, not the current layout.

---

## 1. Goal

Replace the placeholder model behind `SpreadsheetExample` with a proper, best-practice .NET
spreadsheet data model:

* strongly typed cell values — text, number, boolean, date/time, error, empty;
* formulas with dependency tracking and incremental recalculation;
* every cell editable (in the grid and from a formula bar);
* per-cell formatting (alignment, bold);
* file operations (new / open / save), find / replace;
* efficient: sparse storage, no boxing in the value path, no full-sheet recalculation on every edit;
* fully unit-testable — the model has **no WPF dependency**.

And in the WPF UI:

* a menu bar: **File** (New, Open, Save), **Edit** (Find, Replace), **Format** (Alignment, Bold);
* below it, a formula bar: the current cell reference (`A1`) and an edit box bound to the
  current cell's content.

---

## 2. Current state

`SpreadsheetExample.xaml.cs` currently contains a throw-away model (~300 lines, all in one file):

| Problem | Detail |
| --- | --- |
| `Cell` has no data | `ToString()` returns the literal `"OK"`; there is no value, no type, no formula. |
| `CellReference` is a mutable struct without `IEquatable<T>` | Used as a `Dictionary` key → boxing + reflection-based `Equals` on every lookup. |
| `GetCell` always materialises and stores a cell | Reading the sheet mutates it; empty cells are never reclaimed. |
| `RowAccessor` is allocated per `[]` access | Row identity is unstable; `IEnumerator` throws `NotImplementedException`. |
| No change notification | Nothing implements `INotifyPropertyChanged`, so recalculated values would never reach the UI. |
| Model and view are in one file | Not unit-testable. |
| Column-name generation is hand-rolled | `PropertyTools.Wpf.CellRef.ToColumnName` already does this. |

The XAML is a bare `p:DataGrid` with `ItemsSource` / `RowHeadersSource` / `ColumnHeadersSource`.

---

## 3. Constraints imposed by `PropertyTools.Wpf.DataGrid`

These were verified against the library source and drive several design decisions.

1. **Items source shape.** `DataGrid.CreateOperator()` (`DataGrid.cs:1987`) picks
   `ListListOperator` when `TypeHelper.IsIListIList(list)` is true. So the sheet must be exposed to
   the grid as `IList` of `IList`. The model itself should *not* implement these interfaces — a thin
   adapter does.
2. **Binding path.** `ListListOperator.GetBindingPath` returns `"[row][column]"` and
   `GetDataContext` returns the items source (`ListListOperator.cs:288-306`). Every cell control is
   therefore bound *relative to the sheet adapter*, with the sheet adapter as `DataContext`.
3. **Cell element creation.** `DataGrid.CreateDisplayControl` / `CreateEditControl`
   (`DataGrid.cs:2600-2641`) ask `ICellDefinitionFactory` for a `CellDefinition` and
   `IDataGridControlFactory` for the element, then set `element.DataContext = cd.BindingSource` and
   subscribe to `SourceUpdated`. Both factories are dependency properties
   (`DataGrid.cs:201`, `DataGrid.cs:210`) → **customisable without touching the library**.
4. **The edit control must *be* a `TextBox`.** `ShowEditControl` (`DataGrid.cs:2663`) and
   `TextEditorPreviewKeyDown` (`DataGrid.cs:3620-3660`) special-case `currentEditControl is TextBox`
   for type-to-edit, Enter (commit via `RemoveEditControl` → `LostFocus` → source update) and
   Escape (`BindingOperations.ClearBinding` → cancel). A wrapped/templated editor loses this.
   Display controls may be wrapped in a container.
5. **Value round-trip for copy / paste / clear / auto-fill.**
   * Copy uses `GetCellValue` → `FormatCellString` → `value.ToString()`.
   * Paste and multi-cell edit use `DataGridOperator.TrySetCellValue` (`DataGridOperator.cs:469`),
     which calls `GetPropertyType(cell)` (falls back to `currentValue.GetType()`) and then
     `TryConvert` (`DataGridOperator.cs:706`). `TryConvert` ends up at
     `TypeDescriptor.GetConverter(targetType)` → **a `TypeConverter` on `Cell` makes paste work**.
   * Delete/Clear calls `TrySetCellValue(cell, null)` for reference types
     (`DataGrid.cs:2057`, `DataGrid.cs:2071`) → the adapter's indexer setter must treat `null` as
     "clear contents".
   * Multi-cell edit propagation passes the *source `Cell` instance* to the other cells, so the
     setter must **copy content, never store the incoming instance**.
6. **`CurrentCell` is a two-way-bindable DP** (`DataGrid.cs:219`, `BindsTwoWayByDefault`) of type
   `CellRef`, which already renders as `"A1"` via `ToString()` and parses from `"A1"` via
   `CellRefConverter`. The formula bar binds straight to it.
7. **No UI virtualisation.** The grid materialises an element per cell, so the demo keeps a bounded
   sheet size (default 100 × 26, max ~1000 × 100).

---

## 4. Architecture

```
┌───────────────────── DataGridDemo (single WinExe project) ──────────────────────┐
│  Examples/Spreadsheet/                                                          │
│  ├─ SpreadsheetExample.xaml         menu bar + formula bar + p:DataGrid         │
│  │  SpreadsheetViewModel            commands, current cell, dirty/file state    │
│  │  SheetGridAdapter / SheetRowAdapter   IList<IList> bridge for the DataGrid   │
│  │  SpreadsheetControlFactory       display TextBlock + edit TextBox per cell   │
│  │  Converters, Find/Replace dialog                    (WPF, namespace ...Ui)   │
│  │                                  ┄┄┄┄┄┄┄┄┄┄ no WPF types cross this line ┄┄┄┄ │
│  └─ Model/                          Workbook ─ Sheet ─ Cell, sparse storage     │
│       CellValue, CellAddress, CellRange, CellStyle, CellContent  (value types)  │
│       Formulas/  Lexer → Parser → AST → Evaluator → FunctionRegistry            │
│       Calculation/  DependencyGraph + RecalculationEngine                       │
│       CellInputParser / CellFormatter   text ⇄ typed value                      │
│       Commands/ UndoStack, Serialization/ ISheetSerializer (JSON, CSV), Search/ │
└───────────────────────────────────────────────────────────────────────────────┘
```

**Project layout decision.** No new projects. The model lives inside the existing demo project, as
plain C# with no WPF types anywhere in its namespace:
`Source/Examples/DataGrid/DataGridDemo/Examples/Spreadsheet/Model/`, namespace
`DataGridDemo.Spreadsheet.Model`. It has no reference to `System.Windows.*`, so it stays testable
and could be lifted into its own library later without rewrites — the folder boundary is the
enforced seam, not a project boundary.

For unit tests, add a `ProjectReference` from `PropertyTools.Wpf.Tests` to
`DataGridDemo.csproj`. `DataGridDemo` is `OutputType=WinExe`, which a normal test project can
reference without issue (it just pulls in the compiled types); the test project itself stays
`net10.0-windows` and does not need `UseWPF`. Model tests live under
`PropertyTools.Wpf.Tests/Spreadsheet/` alongside the existing fixtures.

---

## 5. Data model

### 5.1 `CellValue` — the typed value (readonly struct, no boxing)

```csharp
public enum CellValueType { Empty, Number, Text, Boolean, DateTime, Error }

public enum CellError { None, DivideByZero, Value, Reference, Name, Number, Circular, NotAvailable }

public readonly struct CellValue : IEquatable<CellValue>
{
    private readonly double number;   // number | OADate | 0/1 for bool | (int)CellError
    private readonly string text;     // text payload, otherwise null
    private readonly CellValueType type;

    public static readonly CellValue Empty;

    public static CellValue FromNumber(double value);
    public static CellValue FromText(string value);      // null/"" → Empty
    public static CellValue FromBoolean(bool value);
    public static CellValue FromDateTime(DateTime value);
    public static CellValue FromError(CellError error);

    public CellValueType Type { get; }
    public bool IsEmpty  { get; }
    public bool IsError  { get; }
    public CellError Error { get; }

    public double   AsNumber();          // throws/returns error for non-numeric
    public string   AsText();
    public bool     AsBoolean();
    public DateTime AsDateTime();

    public bool TryGetNumber(out double value);   // Excel coercion: bool→0/1, date→OADate,
    public bool TryGetText(out string value);     // numeric text→number
    public object ToObject();                     // boxing only at the boundary (clipboard, JSON)
}
```

*Why a discriminated struct:* one value per cell, 24 bytes, zero allocation for numbers/dates/bools
— an `object Value` property would box every number. `IEquatable` is used by the recalculation
engine to suppress notifications when a recalculated value did not change.

### 5.2 `CellAddress` and `CellRange`

```csharp
public readonly struct CellAddress : IEquatable<CellAddress>, IComparable<CellAddress>
{
    public CellAddress(int row, int column);
    public int Row { get; }            // 0-based
    public int Column { get; }         // 0-based
    public override int GetHashCode() => (this.column << 20) ^ this.row;
    public override string ToString(); // "A1", "AA10"
    public static bool TryParse(string s, out CellAddress address);       // "A1", "$A$1"
    public static string ToColumnName(int column);                        // A..Z, AA..
    public static int ParseColumnName(string name);
}

public readonly struct CellRange : IEquatable<CellRange>, IEnumerable<CellAddress>
{
    public CellAddress TopLeft { get; }
    public CellAddress BottomRight { get; }
    public int RowCount { get; }
    public int ColumnCount { get; }
    public bool Contains(CellAddress a);
    public bool Intersects(CellRange other);
    public static bool TryParse(string s, out CellRange range);           // "A1:B10"
}
```

`IEquatable` + explicit `GetHashCode` removes the boxing/reflection cost of the current
`CellReference`. `CellRange` is `IEnumerable<CellAddress>` for the evaluator, `Intersects` for the
dependency graph. Conversion helpers to/from `PropertyTools.Wpf.CellRef` live in the **UI** layer
(`CellRefExtensions`) so the model stays WPF-free.

### 5.3 `CellStyle` — immutable, interned (flyweight)

```csharp
public enum CellHorizontalAlignment { General, Left, Center, Right }

public sealed class CellStyle : IEquatable<CellStyle>
{
    public static readonly CellStyle Default;
    public CellHorizontalAlignment HorizontalAlignment { get; }
    public bool Bold { get; }
    public bool Italic { get; }
    public string FormatString { get; }   // null = general

    public CellStyle WithBold(bool bold);            // returns interned instance
    public CellStyle WithAlignment(CellHorizontalAlignment a);
    public CellStyle WithFormat(string format);
}
```

Styles are immutable and interned in a `ConcurrentDictionary<CellStyle, CellStyle>` pool
(same idea as Excel's shared XF records): 2 600 bold cells share one `CellStyle` instance.
`General` alignment resolves at render time — numbers/dates right, booleans centre, text left.

### 5.4 `CellContent` — what the user typed

```csharp
public readonly struct CellContent
{
    public static readonly CellContent Empty;
    public static CellContent FromValue(CellValue value);
    public static CellContent FromFormula(Formula formula);

    public bool IsFormula { get; }
    public Formula Formula { get; }   // parsed AST + source text, null when literal
    public CellValue Value { get; }   // literal value, Empty when formula
}
```

### 5.5 `Cell` — the bindable per-cell object

```csharp
public sealed class Cell : INotifyPropertyChanged
{
    internal Cell(Sheet sheet, CellAddress address);

    public Sheet Sheet { get; }
    public CellAddress Address { get; }

    public CellContent Content { get; internal set; }   // raises Text/DisplayText changes
    public CellValue Value { get; internal set; }       // computed; == literal for non-formulas
    public CellStyle Style { get; internal set; }

    /// <summary>Round-trippable edit string: "=A1+2", "3.14", "Hello". Setting it edits the sheet.</summary>
    public string Text
    {
        get => CellFormatter.ToEditText(this.Content, CultureInfo.CurrentCulture);
        set => this.Sheet.SetCellText(this.Address, value);   // parse → undo entry → recalc
    }

    /// <summary>Formatted text shown in the grid (applies Style.FormatString, error text).</summary>
    public string DisplayText { get; }

    public bool IsEmpty { get; }
    public event PropertyChangedEventHandler PropertyChanged;
}
```

`Cell` is the WPF binding target, so it must be a class with stable identity for its lifetime and
raise `PropertyChanged` for `Value`, `Text`, `DisplayText` and `Style`. Setting `Text` routes back
through the sheet so that parsing, dependency update, recalculation and undo all happen in one
place — the grid editor and the formula bar then share exactly one code path.

*Materialisation policy:* `Sheet.TryGetCell` (model/engine path) never allocates; `Sheet.GetCell`
(UI path) creates-and-caches. The grid asks for every visible cell, so ~2 600 small objects for the
default sheet — acceptable and documented; empty cells hold `CellContent.Empty` and the shared
default style.

### 5.6 `Sheet` and `Workbook`

```csharp
public sealed class Sheet : INotifyPropertyChanged
{
    public Sheet(string name, int rowCount = 100, int columnCount = 26);

    public string Name { get; set; }
    public int RowCount { get; private set; }
    public int ColumnCount { get; private set; }
    public CellRange UsedRange { get; }

    public Cell GetCell(CellAddress address);              // materialises
    public bool TryGetCell(CellAddress address, out Cell cell);
    public CellValue GetValue(CellAddress address);        // Empty for missing cells

    public void SetCellText(CellAddress address, string input);        // parse + recalc + undo
    public void SetContent(CellAddress address, CellContent content);
    public void SetStyle(CellRange range, Func<CellStyle, CellStyle> transform);
    public void ClearContents(CellRange range);
    public void ClearAll(CellRange range);

    public void InsertRows(int index, int count);          // + reference fix-up (phase 5)
    public void DeleteRows(int index, int count);
    public void InsertColumns(int index, int count);
    public void DeleteColumns(int index, int count);

    public IDisposable DeferRecalculation();               // bulk edit / paste / load
    public CalculationMode CalculationMode { get; set; }   // Automatic | Manual
    public void Recalculate();

    public event EventHandler<CellChangedEventArgs> CellChanged;      // value/content/style
    public event EventHandler<SheetStructureChangedEventArgs> StructureChanged;
}

public sealed class Workbook : INotifyPropertyChanged
{
    public ObservableCollection<Sheet> Sheets { get; }
    public Sheet ActiveSheet { get; set; }
    public string FilePath { get; set; }
    public bool IsModified { get; }        // set by any mutation, cleared on save/load
    public UndoStack UndoStack { get; }
}
```

Storage: `Dictionary<CellAddress, Cell> cells` (sparse). Row/column counts are logical bounds used
by the grid; the used range is tracked incrementally.

### 5.7 Formula engine

Pipeline: **text → lexer → parser → AST → evaluator → `CellValue`**, with the AST cached on the
cell (parsed once per edit, never per recalculation).

```
Formulas/
  Formula.cs               // { string Text; FormulaNode Root; IReadOnlyList<CellAddress> Precedents;
                           //   IReadOnlyList<CellRange> RangePrecedents }
  FormulaLexer.cs          // numbers, strings, booleans, refs, ranges, operators, functions
  FormulaParser.cs         // recursive descent + precedence climbing; FormulaSyntaxException
  Nodes/                   // LiteralNode, ReferenceNode, RangeNode, UnaryOperatorNode,
                           // BinaryOperatorNode, FunctionNode  (immutable, IFormulaNodeVisitor)
  FormulaEvaluator.cs      // visitor → CellValue, Excel coercion + error propagation
  IEvaluationContext.cs    // GetValue(CellAddress), EnumerateRange(CellRange)
  Functions/
    IFunction.cs           // Name, MinArgs, MaxArgs, Invoke(FunctionArguments)
    FunctionRegistry.cs    // case-insensitive dictionary, user-extensible
    MathFunctions.cs LogicalFunctions.cs TextFunctions.cs DateFunctions.cs
    StatisticalFunctions.cs InformationFunctions.cs
```

Operator precedence (lowest → highest): comparison `= <> < > <= >=` · concat `&` · `+ -` ·
`* /` · unary `- +` · `^` · postfix `%` · range `:`.

Correction from an earlier draft of this line: unary `- +` is lower precedence than `^`, not
higher — `^`'s right operand still allows a leading unary sign so `2^-2` parses. This matches
Excel's actual (if inconsistently documented) behaviour and standard mathematical convention:
`-2^2` evaluates to `-4`, not `4`. See `FormulaParser`'s remarks for the exact grammar.

Built-in functions for v1: `SUM AVERAGE MIN MAX COUNT COUNTA` · `IF AND OR NOT IFERROR` ·
`ABS ROUND ROUNDUP ROUNDDOWN SQRT POWER MOD INT` · `CONCAT LEN LEFT RIGHT MID UPPER LOWER TRIM` ·
`TODAY NOW DATE YEAR MONTH DAY` · `ISBLANK ISNUMBER ISTEXT ISERROR`.
Registering a custom function is one call: `sheet.Functions.Register(new MyFunction())`.

Semantics: errors propagate (any error argument → that error); text that looks numeric coerces in
arithmetic, otherwise `#VALUE!`; empty cells are `0` in arithmetic and `""` in concatenation;
division by zero → `#DIV/0!`; unknown function/name → `#NAME?`; out-of-range reference → `#REF!`.
Argument separator is `,` and the decimal separator inside formulas is `.` (invariant) — user
*input of literals* stays culture-aware. This split is deliberate and documented.

### 5.8 Dependency graph and recalculation

```csharp
internal sealed class DependencyGraph
{
    public void SetPrecedents(CellAddress cell, IReadOnlyList<CellAddress> precedents,
                              IReadOnlyList<CellRange> ranges);
    public void Remove(CellAddress cell);
    public IEnumerable<CellAddress> GetDirectDependents(CellAddress changed);
}

internal sealed class RecalculationEngine
{
    public void MarkDirty(CellAddress address);
    public void Recalculate();      // topological order over the dirty sub-graph
}
```

* `dependents: Dictionary<CellAddress, HashSet<CellAddress>>` for direct cell references, plus a
  small list of `(CellRange, CellAddress)` pairs for range references, matched by `Intersects`.
  (Optimisation noted for later: bucket ranges by row band to avoid a linear scan; irrelevant at
  demo scale.)
* An edit marks its cell dirty, then transitively its dependents.
* Recalculation is an iterative DFS with a `visiting` set → a cycle yields `CellError.Circular`
  for every cell on the cycle instead of a stack overflow.
* Only cells whose recomputed `CellValue` differs (`IEquatable`) raise `PropertyChanged`.
* `DeferRecalculation()` returns a scope; recalculation runs once on dispose — used by paste,
  `ReplaceAll`, undo/redo and file load.

### 5.9 Input parsing and formatting

```csharp
public static class CellInputParser
{
    public static CellContent Parse(string input, IFormulaParser parser, CultureInfo culture);
}
public static class CellFormatter
{
    public static string ToEditText(CellContent content, CultureInfo culture);
    public static string ToDisplayText(CellValue value, CellStyle style, CultureInfo culture);
}
```

Rules, in order: `null`/empty → `Empty`; leading `'` → forced text; leading `=` → formula
(syntax error → `#NAME?`-style error value, but the typed text is preserved so the user can fix
it); `TRUE`/`FALSE` → boolean; trailing `%` + number → number/100 with percent format;
`double.TryParse` (current culture) → number; `DateTime.TryParse` (current culture) → date;
otherwise text. Round-trip guarantee: `Parse(ToEditText(c)) == c`, covered by tests.

### 5.10 Undo / redo

```csharp
public interface ISheetCommand { string Description { get; } void Execute(); void Undo(); }
public sealed class UndoStack       // Push/Undo/Redo/Clear, CanUndo/CanRedo, capacity limit
```

Commands: `SetCellContentCommand` (single), `SetRangeContentCommand` (paste / fill / clear /
replace-all — stores old + new `CellContent[]`), `SetRangeStyleCommand`, `InsertRowsCommand` etc.
All sheet mutations funnel through `Sheet.ApplyCommand`, so undo can never diverge from the model.
Wired to `ApplicationCommands.Undo/Redo` (Ctrl+Z / Ctrl+Y).

### 5.11 Persistence

```csharp
public interface ISheetSerializer { string Extension { get; } string FilterText { get; }
                                    void Save(Workbook wb, Stream s); Workbook Load(Stream s); }
```

* `JsonSheetSerializer` (`System.Text.Json`) — native format `.ptsheet`: workbook → sheets →
  `{ "a": "B3", "t": "=SUM(A1:A2)", "s": 4 }` entries plus a style table. Stores **raw input text**
  (not computed values) and recalculates on load; invariant culture on disk.
* `CsvSheetSerializer` — import/export of display values (lossy, no formulas/styles), used for
  interop.
* `.xlsx` is explicitly **out of scope** (would pull in an OpenXML dependency into an example).
* `Workbook.IsModified` drives the "Save changes?" prompt on New/Open/close.

### 5.12 Find / replace

```csharp
public sealed class FindOptions
{
    public string SearchText { get; set; }
    public bool MatchCase { get; set; }
    public bool MatchEntireCell { get; set; }
    public SearchTarget Target { get; set; }    // Values | Formulas
    public CellRange? Scope { get; set; }       // null = whole sheet
}
public static class FindService
{
    public static IEnumerable<CellAddress> Find(Sheet sheet, FindOptions o, CellAddress start);
    public static int ReplaceAll(Sheet sheet, FindOptions o, string replacement);   // one undo entry
}
```

Search order is row-major from the cell after `start`, wrapping — so "Find Next" behaves as
expected.

---

## 6. WPF integration

### 6.1 Grid adapter

```csharp
// DataGridDemo/Examples/Spreadsheet/SheetGridAdapter.cs
public sealed class SheetGridAdapter : IList<SheetRowAdapter>, IList, INotifyCollectionChanged
public sealed class SheetRowAdapter  : IList<Cell>, IList
```

* `SheetGridAdapter` caches one `SheetRowAdapter` per row (stable identity) and rebuilds + raises
  `Reset` on `Sheet.StructureChanged`.
* `SheetRowAdapter[column]` getter → `sheet.GetCell(...)`; setter handles:
  * `null` → `sheet.ClearContents(address)` (Delete key / Clear),
  * `Cell` → copy **content and style**, never the instance (multi-cell edit propagation),
  * `string` / `CellValue` / other → `sheet.SetCellText(address, value.ToString())` (paste).
* `IsFixedSize` / `IsReadOnly` report the truth so `CanInsert`/`CanDelete` behave.
* `[TypeConverter(typeof(CellConverter))]` on `Cell`, converting from `string` to a detached
  `Cell`-shaped carrier — this is what makes `DataGridOperator.TryConvert` succeed on paste
  (see §3.5). `Cell.ToString()` returns `DisplayText`, which is what Copy writes to the clipboard.

### 6.2 Cell display and edit controls

```csharp
public class SpreadsheetControlFactory : DataGridControlFactory
{
    protected override FrameworkElement CreateDisplayControlOverride(CellDefinition d)
    {
        // Border (DataContext = sheet adapter, set by the DataGrid)
        //  └ TextBlock, DataContext bound OneWay to "[r][c]"  → the Cell
        //      Text                ← {Binding DisplayText}
        //      FontWeight          ← {Binding Style.Bold, Converter=BoolToFontWeightConverter}
        //      FontStyle           ← {Binding Style.Italic, ...}
        //      HorizontalAlignment ← MultiBinding {Style.HorizontalAlignment, Value.Type}
        //                             → CellAlignmentConverter (implements "General")
        //      Foreground          ← {Binding Value.IsError, Converter=ErrorBrushConverter}
    }

    protected override FrameworkElement CreateEditControlOverride(CellDefinition d)
    {
        // A bare TextBox (required, see §3.4) with binding path d.BindingPath + ".Text",
        // Mode=TwoWay, UpdateSourceTrigger=LostFocus, NotifyOnSourceUpdated=true.
    }
}
```

`NotifyOnSourceUpdated` must stay `true` — `DataGrid.CreateEditControl` relies on `SourceUpdated`
for multi-cell edit propagation. Editing therefore writes the *raw text* (`=A1+B1`) into
`Cell.Text`, i.e. through `Sheet.SetCellText`.

### 6.3 Window layout

```xml
<DockPanel>
  <Menu DockPanel.Dock="Top"> … </Menu>
  <Grid DockPanel.Dock="Top">           <!-- formula bar -->
    <TextBox x:Name="NameBox" Width="80"
             Text="{Binding CurrentCellReference, UpdateSourceTrigger=LostFocus}"/>
    <TextBox x:Name="FormulaBox" Grid.Column="2"
             Text="{Binding CurrentCellText, UpdateSourceTrigger=LostFocus}">
      <TextBox.InputBindings>
        <KeyBinding Key="Enter"  Command="{Binding CommitFormulaCommand}"/>
        <KeyBinding Key="Escape" Command="{Binding CancelFormulaCommand}"/>
      </TextBox.InputBindings>
    </TextBox>
  </Grid>
  <p:DataGrid x:Name="Grid"
              ItemsSource="{Binding Rows}"
              CurrentCell="{Binding CurrentCell, Mode=TwoWay}"
              RowHeadersSource="{Binding RowHeaders}"
              ColumnHeadersSource="{Binding ColumnHeaders}"
              ControlFactory="{Binding ControlFactory}"/>
</DockPanel>
```

The name box is editable and navigates (`A1`, `B12`) — `CellRefConverter` already parses that
syntax; invalid input reverts.

### 6.4 Menus and commands

| Menu | Item | Command | Gesture |
| --- | --- | --- | --- |
| File | New | `ApplicationCommands.New` | Ctrl+N |
| File | Open… | `ApplicationCommands.Open` | Ctrl+O |
| File | Save | `ApplicationCommands.Save` | Ctrl+S |
| File | *(Save As…, Exit — optional extras)* | `ApplicationCommands.SaveAs` | Ctrl+Shift+S |
| Edit | Find… | `ApplicationCommands.Find` | Ctrl+F |
| Edit | Replace… | `ApplicationCommands.Replace` | Ctrl+H |
| Edit | *(Undo, Redo, Cut, Copy, Paste — optional extras)* | `ApplicationCommands.*` | Ctrl+Z/Y/X/C/V |
| Format | Align Left / Center / Right | `EditingCommands.AlignLeft` / `AlignCenter` / `AlignRight` | Ctrl+L / E / R |
| Format | Bold | `EditingCommands.ToggleBold` | Ctrl+B |

Using the built-in `ApplicationCommands` / `EditingCommands` gives the keyboard gestures and menu
text for free and does not collide with the `DataGrid`'s own bindings (Copy/Paste/Delete, added in
`DataGrid.cs:1300-1306`). Bindings are declared on the window with
`PropertyTools.Wpf.DelegateCommandBinding` (already in the library) or plain `CommandBinding`s
delegating to the view model. Bold and the alignment items are `IsCheckable` and reflect the
current selection's style.

### 6.5 View model

```csharp
public sealed class SpreadsheetViewModel : Observable   // INotifyPropertyChanged
{
    public Workbook Workbook { get; }
    public Sheet Sheet { get; }
    public SheetGridAdapter Rows { get; }
    public IList<string> RowHeaders { get; }
    public IList<string> ColumnHeaders { get; }
    public IDataGridControlFactory ControlFactory { get; }

    public CellRef CurrentCell { get; set; }        // two-way with the DataGrid
    public string CurrentCellReference { get; set; } // "A1"; setting navigates
    public string CurrentCellText { get; set; }      // formula bar; commit → Sheet.SetCellText
    public CellRange Selection { get; set; }         // from DataGrid.CurrentCell/SelectionCell

    public string Title { get; }                     // "Book1* — Spreadsheet"
    // Command handlers: New, Open, Save, SaveAs, Find, Replace, ToggleBold, SetAlignment
}
```

`CurrentCell` changes → recompute `CurrentCellReference` (`CellRef.ToString()`) and
`CurrentCellText` (`Cell.Text`); the view model subscribes to the current `Cell`'s
`PropertyChanged` so a recalculation refreshes the formula bar, and unsubscribes when the current
cell moves.

---

## 7. File inventory

No new `.csproj` and no new solution entries — everything lands inside the existing
`DataGridDemo` project.

**New — model** (`DataGridDemo/Examples/Spreadsheet/Model/`, namespace `DataGridDemo.Spreadsheet.Model`, no WPF references)

```
CellValue.cs  CellValueType.cs  CellError.cs
CellAddress.cs  CellRange.cs
CellStyle.cs  CellHorizontalAlignment.cs  CellStylePool.cs
CellContent.cs  Cell.cs
Sheet.cs  Workbook.cs  CellChangedEventArgs.cs  SheetStructureChangedEventArgs.cs
CellInputParser.cs  CellFormatter.cs
Formulas/…            (lexer, parser, nodes, evaluator, functions — §5.7)
Calculation/DependencyGraph.cs  Calculation/RecalculationEngine.cs
Commands/ISheetCommand.cs  Commands/UndoStack.cs  Commands/*Command.cs
Serialization/ISheetSerializer.cs  JsonSheetSerializer.cs  CsvSheetSerializer.cs
Search/FindOptions.cs  Search/FindService.cs
```

**New — tests** (`PropertyTools.Wpf.Tests/Spreadsheet/`) — see §9. Requires adding a
`ProjectReference` from `PropertyTools.Wpf.Tests.csproj` to `DataGridDemo.csproj`.

**New — demo UI** (`DataGridDemo/Examples/Spreadsheet/`, namespace `DataGridDemo.Spreadsheet`)

```
SheetGridAdapter.cs  SheetRowAdapter.cs  CellConverter.cs  CellRefExtensions.cs
SpreadsheetControlFactory.cs
Converters/BoolToFontWeightConverter.cs  Converters/CellAlignmentConverter.cs
SpreadsheetViewModel.cs
FindReplaceDialog.xaml(.cs)
```

**Modified**

```
SpreadsheetExample.xaml            menu bar, formula bar, DataGrid wiring
SpreadsheetExample.xaml.cs         code-behind reduced to InitializeComponent + command bindings
PropertyTools.Wpf.Tests.csproj     add ProjectReference to DataGridDemo.csproj
README.md                          mention the spreadsheet example (optional)
```

CHANGELOG.md is intentionally left untouched by this plan document itself; the entry belongs to
the implementation PR (see §11.6).

No changes to `PropertyTools.Wpf` are required.

---

## 8. Phased delivery

Each phase compiles, is testable and leaves the demo runnable.

| # | Phase | Contents | Status |
| --- | --- | --- | --- |
| 0 | Skeleton | Model/ folder inside `DataGridDemo`, copyright headers (see §4 — no new projects, unlike the original draft) | ✅ Done |
| 1 | Value core | `CellValue`, `CellAddress`, `CellRange`, `CellStyle`, `CellContent`, `CellInputParser`, `CellFormatter` | ✅ Done — 162 tests, run for real via a scratch WPF-free NUnit project (see §11.8) |
| 2 | Sheet + grid | `Cell`, `Sheet`, `Workbook`, `SheetGridAdapter`, `SpreadsheetControlFactory`, `CellConverter`; XAML wired | ✅ Done — typing text/numbers/dates/bools, copy/paste/Delete/multi-cell fill all work; found and fixed a `ListListOperator` column-insert/delete bug along the way (§11.3→`SpreadsheetDataGrid`) |
| 3 | Formulas | Lexer, parser, AST, evaluator, function registry, dependency graph, recalculation | ✅ Done — 292 tests; unary-minus-vs-`^` precedence corrected from this doc's original (§5.7) to match Excel |
| 4 | UI shell + files & search | Menu bar (File/Edit/Format), formula bar + name box, mutable view model, JSON save/open, Find/Replace dialog | ✅ Done, folded phases 4+5 together — 302 tests. **Dropped**: the undo stack (§5.10) — not requested by the user, cut for scope. **Deferred to phase 6**: CSV export. |
| 6 | Polish (optional) | Row/column insert/delete with formula reference fix-up, number-format menu, `IsModified` in title, CSV export, docs | Not started — stretch goals, not required for the original request |

Phases 0–4 deliver everything the original request asked for (typed cells, formulas, per-cell
editing, the menu bar, and the formula bar) plus working file save/open and find/replace. Phase 6
remains open stretch work. See §11.8 for how this was verified without a Windows machine.

---

## 9. Test plan (NUnit, `Assert.That` constraint syntax, `Method_State_Expected` naming)

| Fixture | Coverage |
| --- | --- |
| `CellValueTests` | factories, type checks, coercion (`TryGetNumber` for bool/date/numeric text), equality, error values |
| `CellAddressTests` | `ToString`/`TryParse` for `A1`, `Z1`, `AA1`, `$A$1`; column name ⇄ index; hash/equality; invalid input |
| `CellRangeTests` | enumeration order, `Contains`, `Intersects`, `TryParse("A1:B10")`, normalisation of reversed corners |
| `CellInputParserTests` | number, negative, percent, date, boolean, apostrophe-forced text, formula, culture (nb-NO vs en-US decimal comma), empty |
| `CellFormatterTests` | edit-text round-trip, format strings, error text (`#DIV/0!`), general alignment inputs |
| `CellStyleTests` | immutability, interning identity, `With*` combinations |
| `FormulaLexerTests` | tokens, strings with escapes, refs vs function names, malformed input |
| `FormulaParserTests` | precedence, associativity, unary/percent, parentheses, function args, syntax errors |
| `FormulaEvaluatorTests` | arithmetic, comparison, concatenation, empty-cell semantics, error propagation, `#DIV/0!`, `#VALUE!`, `#REF!` |
| `FunctionTests` | every built-in: happy path, wrong arity, wrong types, empty ranges |
| `DependencyGraphTests` | precedent registration, transitive dirtying, range dependencies, edge removal on re-edit |
| `RecalculationEngineTests` | topological order, chain `A1→B1→C1`, direct and indirect cycles, deferred recalculation, no-change suppression |
| `SheetTests` | `SetCellText` variants, `ClearContents` keeps style, `UsedRange`, `GetCell` caching, `TryGetCell` not materialising, `CellChanged` events |
| `UndoStackTests` | single/range edits, style edits, undo→redo→undo, capacity |
| `SerializerTests` | JSON round-trip with formulas + styles, CSV export, malformed file handling, invariant culture |
| `FindServiceTests` | case sensitivity, entire-cell, values vs formulas, wrap-around, `ReplaceAll` count + single undo entry |

Target ≥ 80 % line coverage on the model library (per `AGENTS.md`). Adapter/converter behaviour that
does not need a `Dispatcher` (`SheetRowAdapter` setter semantics, `CellConverter`) gets tests too;
the WPF window itself is verified manually via `DataGridDemo.exe SpreadsheetExample`.

---

## 10. Performance notes

* Sparse `Dictionary<CellAddress, Cell>`; `CellAddress` is an `IEquatable` struct → no boxing.
* `CellValue` is a struct → no allocation for numbers, dates, booleans, errors.
* Formulas are parsed once per edit; recalculation walks only the dirty sub-graph.
* Styles are interned; identical formatting costs one shared object.
* Bulk operations (paste, replace-all, load, undo) run inside `DeferRecalculation()`.
* `PropertyChanged` is raised only when the computed value actually changed.
* No LINQ/allocation in the recalculation inner loop; reusable buffers for the DFS stacks.
* The `DataGrid` is not UI-virtualised → the demo defaults to 100 × 26 and documents the ceiling.
* Model mutations are expected on the UI thread (WPF affinity for `PropertyChanged`); the model is
  documented as not thread-safe, with `Sheet` mutation guarded by a re-entrancy check.

---

## 11. Risks and decisions to confirm

1. **Model lives inside `DataGridDemo`, not a separate library** (decided — §4). The `Model/`
   folder has no WPF references, so the seam is enforced by convention rather than the compiler;
   if that ever becomes a problem, lifting it into its own project later is a mechanical move
   (namespaces stay the same modulo the root).
2. **Formula culture split** — invariant inside formulas, current culture for literals. This matches
   how most .NET spreadsheet libraries behave but is worth an explicit sign-off.
3. **Paste via `TypeConverter`** (§3.5) keeps `PropertyTools.Wpf` untouched. If it proves too
   constraining, the fallback is a `SpreadsheetDataGrid : DataGrid` overriding `CreateOperator()`
   with a spreadsheet-specific `IDataGridOperator` — more control, still no library change.
4. **Reference fix-up on insert/delete rows/columns** is deferred to phase 6; until then the demo
   sets `CanInsert="False" CanDelete="False"` rather than silently corrupting formulas.
5. **`.xlsx` support** is out of scope (dependency-free examples).
6. **`CHANGELOG.md` requires an issue number** per `AGENTS.md`; an issue should be filed for this
   work so the entry can read
   `- DataGrid: Spreadsheet example with a typed cell model, formulas, formatting and file support #NNN`.
7. Building and running requires Windows; CI on this repo builds all target frameworks.
8. **Verification without a Windows machine.** This plan was implemented and verified on Linux,
   where neither `dotnet test` nor `dotnet run` can load the WPF runtime (see
   [AGENTS.md#building-on-linux](../../../AGENTS.md#building-on-linux-compile-time-verification-only)).
   Two techniques covered the gap:
   - `dotnet build -p:EnableWindowsTargeting=true` confirmed every file (C# and XAML) compiles,
     including `SpreadsheetViewModel`, the converters, and the code-behind — this is a real compiler
     run, not a guess.
   - The `Model/` namespace (§4) and the adapters (`SheetGridAdapter`, `SheetRowAdapter`) have no WPF
     dependency, so a throwaway NUnit project outside source control (never committed) could
     `<Compile Include>` those files directly by path and run the real test suite against them —
     302 tests passing by the end of phase 4, including the formula engine, dependency graph,
     recalculation, and JSON round-trip.
   - What this does **not** cover: `SpreadsheetControlFactory`, `SpreadsheetViewModel`'s
     `INotifyPropertyChanged` wiring in a live UI, the XAML bindings, and the menu/dialog
     interactions all require the WPF runtime to execute and were only verified by compilation plus
     manual code review — a Windows machine should exercise these before shipping.
