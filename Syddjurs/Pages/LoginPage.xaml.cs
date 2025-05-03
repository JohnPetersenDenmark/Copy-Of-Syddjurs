using System.Text.Json;
using System.Text;
using Syddjurs.Models;
using System.ComponentModel;
using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using Syddjurs.Utilities;

namespace Syddjurs.Pages;

public partial class LoginPage : ContentPage, INotifyPropertyChanged
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

    public LoginPage()
    {
        InitializeComponent();
        
        BindingContext = this;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        UserName = SecureStorage.GetAsync("userLogin").Result;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        UserName = "";
        Password = "";

    }

    private async void LoginClicked(object sender, EventArgs e)
    {
        var loginDto = new LoginDto();

        loginDto.UserName = UserName;
        loginDto.Password = Password;


        await LoginAsync(loginDto);

    }

    private void ValidateUserName()
    {
        if (UserName == "")
        {
            ErrorMessage = "Brugernavn skal udfyldes";
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
            return;
        }

        ErrorMessage = "";
        IsError = false;

        return;
    }

    public async Task LoginAsync(LoginDto dto)
    {
        var json = JsonSerializer.Serialize(dto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{EndpointSettings.ApiBaseUrl}/Login/login", content);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response into LoginResponse
                 var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                //var loginResponse = await response.Content.ReadAsStringAsync(); 

                if (loginResponse != null)
                {
                    // Store the token securely
                    await SecureStorage.SetAsync("auth_token", loginResponse.Token);
                    var userName = JwtHelper.GetUserNameFromToken(loginResponse.Token);
                    await SecureStorage.SetAsync("userLogin", userName);
                }

                ErrorMessage = "";
                IsError = false;
            }
            else
            {                
                ErrorMessage = await response.Content.ReadAsStringAsync(); 
                IsError = true;
            }
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