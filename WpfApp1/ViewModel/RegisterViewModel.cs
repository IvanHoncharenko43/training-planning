using WpfApp1.Core;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class RegisterViewModel : Core.ViewModel
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
    public RegisterViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigateToLogin = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);
    }
}