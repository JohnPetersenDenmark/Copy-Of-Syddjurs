using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using Syddjurs.Models;
using Syddjurs.Utilities;

namespace Syddjurs.Pages;

public partial class ItemListPage : ContentPage, INotifyPropertyChanged
{

    private readonly HttpClient _httpClient;
    public ObservableCollection<ItemInListDto> Items { get; set; }

    private ItemInListDto _selectedItem;

    public ItemInListDto SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnItemSelected(value);
            OnPropertyChanged(nameof(SelectedItem));           
        }
    }
    public ItemListPage()
	{
		InitializeComponent();

        _httpClient = new HttpClient();
        Items = new ObservableCollection<ItemInListDto>(); // Initialize the collection

      //  Loaded += ItemListPage_Loaded;
       

        BindingContext = this;
    }

    //private void ItemListPage_Loaded(object? sender, EventArgs e)
    //{
    //    GetItemsForList();
    //}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetItemsForList();

    }

    private void OnItemSelected(ItemInListDto selectedItem)
    {
        if (selectedItem == null) return;

        foreach (var item in Items)
        {
            if (item.Equals(SelectedItem))
            {
                item.IsSelected = true;
            }
            else
            {
                item.IsSelected = false;
            }
        }
   
    }


    private void OnItemTapped(object sender, EventArgs e)
    {
        Console.WriteLine("Tapped item!");
    }


    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (SelectedItem == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
                    { "ItemToEdit", SelectedItem }
                };

        await Shell.Current.GoToAsync("///addItem", navigationParameter);
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (SelectedItem == null) return;

        // Confirm and remove
        //  ImageUploads.Remove(SelectedStamp);
        SelectedItem = null;
    }

    private async void GetItemsForList()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{EndpointSettings.ApiBaseUrl}/Home/itemsforlist");
            var response = await _httpClient.SendWithTokenAsync(request);

            var items =  JsonSerializer.Deserialize<List<ItemInListDto>>(await response.Content.ReadAsStringAsync());


            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
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