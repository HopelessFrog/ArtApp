using System.Globalization;

namespace PetVideoApp.Converters;

public class ReactionSelectedFontConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 2)
            return FontAttributes.None; 

        var reactionSource = values[0] as string;
        var myReaction = values[1] as string;

        if (!string.IsNullOrEmpty(reactionSource) && !string.IsNullOrEmpty(myReaction) && reactionSource == myReaction)
        {
            return FontAttributes.Bold; 
        }

        return FontAttributes.None;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

  
}