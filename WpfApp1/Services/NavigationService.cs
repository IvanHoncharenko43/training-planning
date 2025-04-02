namespace WpfApp1.Services;
using WpfApp1.Core;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void NavigateTo<TViewModel>() where TViewModel : ViewModel;
}
public class NavigationService : ObservableObject, INavigationService
{
    private Func<Type, ViewModel> _viewModelFactory;
    private ViewModel _currentView;
    public ViewModel CurrentView
    {
        get => _currentView;
        private set
        {
           _currentView = value;
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