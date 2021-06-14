using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    /// <summary>
    /// The design-time data for a <see cref="ContactsListItemViewModel"/>
    /// </summary>
    public class ContactListItemDesignModel : ContactsListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ContactListItemDesignModel Instance => new ContactListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactListItemDesignModel()
        {
            Initials = "LM";
            Username = "Luke";
            ProfilePictureRGB = "3099c5";
        }

        #endregion
    }
}
