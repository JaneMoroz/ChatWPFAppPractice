using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatApp
{
    /// <summary>
    /// Creates a clipping region from the parent <see cref="Border"/> <see cref="CornerRadius"/>
    /// </summary>
    public class ClipFromBorderProperty : BaseAttachedProperty<ClipFromBorderProperty, bool>
    {
        #region Private Members

        /// <summary>
        /// Called when the parent border first loads
        /// </summary>
        private RoutedEventHandler _border_Loaded;

        /// <summary>
        /// Called when the border size changes
        /// </summary>
        private SizeChangedEventHandler _border_SizeChanged;

        #endregion

        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get self
            var self = (sender as FrameworkElement);

            // Check we have a parent Border
            if (!(self.Parent is Border border))
            {
                Debugger.Break();
                return;
            }

            // Setup loaded event
            _border_Loaded = (s1, e1) => Border_OnChange(s1, e1, self);

            // Setup size changed event
            _border_SizeChanged = (s1, e1) => Border_OnChange(s1, e1, self);

            // If true, hook into events
            if ((bool)e.NewValue)
            {
                border.Loaded += _border_Loaded;
                border.SizeChanged += _border_SizeChanged;
            }
            // Otherwise, unhook
            else
            {
                border.Loaded -= _border_Loaded;
                border.SizeChanged -= _border_SizeChanged;
            }
        }

        /// <summary>
        /// Called when the border is loaded and changed size
        /// </summary>
        /// <param name="sender">The border</param>
        /// <param name="e"></param>
        /// <param name="child">The child element (our selves)</param>
        private void Border_OnChange(object sender, RoutedEventArgs e, FrameworkElement child)
        {
            // Get border
            var border = (Border)sender;

            // Check we have an actual size
            if (border.ActualWidth == 0 && border.ActualHeight == 0)
                return;

            // Setup the new child clipping area
            var rect = new RectangleGeometry();

            // Match the corner radius with the borders corner radius
            rect.RadiusX = rect.RadiusY = Math.Max(0, border.CornerRadius.TopLeft - (border.BorderThickness.Left * 0.5));

            // Set rectangle size to match child's actual size
            rect.Rect = new Rect(child.RenderSize);

            // Assign clipping area to the child
            child.Clip = rect;
        }
    }
}
