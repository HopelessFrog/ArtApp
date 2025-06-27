using System;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using PetVideoApp.Pages;
using Refit;
using PetVideoApp;
using PetVideoApp.APIs;
using PetVideoApp.Popups;
using PetVideoApp.Popups.PopupViewModels;
using PetVideoApp.Services;
using PetVideoApp.ViewModels;
using PetVideoApp.Handlers;
using PetVideoApp.Converters;
using PetVideoApp.Consts;

namespace PetVideoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.Services.AddLogging(logging =>
        {
            logging.AddDebug(); 
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddTransient<AuthHeaderHandler>();

        builder.Services.AddTransient<ITokenStorage, TokenStorage>();
        builder.Services.AddTransient<IAuthService, AuthService>();
        
        builder.Services.ConfigureRefit();

        builder.Services.AddTransient<LoginViewModel>()
            .AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterViewModel>()
            .AddTransient<RegisterPage>();
        builder.Services.AddSingleton<SettingsViewModel>()
            .AddTransient<SettingsPage>();
        builder.Services.AddSingleton<ArtFeedViewModel>()
            .AddTransient<ArtFeedPage>();
        builder.Services.AddSingleton<DrawingViewModel>()
            .AddTransient<DrawingPage>();
        builder.Services.AddTransientPopup<ColorPopup, ColorPopupViewModel>();

      
         return builder.Build();
         
    }

    private static IServiceCollection ConfigureRefit(this IServiceCollection services)
    {
        services
            .AddRefitClient<IAuthApi>(s => GetRefitSettings(s, false))
            .ConfigureHttpClient(SetHttpClient);
        
        services
            .AddRefitClient<IUserApi>(s => GetRefitSettings(s, true))
            .ConfigureHttpClient(SetHttpClient);
        return services;
    }
    

   private static RefitSettings GetRefitSettings( IServiceProvider provider , bool needsAuth)
    {
        return new RefitSettings()
        {
            
            HttpMessageHandlerFactory = () => 
            {
                var platformHandler = CreatePlatformHandler(); 

                if (needsAuth) 
                {
                    var authHandler = provider.GetRequiredService<AuthHeaderHandler>();
                    authHandler.InnerHandler = platformHandler;
                    return authHandler;
                }
                return platformHandler;
            }
        };


    }
   
    private static HttpMessageHandler CreatePlatformHandler()
    {
#if DEBUG
#if ANDROID
        return new Xamarin.Android.Net.AndroidMessageHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
#elif IOS
        return new NSUrlSessionHandler
        {
            TrustOverrideForUrl = (_, _, _) => true
        };
#else
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
#endif
#else
    #if ANDROID
        return new Xamarin.Android.Net.AndroidMessageHandler();
    #elif IOS
        return new NSUrlSessionHandler();
    #else
        return new HttpClientHandler();
    #endif
#endif
    }

    static void SetHttpClient(HttpClient client)
    {
         client.BaseAddress = new Uri(ApiConfig.BaseUrl);
         client.Timeout = TimeSpan.FromSeconds(ApiConfig.RequestTimeoutSeconds);
    }
}