using System.ComponentModel;
using System.Windows.Controls;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Slider that calls IEditableObject.BeginEdit/EndEdit when thumb dragging.
    /// </summary>
    public class SliderEx : Slider
    {
        protected override void OnThumbDragStarted(System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            var editableObject = DataContext as IEditableObject;
            if (editableObject != null)
            {
                editableObject.BeginEdit();
            }
        }

        protected override void OnThumbDragCompleted(System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            base.OnThumbDragCompleted(e);
            var editableObject = DataContext as IEditableObject;
            if (editableObject != null)
            {
                editableObject.EndEdit();
            }
        }
    }
}