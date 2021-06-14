using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp
{
    /// <summary>
    /// A view model for each contact in contact list
    /// </summary>
    public class ContactsListItemViewModel
    {
        #region Public Properties

        /// <summary>
        ///  The display username of this contact
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// </summary>
        public string ProfilePictureRGB { get; set; }

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Opens the profile of selected user
        /// </summary>
        public ICommand OpenProfileCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactsListItemViewModel()
        {
            // Create commands
            OpenProfileCommand = new RelayCommand(OpenProfile);
        }

        #endregion

        #region Command Methods

        public void OpenProfile()
        {
           
        }

        #endregion
    }
}
