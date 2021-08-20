using AccountModule.Controllers;
using Microsoft.Win32;
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

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>  
    public partial class ProfileControl : UserControl
    {
        /// <summary>
        /// TO DO
        /// ADD about you section in DB for profile
        /// </summary>
        private readonly ProfileController _profileController = new();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public ProfileControl()
        {
            InitializeComponent();
            string displayName = ApplicationUserController.CurrentUser.LastName; // + " " + ApplicationUserController.CurrentUser.LastName;
            this.displayNameText.Text = displayName;
            this.aboutText.Text = "Say something about you"; // this.aboutText.Text  = ApplicationUserController.CurrentUser.About; // add about column
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }


        private void photoButton_Click(object sender, RoutedEventArgs e)
        {
           
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                profilePicture.Fill = new ImageBrush(new BitmapImage(new Uri(openFileDialog.FileName)));
            }

        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (await _profileController.UpdateProfile(displayNameText.Text, aboutText.Text, openFileDialog.FileName))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Show("Profile updated Succesfuly!");
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Show("There was a problem");
            }
            
        }
    }
}
