using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AccountModule.Controllers;

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private readonly SettingsController _settingsController = new();
        private bool handle = true;

        public SettingsControl()
        {
            InitializeComponent();
        }



        private async void SaveChangesClick(object sender, RoutedEventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            string _saveChangesErrorMessage = await _settingsController.SaveChanges(currentPassword.Password, newPassword.Password, retypedPassword.Password);
            if (_saveChangesErrorMessage.Equals(""))
                messageBox.Show("Password updated successfully!");
            else
            {
                if (_saveChangesErrorMessage.Equals("google"))
                    messageBox.Show("You cannot change your Google account password!");
                else
                    messageBox.Show("Incorrect password!");
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _settingsController.SetProximityRadius((int)radiusSlider.Value);
        }

        private void Anonimity_ValueChanged(object sender, RoutedEventArgs e)
        {
            _settingsController.SetAnonimity((bool)anonimityButton.IsChecked);
        }

        private void FontFamilyComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                ApplyFontSelection();
            }
            handle = true;
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            ApplyFontSelection();
        }

        private void ApplyFontSelection()
        {
            FontFamily fontFamily = (FontFamily)FontFamilyComboBox.SelectedItem;
            if(fontFamily == null)
            {
                return;
            }
            Application.Current.Resources["GlobalFontFamily"] = new FontFamily(fontFamily.Source);
        }


    }
}
