using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    /// <summary>
    ///  A view model for the overview contact list
    /// </summary>
    public class ContactsListViewModel
    {
        /// <summary>
        ///  The contacts list for the list
        /// </summary>
        public List<ContactsListItemViewModel> Contacts { get; set; }
    }
}
