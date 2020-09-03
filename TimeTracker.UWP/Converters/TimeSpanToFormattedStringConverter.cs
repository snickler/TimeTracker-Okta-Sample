using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Windows.UI.Xaml.Data;

namespace TimeTracker.UWP.Converters
{
   public class TimeSpanToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan timeSpan) return String.Format("{0:hh\\:mm\\:ss}", timeSpan);
            return String.Format("{0:hh\\:mm\\:ss}", TimeSpan.Zero);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
