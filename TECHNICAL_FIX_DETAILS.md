# PropertyGrid IDataErrorInfo Style Application Fix

## Problem Statement

When using PropertyGrid with a Style that defines implicit styles in Style.Resources, these implicit styles were not being applied to controls (like TextBoxEx) when the PropertyGrid was bound to objects implementing `IDataErrorInfo` or `INotifyDataErrorInfo`.

### Example Scenario

```xml
<Style TargetType="pt:PropertyGrid" x:Key="MyStyle">
    <Setter Property="TabVisibility" Value="VisibleIfMoreThanOne" />
    <Style.Resources>
        <Style TargetType="pt:TextBoxEx">
            <Setter Property="Background" Value="Yellow" />
        </Style>
    </Style.Resources>
</Style>

<!-- This works correctly -->
<pt:PropertyGrid Style="{StaticResource MyStyle}" 
                 SelectedObject="{Binding PersonWithoutIDataErrorInfo}" />

<!-- This did NOT work - implicit style was not applied -->
<pt:PropertyGrid Style="{StaticResource MyStyle}" 
                 SelectedObject="{Binding PersonWithIDataErrorInfo}" />
```

## Root Cause

The issue was in the `PropertyGrid.cs` file, in the `AddPropertyPanel` method:

1. **Control Creation** (line 1441): `var propertyControl = this.CreatePropertyControl(pi);`
   - Control is created with `Style` property = null

2. **ValidationErrorStyle Applied** (line 1460 - BEFORE fix): `this.ControlFactory.SetValidationErrorStyle(propertyControl, options);`
   - **Problem**: This explicitly sets `control.Style = ValidationErrorStyle`
   - The control is NOT yet in the visual tree
   - Implicit styles from Style.Resources cannot be applied once Style is explicitly set

3. **Control Added to Visual Tree** (line 1480): `propertyPanel.Children.Add(propertyControl);`
   - Too late - Style was already explicitly set in step 2

## The Fix

### 1. PropertyGrid.cs Changes

**Moved the `SetValidationErrorStyle` call to AFTER the control is added to the visual tree:**

```csharp
// BEFORE (line 1460 - called before adding to visual tree)
this.ControlFactory.SetValidationErrorStyle(propertyControl, options);
// ...
propertyPanel.Children.Add(propertyControl); // line 1480

// AFTER (called after adding to visual tree)
propertyPanel.Children.Add(propertyControl); // line 1480
// Apply ValidationErrorStyle after the control is added to the visual tree
// so that implicit styles from Style.Resources can be applied first
if (validationOptions != null)
{
    this.ControlFactory.SetValidationErrorStyle(propertyControl, validationOptions);
}
```

### 2. PropertyGridControlFactory.cs Changes

**Modified `SetValidationErrorStyle` to respect existing styles:**

```csharp
// BEFORE
public virtual void SetValidationErrorStyle(FrameworkElement control, PropertyControlFactoryOptions options)
{
    if (options.ValidationErrorStyle != null)
    {
        control.Style = options.ValidationErrorStyle;
    }
}

// AFTER
public virtual void SetValidationErrorStyle(FrameworkElement control, PropertyControlFactoryOptions options)
{
    // Only apply ValidationErrorStyle if the control doesn't already have a style applied.
    // This allows implicit styles from Style.Resources to take precedence.
    if (options.ValidationErrorStyle != null && control.Style == null)
    {
        control.Style = options.ValidationErrorStyle;
    }
}
```

## Why This Works

1. **Control is created** with Style = null
2. **Control is added to visual tree** 
3. **WPF resolves implicit styles** from Style.Resources and sets control.Style automatically
4. **SetValidationErrorStyle is called**
   - If control.Style != null (implicit style was applied), it does NOT override
   - If control.Style == null (no implicit style), it applies ValidationErrorStyle

## Impact

- **Backward Compatible**: Existing code that doesn't use Style.Resources continues to work
- **Enables New Scenarios**: Users can now define implicit styles in PropertyGrid's Style.Resources that work with IDataErrorInfo
- **Respects User Intent**: Implicit styles (user-defined) take precedence over default ValidationErrorStyle

## Testing

The fix can be tested using the `StyleApplicationDemo` example application:
- Located in: `Source/Examples/PropertyGrid/StyleApplicationDemo/`
- Shows two PropertyGrids with the same style
- Left: Person without IDataErrorInfo
- Right: Person2 with IDataErrorInfo
- Both should display yellow TextBoxEx backgrounds after the fix

## Related Files

- `Source/PropertyTools.Wpf/PropertyGrid/PropertyGrid.cs` - Main fix
- `Source/PropertyTools.Wpf/PropertyGrid/PropertyGridControlFactory.cs` - Supporting fix
- `Source/Examples/PropertyGrid/StyleApplicationDemo/` - Test application
- `CHANGELOG.md` - Updated with fix details
