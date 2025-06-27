using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PetVideoApp.Popups.PopupViewModels;

public partial class ColorPopupViewModel : ObservableObject, IQueryAttributable
{
    private readonly IPopupService _popupService;

    [ObservableProperty][NotifyPropertyChangedFor(nameof(Color))] private int _red;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(Color))] private int _green;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(Color))] private int _blue;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(Color))] private float _alpha;
    
    public Color Color => Color.FromRgba(Red/255.0, Green / 255.0, Blue / 255.0, Alpha);
    
    public ColorPopupViewModel(IPopupService popupService)
    {
        _popupService = popupService;
    }
    
    [RelayCommand]
    private async Task ApplyColor()
    {
       await  _popupService.ClosePopupAsync(Shell.Current, Color);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Red = (int)((float)query[nameof(ColorPopupViewModel.Red)] * 255);
        Green = (int)((float)query[nameof(ColorPopupViewModel.Green)] * 255);
        Blue = (int)((float)query[nameof(ColorPopupViewModel.Blue)] * 255);
        Alpha = (float)query[nameof(ColorPopupViewModel.Alpha)];
    }
}