using CommunityToolkit.Maui.Views;
using PetVideoApp.Popups.PopupViewModels;

namespace PetVideoApp.Popups;

public partial class ColorPopup : Popup<Color>
{
    public ColorPopup(ColorPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}