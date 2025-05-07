using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public  class RoleDto : INotifyPropertyChanged
    {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("roleName")]
        public string RoleName { get; set; }


        bool? _isCheckBoxChecked;
        public bool? IsCheckBoxChecked
        {
            get => _isCheckBoxChecked;
            set
            {
                _isCheckBoxChecked = value;
                OnPropertyChanged(nameof(IsCheckBoxChecked));
            }
        }

        bool? _checkBoxEnabled;
        public bool? CheckBoxEnabled
        {
            get => _checkBoxEnabled;
            set
            {
                _checkBoxEnabled = value;
                OnPropertyChanged(nameof(CheckBoxEnabled));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
