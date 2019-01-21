using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace vssdk_settings
{
    public class Settings : INotifyPropertyChanged
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

        /// <summary>
        /// Unique ID for a particular settings instance, used to track active settings even when renamed.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        private string _name;

        public string Name { get => _name; set => Set(ref _name, value); }

        private int _number;

        public int Number { get => _number; set => Set(ref _number, value); }
    }
}