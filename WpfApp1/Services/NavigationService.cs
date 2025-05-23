namespace WpfApp1.Services;
using WpfApp1.Core;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void NavigateTo<TViewModel>() where TViewModel : ViewModel;
    int CurrentUserId { get; set; }
}

public class NavigationService : ObservableObject, INavigationService
{
    private Func<Type, ViewModel> _viewModelFactory;
    private ViewModel _currentView;
    private int _currentUserId;

    public ViewModel CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public int CurrentUserId
    {
        get => _currentUserId;
        set
        {
            _currentUserId = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, ViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModel
    {
        ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}