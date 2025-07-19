using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

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

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        Window?.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#1a1b26")));
        Window?.SetStatusBarColor(Color.ParseColor("#1a1b26"));
        Window?.SetNavigationBarColor(Color.ParseColor("#1a1b26"));
    }
}