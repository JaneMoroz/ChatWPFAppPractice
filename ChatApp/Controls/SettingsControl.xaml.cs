using System.ComponentModel;
using System.Windows.Controls;
using static ChatApp.DI;


namespace ChatApp
{
    /// <summary>
    /// Логика взаимодействия для SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();

            // If we are in design mode...
            if (DesignerProperties.GetIsInDesignMode(this))
                // Create new instance of settings view model
                DataContext = new SettingsViewModel();
            else
                DataContext = ViewModelSettings;
        }
    }
}
