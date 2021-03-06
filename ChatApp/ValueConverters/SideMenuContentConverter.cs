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
    /// A converter that takes a <see cref="SideMenuContent"/> and converts it to the 
    /// correct UI element
    /// </summary>
    public class SideMenuContentConverter : BaseValueConverter<SideMenuContentConverter>
    {
        #region Protected Members

        /// <summary>
        /// An instance of the current chat list control
        /// </summary>
        protected ChatListControl _chatListControl = new ChatListControl();

        /// <summary>
        /// An instance of the current chat list control
        /// </summary>
        protected ContactsListControl _contactListControl = new ContactsListControl();

        #endregion

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the side menu type
            var sideMenuType = (SideMenuContent)value;

            // Switch based on type
            switch (sideMenuType)
            {
                // Chat 
                case SideMenuContent.Chat:
                    return _chatListControl;

                // Contacts 
                case SideMenuContent.Contacts:
                    return _contactListControl;

                // Unknown
                default:
                    return "No UI yet, sorry :)";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
