using System.Windows.Controls;

namespace vssdk_settings
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl(SettingsGroup settingsGroup)
        {
            InitializeComponent();
            DataContext = ViewModel = new SettingsViewModel(settingsGroup);
        }

        public SettingsViewModel ViewModel { get; }
    }
}