using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using PetVideoApp.Pages;
using Refit;

namespace PetVideoApp.ViewModels;

public partial class BaseViewModel : ObservableValidator
{
    private bool _isConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

    [ObservableProperty] private bool isBusy;


    protected async Task MakeApiCall(Func<Task> apiCall)
    {
        try
        {
            if (!_isConnected)
            {
                await ToastAsync("No internet connection");
                return;
            }

            IsBusy = true;
            await apiCall.Invoke();
        }
        catch (ApiException apiEx) when (apiEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await ToastAsync(apiEx.Message);
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        catch (ApiException apiEx) 
        {
            await ToastAsync(apiEx.Message);
        }
        catch (Exception ex)
        {
            await ToastAsync("App error");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    protected async Task ToastAsync(string message) =>
        await Toast.Make(message).Show();
}