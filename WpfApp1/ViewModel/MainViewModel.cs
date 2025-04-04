using System.Security;
using System.Windows.Media.TextFormatting;
using WpfApp1.Core;
using WpfApp1.Data;
using WpfApp1.Services;
using WpfApp1.View;
using WpfApp1.ViewModel;
namespace WpfApp1.ViewModel;

public class MainViewModel : Core.ViewModel
{
    public RelayCommand NavigateToLogin { get; set; }
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
    public MainViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        //LoginCommand = new RelayCommand(o => Login(), o => CheckLogin());
        NavigateToLogin = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);
        NavigateToLogin.Execute(null);
    }
}