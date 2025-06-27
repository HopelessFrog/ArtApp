using System.Globalization;

namespace PetVideoApp.Converters;

public class ReactionSelectedConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
     {
         if (values == null || values.Length < 2)
             return Color.FromArgb("#40000000"); 
 
         var reactionSource = values[0] as string;
         var myReaction = values[1] as string;
 
         if (!string.IsNullOrEmpty(reactionSource) && !string.IsNullOrEmpty(myReaction) && reactionSource == myReaction)
         {
             return new SolidColorBrush(Color.FromArgb("#80808080")); 
         }
 
         return new  SolidColorBrush(Color.FromArgb("#60000000")); 
     }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}