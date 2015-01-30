using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Wpf.Converters;
using Cirrious.MvvmCross.Plugins.Visibility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LogViewer.Wpf.NativeConverters
{
    #region Converter_ColorToBrush

    public class ColorToBrushConverter : MvxValueConverter<Color, SolidColorBrush>
    {
        protected override SolidColorBrush Convert(Color value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush(value);
        }
    }

    public class NativeColorToBrushConverter : MvxNativeValueConverter<ColorToBrushConverter> { }

    #endregion

    #region Converter_StringToColor

    public class StringToColorConverter : MvxValueConverter<String, Color?>
    {
        protected override Color? Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ColorConverter.ConvertFromString(value) as Color?;
        }

        protected override string ConvertBack(Color? value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!value.HasValue)
                return String.Empty;

            return value.ToString();
        }
    }

    public class NativeStringToColorConverter : MvxNativeValueConverter<StringToColorConverter> { }

    #endregion

    #region Converter_MenuColumnMinWidth

    public class MenuColumnMinWidthConverter : MvxValueConverter<Visibility, Double>
    {
        protected override double Convert(Visibility value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == Visibility.Visible)
            {
                var minWidth = Double.Parse(parameter.ToString());
                return minWidth;
            }

            return 0;
        }
    }

    public class NativeMenuColumnMinWidthConverter : MvxNativeValueConverter<MenuColumnMinWidthConverter> { } 

    #endregion

    #region Converter_FileSizeFormatting

    public class FileSizeFormattingConverter : MvxValueConverter<Int64, String>
    {
        protected override string Convert(long value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //convert to mb, kb, b
            Double actual = value;
            ByteMultiples extension = ByteMultiples.Bytes;

            while(actual > 1024 || extension == ByteMultiples.Gigabytes)
            {
                actual = actual / 1024;
                extension++;
            }
            
            actual = Math.Round(actual, 2);

            return String.Format("{0} {1}", actual, _friendlyNames[extension]);
        }

        private static Dictionary<ByteMultiples, String> _friendlyNames = new Dictionary<ByteMultiples, string>()
        {
            {ByteMultiples.Bytes, "Bytes"},
            {ByteMultiples.Kilobytes, "KB"},
            {ByteMultiples.Megabytes, "MB"},
            {ByteMultiples.Gigabytes, "GB"}
        };

        private enum ByteMultiples
        {
            Bytes = 0,
            Kilobytes = 1,
            Megabytes = 2,

            Gigabytes = 3
        }
    }

    public class NativeFileSizeFormattingConverter : MvxNativeValueConverter<FileSizeFormattingConverter> { }

    #endregion

    public class NativeVisibilityConverter : MvxNativeValueConverter<MvxVisibilityValueConverter> { }
    public class NativeInvertedVisibilityConverter : MvxNativeValueConverter<MvxInvertedVisibilityValueConverter> { }
}
