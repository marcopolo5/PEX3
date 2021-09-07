using System.Windows;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public void Show(string message)
        {
            messageText.Text = message;
            this.Show();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
