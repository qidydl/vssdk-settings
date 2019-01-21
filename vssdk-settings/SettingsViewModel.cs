using System.Windows.Input;

namespace vssdk_settings
{
    public class SettingsViewModel
    {
        public SettingsViewModel(SettingsGroup settingsGroup)
        {
            SettingsGroup = settingsGroup;

            AddSettingsCommand = new DelegateCommand(AddSettings);
            DeleteSettingsCommand = new ConditionalDelegateCommand(SettingsGroup, DeleteSettings, AreSettingsSelected);
            CopySettingsCommand = new ConditionalDelegateCommand(SettingsGroup, CopySettings, AreSettingsSelected);
        }

        public SettingsGroup SettingsGroup { get; }

        public bool AreSettingsSelected() => SettingsGroup.CurrentSettings != null;

        #region Add Settings

        public ICommand AddSettingsCommand { get; }

        public void AddSettings(object parameter)
        {
            var newSettings = new Settings
            {
                Name = "New Settings"
            };

            SettingsGroup.Settings.Add(newSettings);

            SettingsGroup.CurrentSettings = newSettings;
        }

        #endregion Add Settings

        #region Delete Settings

        public ICommand DeleteSettingsCommand { get; }

        public void DeleteSettings(object parameter)
        {
            var toRemove = SettingsGroup.CurrentSettings;
            if (toRemove != null)
            {
                SettingsGroup.CurrentSettings = null;
                SettingsGroup.Settings.Remove(toRemove);
            }
        }

        #endregion Delete Settings

        #region Copy Settings

        public ICommand CopySettingsCommand { get; }

        public void CopySettings(object parameter)
        {
            var toCopy = SettingsGroup.CurrentSettings;
            if (toCopy != null)
            {
                var newSettings = new Settings
                {
                    Name = toCopy.Name + " Copy",
                    Number = toCopy.Number
                };

                SettingsGroup.Settings.Add(newSettings);
                SettingsGroup.CurrentSettings = newSettings;
            }
        }

        #endregion Copy Settings
    }
}