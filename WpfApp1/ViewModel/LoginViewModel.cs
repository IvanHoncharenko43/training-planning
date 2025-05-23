using WpfApp1.Core;
using WpfApp1.Data;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class LoginViewModel : Core.ViewModel
{
    private string _loginEmail = "Email";
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

    private string _registerName = "Ім'я";
    public string RegisterName
    {
        get => _registerName;
        set
        {
            _registerName = value;
            OnPropertyChanged();
        }
    }
    private string _registerEmail = "Email";
    public string RegisterEmail
    {
        get => _registerEmail;
        set
        {
            _registerEmail = value;
            OnPropertyChanged();
        }
    }
    private string _registerPassword;
    public string RegisterPassword
    {
        get => _registerPassword;
        set
        {
            _registerPassword = value;
            OnPropertyChanged();
        }
    }

    private readonly UserRepository _userRepository;
    private readonly INavigationService _navigationService; // readonly

    public INavigationService NavigationService => _navigationService; // get-only

    public RelayCommand LoginCommand { get; set; }
    public RelayCommand RegisterCommand { get; set; }
    public RelayCommand NavigateToMenu { get; set; }

    public LoginViewModel(INavigationService navigationService, UserRepository userRepository)
    {
        _navigationService = navigationService;
        _userRepository = userRepository;
        LoginCommand = new RelayCommand(o => Login(), o => CheckLogin());
        RegisterCommand = new RelayCommand(o => Register(), o => CheckRegister());
        NavigateToMenu = new RelayCommand(o => { _navigationService.NavigateTo<MenuViewModel>(); }, o => true);
    }

    private void Login()
    {
        bool isSucceed = _userRepository.CheckIfExists(LoginEmail, LoginPassword);
        if (isSucceed)
        {
            var user = _userRepository.GetUserByEmail(LoginEmail);
            if (user != null)
            {
                _navigationService.CurrentUserId = user.Id;
                NavigateToMenu.Execute(null);
            }
        }
    }

    private bool CheckLogin()
    {
        if (string.IsNullOrWhiteSpace(LoginEmail) || string.IsNullOrWhiteSpace(LoginPassword) || LoginEmail == "Email")
        {
            return false;
        }
        return true;
    }

    private void Register()
    {
        _userRepository.Register(RegisterName, RegisterEmail, RegisterPassword);
        LoginEmail = RegisterEmail;
        LoginPassword = RegisterPassword;
        Login();
    }

    private bool CheckRegister()
    {
        if (string.IsNullOrWhiteSpace(RegisterName) || string.IsNullOrWhiteSpace(RegisterEmail) || string.IsNullOrWhiteSpace(RegisterPassword) ||
            RegisterName == "Ім'я" || RegisterEmail == "Email")
        {
            return false;
        }
        return true;
    }
}