using AccountModule.Controllers;
using Domain;
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
        
            this.displayNameText.Text = ApplicationUserController.CurrentUser.Profile.DisplayName ;
            this.aboutText.Text = "Say something about you"; // this.aboutText.Text  = ApplicationUserController.CurrentUser.About; // add about column
            //this.profilePicture.Fill = new ImageBrush(new BitmapImage(new Uri(ApplicationUserController.CurrentUser.Profile.Image)));
            var reputation = ApplicationUserController.CurrentUser.Profile.Reputation;
            this.ratingValueTextBLock.Text = reputation + "";
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

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {          
            await _profileController.UpdateProfile(displayNameText.Text, aboutText.Text, openFileDialog.FileName);
        }
    }
}
