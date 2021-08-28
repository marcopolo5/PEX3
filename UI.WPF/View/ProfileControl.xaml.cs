﻿using AccountModule.Controllers;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            BitmapImage bi_profilePicture = LoadImage(ApplicationUserController.CurrentUser.Profile.Image);
            profilePicture.Fill = new ImageBrush(bi_profilePicture);
            int reputation = ApplicationUserController.CurrentUser.Profile.Reputation;
            ratingValueTextBLock.Text = reputation + "";
        }

        /// <summary>
        /// Opens a file dialog and let user choose a new profile picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
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
            byte[] imageByteArray = ApplicationUserController.GetImageBytes(openFileDialog.FileName);
            await _profileController.UpdateProfile(displayNameText.Text, aboutText.Text, imageByteArray);
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.Show("Profile updated successfully");
        }

        /// <summary>
        /// Convert binary array into a BitmapImage
        /// </summary>
        /// <param name="imageData">The binary array with the image data</param>
        /// <returns>BitmapImage corresponding to the binary array given</returns>
        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            BitmapImage bi_image = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                bi_image.BeginInit();
                bi_image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bi_image.CacheOption = BitmapCacheOption.OnLoad;
                bi_image.UriSource = null;
                bi_image.StreamSource = memoryStream;
                bi_image.EndInit();
            }
            bi_image.Freeze();
            return bi_image;
        }

    }
}
