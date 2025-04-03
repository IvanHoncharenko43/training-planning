using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.ComponentControl;

public partial class BindablePasswordBox : UserControl
{
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
        new PropertyMetadata(string.Empty));

    public string Password
    {
        get
        {
            return (string)GetValue(PasswordProperty);
        }
        set
        {
            SetValue(PasswordProperty, value);
        }
    }
    public BindablePasswordBox()
    {
        InitializeComponent();
    }

    private void LoginPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = PasswordBox.Password;
    }
}