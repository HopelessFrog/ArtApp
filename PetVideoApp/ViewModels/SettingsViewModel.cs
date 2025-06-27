using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PetVideoApp.Models;
using PetVideoApp.Pages;
using PetVideoApp.Services;
using CommunityToolkit.Maui.Alerts;
using PetVideoApp.APIs;
using PetVideoApp.Models.DTOs;

namespace PetVideoApp.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly ITokenStorage _tokenStorage;
    private readonly IUserApi _userApi;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(Initial))]
    private string _name = "Test";

    public string Initial => Name.Length > 0 ? Name[0].ToString().ToUpper() : "";

    public SettingsViewModel(ITokenStorage tokenStorage, IUserApi userApi)
    {
        _tokenStorage = tokenStorage;
        _userApi = userApi;
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        var newPassword = await Shell.Current.DisplayPromptAsync("Change Password",
            "Please enter a new Password to Change", "Change Password");
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            await ToastAsync("Invalid Password");
            return;
        }

        await MakeApiCall(async () =>
        {
            var result = await _userApi.ChangePassword(newPassword);
           

            await ToastAsync("Password Changed Successfully");
            await _tokenStorage.SetTokensAsync(result.AccessToken, result.RefreshToken);
        });
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await MakeApiCall(async () =>
        {
            var refreshToken = await _tokenStorage.GetRefreshTokenAsync();

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                try
                {
                    await _userApi.LogoutAsync(refreshToken);
                }
                catch (Exception)
                { }
                finally
                {
                    await _tokenStorage.ClearTokensAsync();
                }
            }

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        });
    }
}