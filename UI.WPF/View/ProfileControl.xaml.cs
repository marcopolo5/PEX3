using AccountModule.Controllers;
using Domain.Helpers;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UI.WPF.Common;
using Domain.Exceptions;

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>  
    public partial class ProfileControl : UserControl
    {
        private readonly ProfileController _profileController = new();

        private OpenFileDialog openFileDialog = new();

        public ProfileControl()
        {
            InitializeComponent();

            // Load profile informations
            displayNameText.Text = ApplicationUserController.CurrentUser.Profile.DisplayName;
            aboutText.Text = ApplicationUserController.CurrentUser.Profile.StatusMessage;
            var bi_profilePicture = BitmapImageLoader.LoadImage(ApplicationUserController.CurrentUser.Profile.Image);
            profilePicture.Fill = new ImageBrush(bi_profilePicture);
            var reputation = ApplicationUserController.CurrentUser.Profile.Reputation.ToString();
            ratingValueTextBLock.Text = reputation;
        }

        /// <summary>
        /// Opens a file dialog and let user choose a new profile picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Filter = "Image files (*.png;*.jpeg, *.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                profilePicture.Fill = new ImageBrush(new BitmapImage(new Uri(openFileDialog.FileName)));
            }
        }

        /// <summary>
        /// Updates profile with the new informations:
        /// - display name
        /// - status message
        /// - profile picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    var imageByteArray = ImageHelper.GetImageBytes(openFileDialog.FileName);
                    await _profileController.UpdateProfile(displayNameText.Text, aboutText.Text, imageByteArray);
                    new CustomMessageBox().Show("Profile updated successfully");
                }
                catch(ArgumentException)
                {
                    await _profileController.UpdateProfile(displayNameText.Text, aboutText.Text, ApplicationUserController.CurrentUser.Profile.Image);
                    new CustomMessageBox().Show("Profile updated successfully");
                }
            }
            catch (InvalidEntityException exception)
            {
                new CustomMessageBox().Show(exception.Message);
            }
        }

    }
}
