using System.Security;
using WpfApp1.Core;
using WpfApp1.Services;
using WpfApp1.ViewModel;
namespace WpfApp1.ViewModel;

public class MainViewModel : Core.ViewModel
{
    private string _loginEmail;
    public string LoginEmail
    {
        get => _loginEmail;
        set
        {
            _loginEmail = value;
            OnPropertyChanged();
        }
    }
    private string _loginPassword;
    public string LoginPassword
    {
        get => _loginPassword;
        set
        {
            _loginPassword = value;
            OnPropertyChanged();
        }
    }
    public RelayCommand LoginCommand { get; set; }
    public RelayCommand RegisterCommand { get; set; }
    private INavigationService _navigationService;
    public RelayCommand NavigateToLogin { get; set; }
    
    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }
    }
    public MainViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigateToLogin = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);
        LoginCommand = new RelayCommand(o => Login(), o => CheckLogin());
        RegisterCommand = new RelayCommand(o => Register(), o => true);
    }

    private void Login()
    {
        if (string.IsNullOrWhiteSpace(LoginEmail) || string.IsNullOrWhiteSpace(LoginPassword))
        {
            return;  //пізніше допишу помилку
        }
        
    }

    private bool CheckLogin()
    {
        return false;
    }

    private void Register()
    {
        if (string.IsNullOrWhiteSpace(LoginEmail) || string.IsNullOrWhiteSpace(LoginPassword))
        {
            return;
        }
        
    }
}