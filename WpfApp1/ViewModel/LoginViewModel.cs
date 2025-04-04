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

    public LoginViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        LoginCommand = new RelayCommand(o => Login(), o => CheckLogin());
        RegisterCommand = new RelayCommand(o => Register(), o => CheckRegister());
        NavigateToMenu = new RelayCommand(o => { NavigationService.NavigateTo<MenuViewModel>(); }, o => true);
        _userRepository = new UserRepository();
    }
    private void Login()
    {
        bool isSucceed = _userRepository.CheckIfExists(LoginEmail, LoginPassword);
        if (isSucceed)
            NavigateToMenu.Execute(null);
        return; //висвітлення вікна про некоретне введення даних
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
        //NavigateToMenu.Execute(null);
    }

    private bool CheckRegister()
    {
        if (string.IsNullOrWhiteSpace(RegisterName) || string.IsNullOrWhiteSpace(RegisterEmail) || string.IsNullOrWhiteSpace(RegisterPassword) ||
            RegisterName == "Name" || RegisterEmail == "Email")
        {
            return false;
        }

        return true;
    }
}