
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
       

        private string? _userName;
        private string? _password;
       

        public string? UserName
        {
            get => _userName;

            set
            {
                _userName = value;
                Preferences.Set("UserName", value);
                OnPropertyChanged(nameof(UserName));
            }

        }

        public string? Password
        {
            get => _password;

            set
            {
                _password = value;
                Preferences.Set("Password",value);
                OnPropertyChanged(nameof(Password));    
            }

        }

      


        public void OnPropertyChanged([CallerMemberName] string name = "") =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
