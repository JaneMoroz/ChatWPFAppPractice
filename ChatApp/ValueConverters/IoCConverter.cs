using System;
using System.Diagnostics;
using System.Globalization;
using static ChatApp.DI;

namespace ChatApp
{
    /// <summary>
    /// Converts a string name to a service pulled from the IoC container
    /// </summary>
    public class IoCConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page
            switch ((string)parameter)
            {
                case nameof(ApplicationViewModel):
                    return ViewModelApplication;

                default:
                    Debugger.Break();
                    return null;  
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
