using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp.Core
{
    public class RelayCommand : ICommand
    {
        #region Private Members

        private Action _action;

        #endregion

        #region Public Events
        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> 
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion
        #region Constructor
        public RelayCommand(Action action)
        {
            _action = action;
        }

        #endregion

        /// <summary>
        /// A relay command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
