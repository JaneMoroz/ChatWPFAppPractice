using ChatApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// A view model for any popup menus
    /// </summary>
    public class BasePopupMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The background color of the bubble in ARGB value
        /// </summary>
        public string BubbleBackground { get; set; }

        /// <summary>
        /// The alignment of the bubble arrow
        /// </summary>
        public ElementHorizontalAlignment ArrowAlignment { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePopupMenuViewModel()
        {
            // Set default values
            // TODO: Move colors into Core and make use of it here
            BubbleBackground = "ffffff";
            ArrowAlignment = ElementHorizontalAlignment.Left;
        }

        #endregion
    }
}
