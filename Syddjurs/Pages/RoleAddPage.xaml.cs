using Syddjurs.Models;
using Syddjurs.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace Syddjurs.Pages;

public partial class RoleAddPage : ContentPage , INotifyPropertyChanged
{

    private readonly HttpClient _httpClient;
    public ObservableCollection<RoleDto> Roles { get; set; }

    private RoleDto _selectedRole;

    public RoleDto SelectedRole
    {
        get => _selectedRole;
        set
        {
            _selectedRole = value;
            OnRoleSelected(value);
            OnPropertyChanged(nameof(SelectedRole));
        }
    }


    public RoleAddPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        Roles = new ObservableCollection<RoleDto>();
        BindingContext = this;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetAllRoles();       
    }

    private void OnRoleSelected(RoleDto selectedItem)
    {
        if (selectedItem == null) return;

        foreach (var role in Roles)
        {
            if (role.Equals(SelectedRole))
            {
                role.IsSelected = true;
            }
            else
            {
                role.IsSelected = false;
            }
        }

    }

    private async void OnNewRoleSaveClicked(object sender, EventArgs e)
    {

        var roleDto = new RoleDto();

        roleDto.RoleName = NewRoleNameEntry.Text;
        roleDto.Id = "";
        roleDto.IsCheckBoxChecked = false;

        var json = JsonSerializer.Serialize(roleDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{EndpointSettings.ApiBaseUrl}/Home/uploadrole");
            request.Content = content;
            var response = await _httpClient.SendWithTokenAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Rollen er gemt", "OK");             
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Rollen blev ikke gemt", "OK");
            }
        }

        catch (Exception ex)
        {

        }

        await GetAllRoles();
       
    }

    private async void OnChangeRoleSaveClicked(object sender, EventArgs e)
    {

        var roleDto = new RoleDto();

        roleDto.RoleName = NewRoleNameEntry.Text;
        roleDto.Id = _selectedRole.Id;
        roleDto.IsCheckBoxChecked = false;

        var json = JsonSerializer.Serialize(roleDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{EndpointSettings.ApiBaseUrl}/Home/uploadrole");
            request.Content = content;
            var response = await _httpClient.SendWithTokenAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Rollen er gemt", "OK");          
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Rollen blev ikke gemt", "OK");
            }
        }

        catch (Exception ex)
        {

        }

        await GetAllRoles();

    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (SelectedRole == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
                    { "RoleToEdit", SelectedRole }
                };

        await Shell.Current.GoToAsync("///addItem", navigationParameter);
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (SelectedRole == null) return;

        // Confirm and remove
        //  ImageUploads.Remove(SelectedStamp);
        SelectedRole = null;
    }

    private async Task GetAllRoles()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{EndpointSettings.ApiBaseUrl}/Home/allroles");
            var response = await _httpClient.SendWithTokenAsync(request);

            var roles = JsonSerializer.Deserialize<List<RoleDto>>(await response.Content.ReadAsStringAsync());

            Roles.Clear();
            foreach (var role in roles)
            {
                var roleDto = new RoleDto();
                roleDto.Id = role.Id;
                roleDto.RoleName = role.RoleName;
                roleDto.IsSelected = false;
                roleDto.CheckBoxEnabled = false;

                Roles.Add(roleDto);
            }
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}