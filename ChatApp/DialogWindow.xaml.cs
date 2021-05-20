using ChatApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        #region Private Members

        /// <summary>
        /// The view model for this window
        /// </summary>
        private DialogWindowViewModel _viewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model for this window
        /// </summary>
        public DialogWindowViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                // Set new value
                _viewModel = value;

                // Update data context
                DataContext = _viewModel;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogWindow()
        {
            InitializeComponent();
        }

        #endregion
    }
}
