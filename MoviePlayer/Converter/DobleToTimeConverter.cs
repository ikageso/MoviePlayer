using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MoviePlayer.Converter
{
    class DobleToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                return "0:00";

            if (value == null)
                return "";

            double pos = (double)value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, (int)pos);

            string format = string.Empty;

            if (ts.Hours == 0)
                format = string.Format("{0}:{1:00}", ts.Minutes, ts.Seconds);
            else
                format = string.Format("{0}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

            return format;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
