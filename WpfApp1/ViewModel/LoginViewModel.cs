using System.Security;
using WpfApp1.Core;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class LoginViewModel : Core.ViewModel
{
    private string _username;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }
    private SecureString _password;

    public SecureString Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }
    
    private INavigationService _navigationService;
    public RelayCommand NavigateToRegister { get; set; }
    
    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }
    }
    public LoginViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigateToRegister = new RelayCommand(o => { NavigationService.NavigateTo<RegisterViewModel>(); }, o => true);
    }
}