using CRMYourBankers.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CRMYourBankers.Converters
{
    public class EnumDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var type = value.GetType();
                var memInfo = type.GetMember(value.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(ZusUs), false);
                return (attributes.Length > 0) ? (ZusUs)attributes[0] : null;
            }
            return string.Format("{0}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
