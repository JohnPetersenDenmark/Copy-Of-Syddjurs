using Syddjurs.Models;
using Syddjurs.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace Syddjurs.Pages;

public partial class AdminUsersPage1 : ContentPage, INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<UserDto> Users { get; set; }

    public List<RoleDto> AllRoles { get; set; }

    private UserDto _selectedUser;
    public UserDto SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnUserSelected();
            OnPropertyChanged(nameof(SelectedUser));
        }
    }

    private UserDto _selectedUserBackup;

    public AdminUsersPage1()
	{
		InitializeComponent();

        _httpClient = new HttpClient();
        Users = new ObservableCollection<UserDto>(); // Initialize the collection
        AllRoles = new List<RoleDto>(); // Initialize the collection

        BindingContext = this;
    }

    private void OnUserSelected()
    {
        if (SelectedUser == null)
        {
            foreach (var user in Users)
            {
                user.IsSelected = false;
                user.ShowInList = true;
                foreach (var role in user.Roles)
                {
                    role.CheckBoxEnabled = false;
                }
            }
            return;
        }

        _selectedUserBackup = CloneUserDto(SelectedUser);

        foreach (var user in Users)
        {
            if (user.Equals(SelectedUser))
            {
                user.IsSelected = true;
                user.ShowInList = true;
                foreach (var role in user.Roles)
                {                  
                    role.CheckBoxEnabled = true;
                }
            }
            else
            {
                user.IsSelected = false;
                user.ShowInList = false;
                foreach (var role in user.Roles)
                {
                    role.CheckBoxEnabled = false;
                }
            }
        }     
    }


    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetAllRoles();
        await GetUsersForList();
    }

  
    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (SelectedUser == null) return;
      
        SelectedUser = null;
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (SelectedUser == null) return;

        UploadUser(SelectedUser);
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        if (SelectedUser == null) return;
       
        Users.Remove(SelectedUser);

        Users.Add(_selectedUserBackup);
    
        SelectedUser = null;
    }

    private async Task GetUsersForList()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{EndpointSettings.ApiBaseUrl}/Home/usersforlist");
            var response = await _httpClient.SendWithTokenAsync(request);

            var users = JsonSerializer.Deserialize<List<UserDto>>(await response.Content.ReadAsStringAsync());

            Users.Clear();
            foreach (var user in users)
            {
                user.ShowInList = true;
                foreach(var role in user.Roles )
                {
                    role.IsCheckBoxChecked = true;
                    role.CheckBoxEnabled = false;
                }

                foreach (var role in AllRoles)
                {
                    if (!user.Roles.Any(r => r.Id == role.Id))
                    {
                        user.Roles.Add(new RoleDto
                        {
                            Id = role.Id,
                            RoleName = role.RoleName,
                            IsCheckBoxChecked = false,
                            CheckBoxEnabled = false
                        });
                    }
                }
                

                Users.Add(user);
            }
        }
        catch (Exception ex)
        {           
            Console.WriteLine($"Error loading images: {ex.Message}");
        }
    }


    private async Task GetAllRoles()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{EndpointSettings.ApiBaseUrl}/Home/allroles");
            var response = await _httpClient.SendWithTokenAsync(request);

            var roles = JsonSerializer.Deserialize<List<RoleDto>>(await response.Content.ReadAsStringAsync());

            AllRoles.Clear();           
            foreach (var role in roles)
            {
                var roleDto = new RoleDto();
                roleDto.Id = role.Id;
                roleDto.RoleName = role.RoleName;
                roleDto.IsCheckBoxChecked = false;
                roleDto.CheckBoxEnabled = false;
             
                AllRoles.Add(roleDto);
            }
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task UploadUser(UserDto dto)
    {
        var json = JsonSerializer.Serialize(dto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{EndpointSettings.ApiBaseUrl}/Home/uploaduser");
            request.Content = content;

            var response = await _httpClient.SendWithTokenAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Brugeren er gemt", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Brugeren blev ikke gemt", "OK");
            }
        }

        catch (Exception ex)
        {

        }

        await GetUsersForList();
       
    }

    private UserDto CloneUserDto(UserDto userDtoToClone)
    {
        var clonedDto = new UserDto();

        clonedDto.Id = userDtoToClone.Id;
        clonedDto.UserName = userDtoToClone.UserName;
        clonedDto.Email = userDtoToClone.Email;

       

        foreach (var role in userDtoToClone.Roles)
        {
            var clonedRoleDto = new RoleDto();
            clonedRoleDto.Id = role.Id;
            clonedRoleDto.RoleName = role.RoleName;
            clonedRoleDto.IsCheckBoxChecked = role.IsCheckBoxChecked;

            clonedRoleDto.CheckBoxEnabled = false;

            clonedDto.Roles.Add(clonedRoleDto);
        }

        return clonedDto;
        
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}