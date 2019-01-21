using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;

namespace vssdk_settings
{
    public class SettingsGroup : UIElementDialogPage, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool Set<T>(ref T storage, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, newValue))
            {
                return false;
            }

            storage = newValue;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion INotifyPropertyChanged members

        [NonSerialized]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private ObservableCollection<Settings> _settings = new ObservableCollection<Settings>();

        [TypeConverter(typeof(SettingsConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ObservableCollection<Settings> Settings { get => _settings; set => Set(ref _settings, value); }

        private Guid _currentSettingsId = Guid.Empty;

        public Guid CurrentSettingsId
        {
            get => _currentSettingsId;
            set
            {
                Set(ref _currentSettingsId, value);
                // Changing ID implicitly changes current settings as well.
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSettings)));
            }
        }

        public Settings CurrentSettings
        {
            get => CurrentSettingsId == Guid.Empty ? null : Settings.SingleOrDefault(s => s.Id == CurrentSettingsId);
            set => CurrentSettingsId = value?.Id ?? Guid.Empty;
        }

        #region Visual Studio integration

        [NonSerialized]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private SettingsControl _uiElement = null;

        /// <summary>
        /// Gets the Windows Presentation Foundation (WPF) child element to be hosted inside the dialog page.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override UIElement Child
        {
            get
            {
                if (_uiElement == null)
                {
                    _uiElement = new SettingsControl(this);
                }
                return _uiElement;
            }
        }

        /// <summary>
        /// Custom converter used to aid with serialization and deserialization of project settings.
        /// </summary>
        public class SettingsConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string str)
                {
                    var settings = JsonConvert.DeserializeObject<List<Settings>>(str);
                    return new ObservableCollection<Settings>(settings);
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var settings = (ObservableCollection<Settings>)value;
                    return JsonConvert.SerializeObject(settings.ToList());
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        #endregion Visual Studio integration
    }
}