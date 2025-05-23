using WpfApp1.Core;
using WpfApp1.Data;
using WpfApp1.Services;

namespace WpfApp1.ViewModel;

public class HomeViewModel : WpfApp1.Core.ViewModel
{
    private readonly AppDbContext _context;
    private readonly INavigationService _navigationService;

    public HomeViewModel(AppDbContext context, INavigationService navigationService)
    {
        _context = context;
        _navigationService = navigationService;
    }
}