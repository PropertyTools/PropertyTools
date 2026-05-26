# PropertyTools Concepts

This document explains the key design patterns and concepts used throughout the PropertyTools library.

---

## Table of Contents

- [Operator](#operator)
- [Control Factory](#control-factory)
- [Localizable Operator](#localizable-operator)
- [How Operators and Control Factories Work Together](#how-operators-and-control-factories-work-together)

---

## Operator

### What Is an Operator?

An **operator** is a *strategy* object that encapsulates data-access and data-manipulation logic for a PropertyTools control. It acts as an intermediary between the control and its underlying data source, adapting the control's behavior to different data structures without modifying the control itself.

This follows the **Strategy design pattern** — the control delegates data-specific work to the operator, and different operator implementations can be swapped in to support different data source types.

### Why Does It Exist?

Controls such as `DataGrid` and `PropertyGrid` must work with many different data structures:

| Control | Example Data Sources |
|---|---|
| `DataGrid` | `IList<T>`, two-dimensional arrays, `DataTable`, `IEnumerable` |
| `PropertyGrid` | Any object with public properties, `ICustomTypeDescriptor` implementations |

Each data source type requires different logic for reading/writing values, counting rows and columns, inserting items, and so on. Rather than embedding all of this logic inside the control (violating the *Single Responsibility Principle*), the operator pattern extracts it into a replaceable component.

### Interfaces

| Interface | Control | Responsibility |
|---|---|---|
| `IDataGridOperator` | `DataGrid` | Cell value access, row/column counts, insert/delete, auto-generating columns |
| `IPropertyGridOperator` | `PropertyGrid` | Inspecting an object and producing a model of `Tab` objects that drive the UI |

### Default Behavior

Both controls create a default operator automatically when their data source changes:

- **DataGrid** selects an operator based on the `ItemsSource` type (e.g., `ListOperator` for `IList`, `Array2DOperator` for 2-D arrays).
- **PropertyGrid** uses `PropertyGridOperator`, which reflects over public properties and respects data annotations and custom attributes.

### Creating a Custom Operator

To customize how the control interacts with data, create a class that extends the default operator (or implements the interface directly) and assign it to the control's `Operator` property.

**Example — Custom PropertyGrid Operator:**

```csharp
using PropertyTools.Wpf;

// Inherit from the default operator and override only what you need.
public class MyOperator : PropertyGridOperator
{
    // Override CreateModel to change how properties are discovered or grouped.
}
```

```xml
<pt:PropertyGrid>
    <pt:PropertyGrid.Operator>
        <local:MyOperator />
    </pt:PropertyGrid.Operator>
</pt:PropertyGrid>
```

---

## Control Factory

### What Is a Control Factory?

A **control factory** is a *creational* object responsible for producing the WPF `FrameworkElement` controls that are placed inside a host control to represent individual data items. It follows the **Abstract Factory design pattern** — the factory decides *what* control to create, while the host control decides *where* to place it.

### Why Does It Exist?

Controls like `DataGrid` and `PropertyGrid` display many different property types (strings, numbers, booleans, enums, colors, file paths, and so on). Each type needs a different editor widget. Hard-coding all of these mappings inside the control would make it rigid and difficult to extend. The control factory pattern extracts this mapping into a replaceable component.

### Interfaces

| Interface | Control | Responsibility |
|---|---|---|
| `IDataGridControlFactory` | `DataGrid` | Creates display and edit controls for each cell based on a `CellDefinition` |
| `IPropertyGridControlFactory` | `PropertyGrid` | Creates editor controls for each property row based on a `PropertyItem` |

### Default Behavior

- **`DataGridControlFactory`** maps common .NET types to standard WPF controls (e.g., `TextBlock` for display, `TextBox` for editing strings, `CheckBox` for booleans).
- **`PropertyGridControlFactory`** provides a rich set of editors including `TextBox`, `CheckBox`, `ComboBox`, `Slider`, `ColorPicker`, file/directory pickers, and more.

### Creating a Custom Control Factory

To introduce specialized editors or third-party controls, extend the default factory and override the creation method for the types you want to customize.

**Example — Custom PropertyGrid Control Factory:**

```csharp
using PropertyTools.Wpf;
using System.Windows;

public class MyControlFactory : PropertyGridControlFactory
{
    public override FrameworkElement CreateControl(
        PropertyItem pi, PropertyControlFactoryOptions options)
    {
        // Provide a custom editor for a specific property type or attribute.
        if (pi.Is(typeof(MySpecialType)))
        {
            return CreateMySpecialEditor(pi);
        }

        // Fall back to the default behavior for everything else.
        return base.CreateControl(pi, options);
    }

    private FrameworkElement CreateMySpecialEditor(PropertyItem pi)
    {
        // Build and return a custom FrameworkElement here.
        // ...
    }
}
```

```xml
<pt:PropertyGrid>
    <pt:PropertyGrid.ControlFactory>
        <local:MyControlFactory />
    </pt:PropertyGrid.ControlFactory>
</pt:PropertyGrid>
```

---

## Localizable Operator

### What Is a Localizable Operator?

A **localizable operator** provides localization (translation) support for display strings and descriptions shown by PropertyTools controls. It is defined by the `ILocalizableOperator` interface and can be injected into any operator via the `ICustomLocalizableOperator.UseLocalizableOperator` method or by setting the `LocalizableOperator` property on the control.

### Interfaces

| Interface | Responsibility |
|---|---|
| `ILocalizableOperator` | Translates resource keys into localized strings and descriptions |
| `ICustomLocalizableOperator` | Allows an operator to accept a custom `ILocalizableOperator` for delegation |

### Default Behavior

The `DefaultLocalizableOperator` base class returns the key itself as the localized value (i.e., no translation). Both `DataGridOperator` and `PropertyGridOperator` extend this class, so localization is a no-op unless a custom localizable operator is supplied.

### Creating a Custom Localizable Operator

```csharp
using PropertyTools.Wpf.Operators;
using System;

public class MyLocalizableOperator : DefaultLocalizableOperator
{
    public override string GetLocalizedString(string key, Type declaringType)
    {
        // Look up the key in your resource file or localization system.
        var resourceKey = declaringType != null
            ? $"{declaringType.FullName}.{key}"
            : key;

        return MyResources.ResourceManager.GetString(resourceKey) ?? key;
    }
}
```

```xml
<pt:PropertyGrid>
    <pt:PropertyGrid.LocalizableOperator>
        <local:MyLocalizableOperator />
    </pt:PropertyGrid.LocalizableOperator>
</pt:PropertyGrid>
```

---

## How Operators and Control Factories Work Together

The operator and the control factory serve complementary roles within the same control. Their responsibilities are clearly separated:

```
┌─────────────────────────────────────────────────────────────────┐
│                        DataGrid / PropertyGrid                  │
│                                                                 │
│  ┌───────────────────┐          ┌────────────────────────────┐  │
│  │     Operator       │          │     Control Factory         │  │
│  │  (Strategy)        │          │  (Abstract Factory)         │  │
│  │                    │          │                             │  │
│  │  • Reads data      │──────▶  │  • Creates display controls │  │
│  │  • Writes data     │  model  │  • Creates edit controls    │  │
│  │  • Counts rows/cols│  info   │  • Maps types to editors    │  │
│  │  • Inserts/deletes │          │                             │  │
│  │  • Builds model    │          │                             │  │
│  └───────────────────┘          └────────────────────────────┘  │
│            ▲                                ▲                    │
│            │                                │                    │
│       ItemsSource /                   ControlFactory             │
│       Operator property               property                  │
└─────────────────────────────────────────────────────────────────┘
```

1. The **operator** reads the data source and produces metadata (cell definitions, property items, column/row counts).
2. The **control factory** receives that metadata and creates the appropriate WPF controls.
3. Both can be replaced independently, giving consumers fine-grained control over data access *and* UI rendering.

This separation means you can change *how data is accessed* (custom operator) without changing *how it is displayed* (control factory), and vice versa.
