using System.Windows;
using System.Windows.Controls.Primitives;

namespace UI.WPF
{
    public class CustomToggleButton : ToggleButton
    {
        static CustomToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomToggleButton), new FrameworkPropertyMetadata(typeof(CustomToggleButton)));
        }
    }
}
