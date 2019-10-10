using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinApp
{
    public class GuidToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Guid.Empty.ToString();
            else
                return ((Guid)value).ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
