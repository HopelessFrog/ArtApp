using PetVideoApp.Pages;

namespace PetVideoApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(ArtFeedPage), typeof(ArtFeedPage));
        Routing.RegisterRoute(nameof(DrawingPage), typeof(DrawingPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}