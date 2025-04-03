using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoginApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleForm_Click(object sender, MouseButtonEventArgs e)
        {
            if (LoginForm.Visibility == Visibility.Visible)
            {
                LoginForm.Visibility = Visibility.Collapsed;
                RegisterForm.Visibility = Visibility.Visible;
                FormTitle.Text = "Реєстрація";
            }
            else
            {
                LoginForm.Visibility = Visibility.Visible;
                RegisterForm.Visibility = Visibility.Collapsed;
                FormTitle.Text = "Вхід";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Email" || textBox.Text == "Ім'я"))
            {
                textBox.Text = "";
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // логіку входу судазес
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // логіка реєстрації судазес
        }
    }
}