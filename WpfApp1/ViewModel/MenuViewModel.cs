using WpfApp1.Core;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class MenuViewModel : WpfApp1.Core.ViewModel
{
    private readonly INavigationService _navigationService;

    public RelayCommand NavigateToHomeCommand { get; set; }

    public MenuViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateToHomeCommand = new RelayCommand(o => { _navigationService.NavigateTo<HomeViewModel>(); }, o => true);
    }
}