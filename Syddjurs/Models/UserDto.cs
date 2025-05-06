using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public  class UserDto : INotifyPropertyChanged
    {

        [JsonPropertyName("userName")]
        public string UserName { get; set; }


        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("roles")]
        public List<RoleDto> Roles { get; set; }


        bool _isSelected;       
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        bool _showInList;
        public bool ShowInList
        {
            get => _showInList;
            set
            {
                _showInList = value;
                OnPropertyChanged(nameof(ShowInList));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
