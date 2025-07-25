using AerospacePlayer.ViewModels;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AerospacePlayer.Android;

[Activity(
    Label = "AerospacePlayer.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    WindowSoftInputMode = SoftInput.AdjustResize,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    public override void OnBackPressed()
    {
        var lifetime = Avalonia.Application.Current.ApplicationLifetime as ISingleViewApplicationLifetime;

        var vm = lifetime.MainView?.DataContext as ShellViewModel;

        if (vm.Router?.NavigationStack?.Count > 1)
        {
            vm.Router.NavigateBack.Execute();
        }
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }
}