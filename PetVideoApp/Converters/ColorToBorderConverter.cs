using System.Globalization;

namespace PetVideoApp.Converters;

public class ColorToBorderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color selectedColor && parameter is string colorName)
        {
            var targetColor = colorName switch
            {
                "Black" => Colors.Black,
                "Red" => Colors.Red,
                "Blue" => Colors.Blue,
                "Green" => Colors.Green,
                "Yellow" => Colors.Yellow,
                "Orange" => Colors.Orange,
                "Purple" => Colors.Purple,
                "Pink" => Colors.Pink,
                _ => Colors.Transparent
            };

            // Сравниваем цвета (приблизительно, так как Colors.* могут иметь небольшие различия)
            return ColorsAreEqual(selectedColor, targetColor) ? Colors.White : Colors.Transparent;
        }

        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private bool ColorsAreEqual(Color color1, Color color2)
    {
        const float tolerance = 0.01f;
        return Math.Abs(color1.Red - color2.Red) < tolerance &&
               Math.Abs(color1.Green - color2.Green) < tolerance &&
               Math.Abs(color1.Blue - color2.Blue) < tolerance &&
               Math.Abs(color1.Alpha - color2.Alpha) < tolerance;
    }
}