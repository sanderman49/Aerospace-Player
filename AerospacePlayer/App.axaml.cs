using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Reflection;
using Avalonia.Markup.Xaml;
using AerospacePlayer.ViewModels;
using AerospacePlayer.Views;
using Avalonia.Interactivity;
using ReactiveUI;
using Splat;

namespace AerospacePlayer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var shell = new ShellViewModel();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

            desktop.MainWindow = new MainWindow
            {
                DataContext = shell
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
            Locator.CurrentMutable.Register(() => new ShellView(), typeof(IViewFor<ShellViewModel>));
            Locator.CurrentMutable.Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>));
            Locator.CurrentMutable.Register(() => new ProgramsView(), typeof(IViewFor<ProgramsViewModel>));
            Locator.CurrentMutable.Register(() => new EditProgramView(), typeof(IViewFor<EditProgramViewModel>));
            Locator.CurrentMutable.Register(() => new PatchSelectView(), typeof(IViewFor<PatchSelectViewModel>));
            
            singleViewPlatform.MainView = new ShellView
            {
                DataContext = shell
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}