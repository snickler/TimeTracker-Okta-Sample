using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TimeTracker.UWP.Converters
{
   public class BooleanToTimerStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolState) return boolState ? Symbol.Stop : Symbol.Play;
            return System.Convert.ToBoolean(value) ? Symbol.Stop : Symbol.Play;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
