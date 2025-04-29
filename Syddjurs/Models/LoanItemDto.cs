using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public class LoanItemDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }       
        public string Lender { get; set; }        
     


        string _note;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        int _availabeNumber;
        public int AvailabeNumber
        {
            get => _availabeNumber;
            set
            {
                _availabeNumber = value;
                OnPropertyChanged(nameof(AvailabeNumber));
            }
        }

        int _number;
        public int Number
        {
            get => _number;
            set
           {
                _number = value;
                OnPropertyChanged(nameof(Number));
                ValidateNumber();
            }
        }

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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // Property to control visibility of the error label
        public bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set
            {
                _isErrorVisible = value;
                OnPropertyChanged(nameof(IsErrorVisible));
            }
        }

        private void ValidateNumber()
        {
            if (Number <= 0)
            {
                ErrorMessage = "Antal skal være 1 minimum";
                IsErrorVisible = true;
                return;
            }

            if (Number > AvailabeNumber)
            {
                ErrorMessage = "Antal kan ikke være større end antal på lage";
                IsErrorVisible = true;
                return;
            }

           
                ErrorMessage = string.Empty;
                IsErrorVisible = false;
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
