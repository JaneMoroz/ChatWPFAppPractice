using ChatApp.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    /// <summary>
    /// A converter that takes in a <see cref="BaseViewModel"/>and returns the specific UI control 
    /// that should bind to that type of ViewModel
    /// </summary>
    public class PopupContentConverter : BaseValueConverter<PopupContentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChatAttachmentPopupViewModel basePopup)
                return new VerticalMenu { DataContext = basePopup.Content };

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
