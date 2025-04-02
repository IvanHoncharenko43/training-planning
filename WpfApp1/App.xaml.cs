﻿using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.View;
using WpfApp1.ViewModel;

namespace WpfApp1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
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
        services.AddSingleton<VeryMainView>(provider => new VeryMainView()
        {
            DataContext = provider.GetRequiredService<VeryMainViewModel>()
        });
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<SettingsViewModel>();
        
        services.AddSingleton<VeryMainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<RegisterViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        

        services.AddSingleton<Func<Type, Core.ViewModel>>(provider =>
            viewModelType => (Core.ViewModel)provider.GetRequiredService(viewModelType));
        
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        // mainWindow.Show();
        // base.OnStartup(e);
        var mainWindow = _serviceProvider.GetRequiredService<VeryMainView>();
        mainWindow.Show();
        base.OnStartup(e);
    }
    }
