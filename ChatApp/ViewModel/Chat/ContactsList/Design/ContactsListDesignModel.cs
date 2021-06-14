using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    /// <summary>
    /// The design-time data for a <see cref="ContactsListViewModel"/>
    /// </summary>
    public class ContactsListDesignModel : ContactsListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ContactsListDesignModel Instance => new ContactsListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactsListDesignModel()
        {
            Contacts = new List<ContactsListItemViewModel>
            {
                new ContactsListItemViewModel
                {
                    Username = "Luke",
                    Initials = "LM",
                    ProfilePictureRGB = "3099c5"
                },
                new ContactsListItemViewModel
                {
                    Username = "Jesse",
                    Initials = "JA",
                    ProfilePictureRGB = "fe4503"
                },
                new ContactsListItemViewModel
                {
                    Username = "Parnell",
                    Initials = "PL",
                    ProfilePictureRGB = "00d405"
                },
                new ContactsListItemViewModel
                {
                    Username = "Patricia",
                    Initials = "PM",
                    ProfilePictureRGB = "3099c5"
                },
                new ContactsListItemViewModel
                {
                    Username = "Mike",
                    Initials = "MA",
                    ProfilePictureRGB = "fe4503"
                },
            };
        }

        #endregion
    }
}
