using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    /// <summary>
    /// The design-time data for a <see cref="ContactProfileViewModel"/>
    /// </summary>
    public class ContactProfileDesignModel : ContactProfileViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ContactProfileDesignModel Instance => new ContactProfileDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactProfileDesignModel()
        {
            FirstName = "Jane";
            LastName = "Moroz";
            Username = "moroz";
            Email = "contact@moroz.com";
        }

        #endregion
    }
}
