using Syddjurs.Models;
using Syddjurs.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;

namespace Syddjurs.Pages;

public partial class AdminUsersPage1 : ContentPage, INotifyPropertyChanged
{

    private readonly HttpClient _httpClient;
    public ObservableCollection<UserDto> Users { get; set; }

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


    public AdminUsersPage1()
	{
		InitializeComponent();

        _httpClient = new HttpClient();
        Users = new ObservableCollection<UserDto>(); // Initialize the collection

        BindingContext = this;
    }


    private void OnUserSelected()
    {
        if (SelectedUser == null) return;

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


    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetUsersForList();


    }

    //private async void OnEditClicked(object sender, EventArgs e)
    //{
    //    if (SelectedUser == null) return;

    //    var navigationParameter = new Dictionary<string, object>
    //    {
    //                { "UserToEdit", SelectedUser }
    //            };

    //    await Shell.Current.GoToAsync("///addUser", navigationParameter);
    //}

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (SelectedUser == null) return;
      
        SelectedUser = null;
    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        if (SelectedUser == null) return;

        foreach (var user in Users)
        {
            user.ShowInList = true;
            user.IsSelected = false;
            foreach (var role in user.Roles)
            {
                role.CheckBoxEnabled = false;
            }
        }

        SelectedUser = null;
    }

    private async void GetUsersForList()
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
                    role.IsChecked = true;
                    role.CheckBoxEnabled = false;
                }              
                Users.Add(user);
            }

            //IsDropdownVisible = true;
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error loading images: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}