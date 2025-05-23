using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WpfApp1.Data;
using WpfApp1.Services;
using WpfApp1.View;
using WpfApp1.ViewModel;

namespace WpfApp1;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<MainWindow>(provider => new MainWindow()
        {
            DataContext = provider.GetRequiredService<MainViewModel>()
        });

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>(provider => new LoginViewModel(
            provider.GetRequiredService<INavigationService>(),
            provider.GetRequiredService<UserRepository>()
        ));
        services.AddSingleton<MenuViewModel>(provider => new MenuViewModel(
            provider.GetRequiredService<INavigationService>()
        ));
        services.AddSingleton<HomeViewModel>(provider => new HomeViewModel(
            provider.GetRequiredService<AppDbContext>(),
            provider.GetRequiredService<INavigationService>()
        ));

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<Func<Type, Core.ViewModel>>(provider =>
            viewModelType => (Core.ViewModel)provider.GetRequiredService(viewModelType));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db")
                   .LogTo(Console.WriteLine, LogLevel.Information),
            ServiceLifetime.Singleton);

        services.AddSingleton<UserRepository>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }
}