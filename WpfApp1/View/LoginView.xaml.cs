using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.View;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null && (textBox.Text == "Email" || textBox.Text == "Ім'я"))
        {
            textBox.Text = "";
        }
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
}