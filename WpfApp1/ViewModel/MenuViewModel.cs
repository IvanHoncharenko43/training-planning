using WpfApp1.Core;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class MenuViewModel : Core.ViewModel
{
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

    public RelayCommand NavigateToSettingsCommand { get; set; }

    public MenuViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigateToSettingsCommand = new RelayCommand(o => NavigateToSettings(), o => true);
    }

    private void NavigateToSettings()
    {
        NavigationService.NavigateTo<SettingsViewModel>();
    }
}   