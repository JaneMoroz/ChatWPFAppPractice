using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static ChatApp.DI;
using static Dna.FrameworkDI;

namespace ChatApp
{
    /// <summary>
    /// A contact profile view model fore selected contact
    /// </summary>
    public class ContactProfileViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The text to show while loading text
        /// </summary>
        private string _loadingText = "...";

        #endregion

        #region Public Properties

        /// <summary>
        /// The current users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The current users last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The current users username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The current users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The text for the logout button
        /// </summary>
        public string DeleteContactButtonText { get; set; }

        #endregion

        #region Transactional Properties

        /// <summary>
        /// Indicates if the profile details are currently being loaded
        /// </summary>
        public bool ProfileLoading { get; set; }

        /// <summary>
        /// Indicates if the user is currently being deleted
        /// </summary>
        public bool DeletingUser { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to logout of the application
        /// </summary>
        public ICommand DeleteContactCommand { get; set; }

        /// <summary>
        /// Loads the profile data from the client data store
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactProfileViewModel()
        {
            // Create First Name
            FirstName = _loadingText;

            // Create Last Name
            LastName = _loadingText;

            // Create Username
            Username = _loadingText;

            // Create Email
            Email = _loadingText;

            // Create commands
            CloseCommand = new RelayCommand(Close);
            DeleteContactCommand = new RelayCommand(DeleteContact);
            LoadCommand = new RelayCommand(Load);

            // TODO: Get from localization
            DeleteContactButtonText = "Delete Contact";
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Closes the settings menu
        /// </summary>
        public void Close()
        {
            // Close profile
            ViewModelApplication.ContactProfileVisible = false;

        }

        /// <summary>
        /// Delete the contact from the list of contacts
        /// </summary>
        public void DeleteContact()
        {
            // Delete contact

        }

        /// <summary>
        /// Load the profile of selected contact
        /// </summary>
        public void Load()
        {
            // Load contact


        }
        #endregion
    }
}
