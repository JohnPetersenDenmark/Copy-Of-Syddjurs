using Microsoft.Maui.ApplicationModel.Communication;
using Syddjurs.Models;
using Syddjurs.Utilities;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Syddjurs.Pages;

public partial class RegisterUserPage : ContentPage, INotifyPropertyChanged
{
    bool _isError;
    public bool IsError
    {
        get => _isError;
        set
        {
            _isError = value;
            OnPropertyChanged(nameof(IsError));
        }
    }

    string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            ValidateEmail();
            OnPropertyChanged(nameof(Email));
        }
    }

    string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            ValidateUserName();
            OnPropertyChanged(nameof(UserName));
        }
    }

    string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            ValidatePassword();
            OnPropertyChanged(nameof(Password));
        }
    }

    public RegisterUserPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

	private async void SaveClicked(object sender, EventArgs e)
	{		
		var registerDto = new RegisterUserDto();

       // registerDto.UserName = UserName;
        registerDto.Password = Password;
        registerDto.Email = RegisterEmail.Text;

        await RegisterAsync(registerDto);

    }

    private  void  ValidateUserName()
	{
		if (UserName == "")
		{
            ErrorMessage = "Brugernavn skal udfyldes";
            IsError = true; 
            return ;	
		}

        ErrorMessage = "";
        IsError = false;
        return ;
	}

    private void ValidateEmail()
    {
        if (RegisterEmail.Text == "")
        {
            ErrorMessage = "Email skal udfyldes";
            IsError = true;
            return;
        }

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if(! emailRegex.IsMatch(RegisterEmail.Text))
        {
            ErrorMessage = "Ugyldig email";
            IsError = true;
            return;
        }

        ErrorMessage = "";
        IsError = false;
        return;
    }

    private void ValidatePassword()
    {        
        if (Password == "")
        {
            ErrorMessage = "Kodeord skal udfyldes";
            IsError = true;
            return ;
        }

        ErrorMessage = "";
        IsError = false;

        return ;
    }
    public async Task RegisterAsync(RegisterUserDto dto)
    {
        var json = JsonSerializer.Serialize(dto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{EndpointSettings.ApiBaseUrl}/Login/register", content);
            var responsecontent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ErrorMessage = "Bruger blev oprettet";
                IsError = true;
                return; 
            }
         
            var errorObj = JsonSerializer.Deserialize<ErrorResponse>(responsecontent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ErrorMessage = string.Join("\n", errorObj.Errors); 
            IsError = true;

            return;

        }

        catch (Exception ex)
        {

        }
    }

  

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}