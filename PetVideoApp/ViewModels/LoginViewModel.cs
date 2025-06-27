using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PetVideoApp.Pages;
using PetVideoApp.Services;
using CommunityToolkit.Maui.Alerts;
using PetVideoApp.APIs;

namespace PetVideoApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthApi _authApi;
    private readonly ITokenStorage _tokenStorage;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(5, ErrorMessage =  "Username must be at least 5 characters long.")]
    [MaxLength(40, ErrorMessage =  "Username must be less than 40 characters long.")]
    private string _emailOrUsername = string.Empty;
    
    [ObservableProperty] 
    [NotifyDataErrorInfo]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [MaxLength(20,ErrorMessage = "Password must be  less than 20 characters long.")]
    private string _password = string.Empty;
    
    
    public LoginViewModel(IAuthApi authApi, ITokenStorage tokenStorage)
    {
        _authApi = authApi;
        _tokenStorage =  tokenStorage;
    }

    [RelayCommand]
    private async Task NavigateToRegisterPageAsync()
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private bool CanLogin => !HasErrors &&  !IsBusy;
    
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        await MakeApiCall(async () =>
        {
            var result = await _authApi.LoginAsync(new(EmailOrUsername, Password));

            await _tokenStorage.SetTokensAsync(result.AccessToken, result.RefreshToken);
            await Shell.Current.GoToAsync("//MainPage");
        });
    }
    
}