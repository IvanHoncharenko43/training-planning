using WpfApp1.Core;
using WpfApp1.Services;
using WpfApp1.ViewModel;
namespace WpfApp1.ViewModel;

public class MainViewModel : Core.ViewModel
{
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
    }
}