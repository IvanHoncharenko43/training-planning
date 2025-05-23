using WpfApp1.Core;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class MainViewModel : WpfApp1.Core.ViewModel
{
    public RelayCommand NavigateToLogin { get; set; }
    public RelayCommand NavigateToHome { get; set; }
    private INavigationService _navigationService;

    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged(); // “епер метод доступний через базовий клас
        }
    }

    public MainViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigateToLogin = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);
        NavigateToHome = new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateToLogin.Execute(null);
    }
}