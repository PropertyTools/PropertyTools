# StyleApplicationDemo

This demo application demonstrates the fix for the PropertyGrid style application issue when using IDataErrorInfo.

## Issue

When a PropertyGrid style includes implicit styles in Style.Resources (e.g., setting a yellow background for TextBoxEx controls), these styles were not being applied to controls when the PropertyGrid was bound to objects implementing IDataErrorInfo or INotifyDataErrorInfo.

## Demonstration

This app shows two PropertyGrid controls side by side:

- **Left Grid**: Bound to `Person1` (without IDataErrorInfo implementation)
- **Right Grid**: Bound to `Person2` (with IDataErrorInfo implementation)

Both PropertyGrids use the same style resource `MyStyle`, which includes:
```xml
<Style.Resources>
    <Style TargetType="pt:TextBoxEx">
        <Setter Property="Background" Value="Yellow" />
    </Style>
</Style.Resources>
```

## Expected Behavior (After Fix)

Both PropertyGrids should display TextBoxEx controls with yellow backgrounds, regardless of whether the bound object implements IDataErrorInfo.

## Bug Behavior (Before Fix)

- Left Grid (Person1): TextBoxEx controls have yellow background ✓
- Right Grid (Person2): TextBoxEx controls have default background ✗

## Technical Details

The issue was caused by the PropertyGrid applying `ValidationErrorStyle` before the control was added to the visual tree. This prevented implicit styles from Style.Resources from being resolved.

The fix:
1. Moved the `SetValidationErrorStyle` call to after the control is added to the visual tree
2. Modified `SetValidationErrorStyle` to only apply ValidationErrorStyle if the control doesn't already have a style (control.Style == null)
3. This allows implicit styles from Style.Resources to take precedence

## Building and Running

```bash
dotnet build StyleApplicationDemo.csproj
dotnet run --project StyleApplicationDemo.csproj
```

Note: This requires Windows as it's a WPF application.
