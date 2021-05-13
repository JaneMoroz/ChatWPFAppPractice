using ChatApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ChatApp
{
    /// <summary>
    /// Base page for all pages to gain base fynctionality
    /// </summary>
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new()
    {
        #region Private Member

        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        private VM _viewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The animation the play when the page is first loaded
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// The animation the play when the page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

        /// <summary>
        /// The time any slide animation takes to complete
        /// </summary>
        public float SlideSeconds { get; set; } = 0.8f;

        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        public VM ViewModel
        {
            get { return _viewModel; }
            set
            {
                // If nothing has changed, return
                if (_viewModel == value)
                    return;

                // Update the value
                _viewModel = value;

                // Set the data context for this page
                DataContext = _viewModel;
            }
        }

        #endregion

            #region Constructor

        public BasePage()
        {
            // If we are animating in, hide to begin with
            if (PageLoadAnimation != PageAnimation.None)
            {
                Visibility = Visibility.Collapsed;
            }

            // Listen out for the page loading
            Loaded += BasePage_Loaded;

            // Create a default view model
            ViewModel = new VM();
        }

        #endregion

        /// <summary>
        /// Once the page is loaded, perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Animation Load / Unload

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            // Animate the page in
            await AnimateIn();
        }

        /// <summary>
        /// Animates in this page
        /// </summary>
        /// <returns></returns>
        public async Task AnimateIn()
        {
            // Make sure we have smth to do
            if (PageLoadAnimation == PageAnimation.None)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:

                    // Start the animation
                    await this.SlideAndFadeInFromRight(SlideSeconds);

                    break;
            }
        }

        /// <summary>
        /// Animates the page out
        /// </summary>
        /// <returns></returns>
        public async Task AnimateOut()
        {
            // Make sure we have something to do
            if (PageUnloadAnimation == PageAnimation.None)
                return;

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToLeft:

                    // Start the animation
                    await this.SlideAndFadeOutToLeft(SlideSeconds);

                    break;
            }
        }

        #endregion
    }
}
