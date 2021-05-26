using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ChatApp
{
    /// <summary>
    /// The dock position of the window
    /// </summary>
    public enum WindowDockPosition
    {
        /// <summary>
        /// Not docked
        /// </summary>
        Undocked = 0,
        /// <summary>
        /// Docked to the left of the screen
        /// </summary>
        Left = 1,
        /// <summary>
        /// Docked to the right of the screen
        /// </summary>
        Right = 2,
        /// <summary>
        /// Docked to the top/bottom of the screen
        /// </summary>
        TopBottom = 3,
        /// <summary>
        /// Docked to the top-left of the screen
        /// </summary>
        TopLeft = 4,
        /// <summary>
        /// Docked to the top-right of the screen
        /// </summary>
        TopRight = 5,
        /// <summary>
        /// Docked to the bottom-left of the screen
        /// </summary>
        BottomLeft = 6,
        /// <summary>
        /// Docked to the bottom-right of the screen
        /// </summary>
        BottomRight = 7,
    }

    /// <summary>
    /// Fixes the issue with Windows of Style <see cref="WindowStyle.None"/> covering the taskbar
    /// </summary>
    public class WindowResizer
    {
        #region Private Members

        /// <summary>
        /// The window to handle the resizing for
        /// </summary>
        private Window _window;

        /// <summary>
        /// The last calculated available screen size
        /// </summary>
        private Rect _screenSize = new Rect();

        /// <summary>
        /// How close to the edge the window has to be to be detected as at the edge of the screen
        /// </summary>
        private int _edgeTolerance = 8;

        /// <summary>
        /// The transform matrix used to convert WPF sizes to screen pixels
        /// </summary>
        private DpiScale? _monitorDpi;

        /// <summary>
        /// The last screen the window was on
        /// </summary>
        private IntPtr _lastScreen;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition _lastDock = WindowDockPosition.Undocked;

        #endregion

        #region Dll Imports

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        #endregion

        #region Public Events

        /// <summary>
        /// Called when the window dock position changes
        /// </summary>
        public event Action<WindowDockPosition> WindowDockChanged = (dock) => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// The size and position of the current monitor the window is on
        /// </summary>
        public Rectangle CurrentMonitorSize { get; set; } = new Rectangle();

        /// <summary>
        /// The size and position of the current screen in relation to the multi-screen desktop
        /// For example a second monitor on the right will have a Left position of
        /// the X resolution of the screens on the left
        /// </summary>
        public Rect CurrentScreenSize => _screenSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window">The window to monitor and correctly maximize</param>
        /// <param name="adjustSize">The callback for the host to adjust the maximum available size if needed</param>
        public WindowResizer(Window window)
        {
            _window = window;

            // Listen out for source initialized to setup
            _window.SourceInitialized += Window_SourceInitialized;

            // Monitor for edge docking
            _window.SizeChanged += Window_SizeChanged;
            _window.LocationChanged += Window_LocationChanged;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize and hook into the windows message pump
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            // Get the handle of this window
            var handle = (new WindowInteropHelper(_window)).Handle;
            var handleSource = HwndSource.FromHwnd(handle);

            // If not found, end
            if (handleSource == null)
                return;

            // Hook into it's Windows messages
            handleSource.AddHook(WindowProc);
        }


        #endregion

        #region Edge Docking

        /// <summary>
        /// Monitor for moving of the window and constantly check for docked positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Window_SizeChanged(null, null);
        }

        /// <summary>
        /// Monitors for size changes and detects if the window has been docked (Aero snap) to an edge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Cannot calculate size until we know monitor scale
            if (_monitorDpi == null)
                return;
            // Get window rectangle
            var top = _window.Top;
            var left = _window.Left;
            var bottom = top + _window.Height;
            var right = left + _window.Width;

            // Get window position/size in device pixels
            var windowTopLeft = new Point(left * _monitorDpi.Value.DpiScaleX, top * _monitorDpi.Value.DpiScaleX);
            var windowBottomRight = new Point(right * _monitorDpi.Value.DpiScaleX, bottom * _monitorDpi.Value.DpiScaleX);

            // Check for edges docked
            var edgedTop = windowTopLeft.Y <= (_screenSize.Top + _edgeTolerance) && windowTopLeft.Y >= (_screenSize.Top - _edgeTolerance);
            var edgedLeft = windowTopLeft.X <= (_screenSize.Left + _edgeTolerance) && windowTopLeft.X >= (_screenSize.Left - _edgeTolerance);
            var edgedBottom = windowBottomRight.Y >= (_screenSize.Bottom - _edgeTolerance) && windowBottomRight.Y <= (_screenSize.Bottom + _edgeTolerance);
            var edgedRight = windowBottomRight.X >= (_screenSize.Right - _edgeTolerance) && windowBottomRight.X <= (_screenSize.Right + _edgeTolerance);

            // Get docked position
            var dock = WindowDockPosition.Undocked;

            // Left docking
            if (edgedTop && edgedBottom && edgedLeft)
                dock = WindowDockPosition.Left;
            // Right docking
            else if (edgedTop && edgedBottom && edgedRight)
                dock = WindowDockPosition.Right;
             // Top/bottom
            else if (edgedTop && edgedBottom)
                dock = WindowDockPosition.TopBottom;
            // Top-left
            else if (edgedTop && edgedLeft)
                dock = WindowDockPosition.TopLeft;
            // Top-right
            else if (edgedTop && edgedRight)
                dock = WindowDockPosition.TopRight;
            // Bottom-left
            else if (edgedBottom && edgedLeft)
                dock = WindowDockPosition.BottomLeft;
            // Bottom-right
            else if (edgedBottom && edgedRight)
                dock = WindowDockPosition.BottomRight;
            // None
            else
                dock = WindowDockPosition.Undocked;

            // If dock has changed
            if (dock != _lastDock)
                // Inform listeners
                WindowDockChanged(dock);

            // Save last dock position
            _lastDock = dock;
        }

        #endregion

        #region Windows Proc

        /// <summary>
        /// Listens out for all windows messages for this window
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                // Handle the GetMinMaxInfo of the Window
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        #endregion

        /// <summary>
        /// Get the min/max window size for this window
        /// Correctly accounting for the taskbar size and position
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            // Get the point position to determine what screen we are on
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            // Now get the current screen
            var lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            var lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

            // Try and get the current screen information
            var lCurrentScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
                return;

            // Try and get the primary screen information
            var lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
                return;

            // If this has changed from the last one, update the transform
            if (lCurrentScreen != _lastScreen || _monitorDpi == null)
                _monitorDpi = VisualTreeHelper.GetDpi(_window);

            // Store last know screen
            _lastScreen = lCurrentScreen;

            // Get work area sizes and rations
            var currentX = lCurrentScreenInfo.rcWork.Left - lCurrentScreenInfo.rcMonitor.Left;
            var currentY = lCurrentScreenInfo.rcWork.Top - lCurrentScreenInfo.rcMonitor.Top;
            var currentWidth = (lCurrentScreenInfo.rcWork.Right - lCurrentScreenInfo.rcWork.Left);
            var currentHeight = (lCurrentScreenInfo.rcWork.Bottom - lCurrentScreenInfo.rcWork.Top);
            var currentRatio = (float)currentWidth / (float)currentHeight;

            var primaryX = lPrimaryScreenInfo.rcWork.Left - lPrimaryScreenInfo.rcMonitor.Left;
            var primaryY = lPrimaryScreenInfo.rcWork.Top - lPrimaryScreenInfo.rcMonitor.Top;
            var primaryWidth = (lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left);
            var primaryHeight = (lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top);
            var primaryRatio = (float)primaryWidth / (float)primaryHeight;


            // Get min/max structure to fill with information
            var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // NOTE: rcMonitor is the monitor size
            //       rcWork is the available screen size (so the area inside the taskbar start menu for example)

            // Size size limits (used by Windows when maximized)
            // relative to 0,0 being the current screens top-left corner
            //
            //  - Position
            lMmi.ptMaxPosition.X = currentX;
            lMmi.ptMaxPosition.Y = currentY;
            //
            // - Size
            lMmi.ptMaxSize.X = currentWidth;
            lMmi.ptMaxSize.Y = currentHeight;

            //
            // BUG: 
            // NOTE: I've noticed a bug which I think is Windows itself
            //       If your non-primary monitor has a greater width than your primary
            //       (or possibly due to the screen ratio's being different)
            //       then setting the max X on the monitor to the correct value causes
            //       it to scale wrong. 
            //
            //       The fix seems to be to set the max width only (height is fine)
            //       to that of the primary monitor, not the current monitor
            //        
            //       However, 1 pixel different and the size goes totally wrong again
            //       so the fix doesn't work when the taskbar is on the left or right
            //

            // Set monitor size
            CurrentMonitorSize = new Rectangle(lMmi.ptMaxPosition.X, lMmi.ptMaxPosition.Y, lMmi.ptMaxSize.X + lMmi.ptMaxPosition.X, lMmi.ptMaxSize.Y + lMmi.ptMaxPosition.Y);

            // Set min size
            var minSize = new Point(_window.MinWidth * _monitorDpi.Value.DpiScaleX, _window.MinHeight * _monitorDpi.Value.DpiScaleX);

            lMmi.ptMinTrackSize.X = (int)minSize.X;
            lMmi.ptMinTrackSize.Y = (int)minSize.Y;

            // Store new size
            _screenSize = new Rect(lCurrentScreenInfo.rcWork.Left, lCurrentScreenInfo.rcWork.Top, lMmi.ptMaxSize.X, lMmi.ptMaxSize.Y);

            // Now we have the max size, allow the host to tweak as needed
            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        /// <summary>
        /// Gets the current cursor position in screen coordinates relative to an entire multi-desktop position
        /// </summary>
        /// <returns></returns>
        public Point GetCursorPosition()
        {
            // Get mouse position
            GetCursorPos(out POINT lMousePosition);

            // Apply DPI scaling
            return new Point(lMousePosition.X / _monitorDpi.Value.DpiScaleX, lMousePosition.Y / _monitorDpi.Value.DpiScaleY);
        }
    }

    #region Dll Helper Structures

    enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public Rectangle rcMonitor = new Rectangle();
        public Rectangle rcWork = new Rectangle();
        public int dwFlags = 0;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int Left, Top, Right, Bottom;

        public Rectangle(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        /// <summary>
        /// x coordinate of point.
        /// </summary>
        public int X;
        /// <summary>
        /// y coordinate of point.
        /// </summary>
        public int Y;

        /// <summary>
        /// Construct a point of coordinates (x,y).
        /// </summary>
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    #endregion
}
