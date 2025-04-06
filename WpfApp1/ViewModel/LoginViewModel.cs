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

    private string _registerLogin = "Логін";
    public string RegisterLogin
    {
        get => _registerLogin;
        set
        {
            _registerLogin = value;
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

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    private UserRepository _userRepository;
    public RelayCommand LoginCommand { get; set; }
    public RelayCommand RegisterCommand { get; set; }
    public RelayCommand NavigateToMenu { get; set; }
    private INavigationService _navigationService;
    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }
    }

    public LoginViewModel(INavigationService navigationService, UserRepository userRepository)
    {
        NavigationService = navigationService;
        _userRepository = userRepository;
        LoginCommand = new RelayCommand(o => Login(), o => CheckLogin());
        RegisterCommand = new RelayCommand(o => Register(), o => CheckRegister());
        NavigateToMenu = new RelayCommand(o => { NavigationService.NavigateTo<MenuViewModel>(); }, o => true);
    }

    private void Login()
    {
        try
        {
            bool isSucceed = _userRepository.CheckIfExists(LoginEmail, LoginPassword);
            if (isSucceed)
            {
                NavigateToMenu.Execute(null);
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorMessage = "Невірний email або пароль";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка входу: {ex.Message}";
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
        try
        {
            _userRepository.Register(RegisterLogin, RegisterEmail, RegisterPassword);
            LoginEmail = RegisterEmail;
            LoginPassword = RegisterPassword;
            Login();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка реєстрації: {ex.Message}";
        }
    }

    private bool CheckRegister()
    {
        if (string.IsNullOrWhiteSpace(RegisterLogin) || string.IsNullOrWhiteSpace(RegisterEmail) || string.IsNullOrWhiteSpace(RegisterPassword) ||
            RegisterLogin == "Логін" || RegisterEmail == "Email")
        {
            return false;
        }
        return true;
    }
}