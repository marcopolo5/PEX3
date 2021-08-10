﻿using Domain.AccountContracts;
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
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using AccountModule.Controllers;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        /// <summary>
        /// Can be used for login, register and logout
        /// </summary>
        private readonly ApplicationUserController _applicationUserController = new();
        //private CustomMessageBox messageBox = new CustomMessageBox();

        public RegisterWindow()
        {
           InitializeComponent();
        }

        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the registerButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRegisterClick(object sender, RoutedEventArgs e)
        {
            string _registerErrorMessage = _applicationUserController.CheckRegisterConstraints(firstNameText.Text, lastNameText.Text, emailText.Text, password1Text.Password, password2Text.Password);
            bool validData = (_registerErrorMessage == "");
            if (validData)
            {
                if (await _applicationUserController.Register(
                    firstNameText.Text, lastNameText.Text, emailText.Text, password1Text.Password))
                {
                    //registerErrorMessage.Foreground = Brushes.Green;
                    //registerErrorMessage.Content = "You have registered successfully!";                  
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.Show("You have registered successfully!");
                }
                else
                {
                    CustomMessageBox messageBox = new CustomMessageBox();                   
                    //registerErrorMessage.Content = "This e-mail has already been used";
                    messageBox.Show("This e-mail has already been used");
                }
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Show(_registerErrorMessage);
                //registerErrorMessage.Content = _registerErrorMessage;
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            firstNameText.Clear();
            lastNameText.Clear();
            emailText.Clear();
            password1Text.Clear();
            password2Text.Clear();
        }

        private void loginTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new LoginWindow().ShowDialog();
            ShowDialog();
        }

        private void returnTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new MainWindow().ShowDialog();
            ShowDialog();
        }

        private void grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
      
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);

        }
    }
}
