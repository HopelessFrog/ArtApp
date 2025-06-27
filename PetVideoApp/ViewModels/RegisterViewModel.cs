using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PetVideoApp.Services;
using CommunityToolkit.Maui.Alerts;
using PetVideoApp.APIs;
using System.ComponentModel.DataAnnotations;
using PetVideoApp.Pages;

namespace PetVideoApp.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthApi _authApi;
    private readonly ITokenStorage _tokenStorage;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
    [MaxLength(20, ErrorMessage = "Username must be less than 20 characters long")]
    private string _username = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [MaxLength(20, ErrorMessage = "Password must be less than 20 characters long")]
    private string _password = string.Empty;

    public RegisterViewModel(IAuthApi authApi, ITokenStorage tokenStorage)
    {
        _authApi = authApi;
        _tokenStorage = tokenStorage;
    }

    private bool CanRegister => !HasErrors && !IsBusy;

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task RegisterAsync()
    {
        await MakeApiCall(async () =>
        {
            var result = await _authApi.RegisterAsync(new(Username, Email, Password));

            await Toast.Make("Registration successful!").Show();
            await _tokenStorage.SetTokensAsync(result.AccessToken, result.RefreshToken);

            await Shell.Current.GoToAsync("//MainPage");
        });
    }

    [RelayCommand]
    private async Task NavigateToLoginPage()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}