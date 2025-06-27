using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if ANDROID
using Android.Content.Res;
#endif

namespace PetVideoApp.Controls;

public class EntryMD : Entry
{
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        RemoveUnderLine();
#if ANDROID
        if (Handler is IEntryHandler entryHandler) entryHandler.PlatformView?.SetPadding(40, 0, 0, 0);
#endif
    }


    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == BackgroundColorProperty.PropertyName) RemoveUnderLine();
    }

    private void RemoveUnderLine()
    {
        var effBackgroundColor = (Color)GetValue(BackgroundColorProperty);
#if ANDROID
        if (Handler is IEntryHandler entryHandler)
        {
            if (effBackgroundColor == null)
                entryHandler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
            else
                entryHandler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(effBackgroundColor.ToPlatform());
        }

#endif
    }
}