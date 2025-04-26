using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Syddjurs.Models;

namespace Syddjurs.Pages;

public partial class ItemPage : ContentPage, IQueryAttributable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly HttpClient _httpClient;

    private ItemDto _selectedItem;
    private int _selectedItemId;

    private bool _isDropdownVisible = true;
    public bool IsDropdownVisible
    {
        get => _isDropdownVisible;
        set
        {
            if (_isDropdownVisible != value)
            {
                _isDropdownVisible = value;
                OnPropertyChanged(nameof(IsDropdownVisible));
            }
        }
    }

    private ItemCategoryDto _selectedCategory;
    public ItemCategoryDto SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
    }

    private ObservableCollection<ItemCategoryDto> _categoryList;
    public ObservableCollection<ItemCategoryDto> CategoryList
    {
        get => _categoryList;
        set
        {
            if (_categoryList != value)
            {
                _categoryList = value;
                OnPropertyChanged(nameof(CategoryList));
            }
        }
    }

    private bool _isSexDropdownVisible = true;
    public bool IsSexDropdownVisible
    {
        get => _isSexDropdownVisible;
        set
        {
            if (_isSexDropdownVisible != value)
            {
                _isSexDropdownVisible = value;
                OnPropertyChanged(nameof(IsSexDropdownVisible));
            }
        }
    }
    public List<string> Sex { get; set; } = new()
    {
        "Dame",
        "Herre",
        "Unisex"
    };


    string _selectedSex;
    public string SelectedSex
    {
        get => _selectedSex;
        set
        {
            if (_selectedSex != value)
            {
                _selectedSex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSex)));
            }
        }
    }

    private bool _isLendable = true;
    public bool IsLendable
    {
        get => _isLendable;
        set
        {
            if (_isLendable != value)
            {
                _isLendable = value;
                OnPropertyChanged(nameof(IsLendable));
            }
        }
    }


    public ItemPage()
	{
		InitializeComponent();

        _httpClient = new HttpClient();

        CategoryList = new ObservableCollection<ItemCategoryDto>(); // Initialize the collection
       

        BindingContext = this;

        Loaded += OnPageLoaded;
    }

    private void OnPageLoaded(object? sender, EventArgs e)
    {
        IsDropdownVisible = false;

         GetCategoriesAsync();

         FetchItemById(_selectedItemId);
       
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        var itemDto = new ItemDto();

        CopyEntryFieldsToDto(itemDto);

        var json = JsonSerializer.Serialize(itemDto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("http://192.168.8.105:5000/Home/uploaditem", content);
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Kategorien er gemt", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Kategorien blev ikke gemt", "OK");
            }
        }

        catch (Exception ex)
        {

        }

        await GetCategoriesAsync();
    }
    

    private void OnCategoryEntryTapped(object sender, EventArgs e)
    {
        IsDropdownVisible = !IsDropdownVisible;
    }

    private void OnSexyEntryTapped(object sender, EventArgs e)
    {
        IsSexDropdownVisible = !IsSexDropdownVisible;
    }

    private async Task GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync("http://192.168.8.105:5000/Home/itemCategories");
           
            var categories = JsonSerializer.Deserialize<List<ItemCategoryDto>>(response);

           
            CategoryList.Clear();
            foreach (var category in categories)
            {               
                CategoryList.Add(category);
            }

            IsDropdownVisible = true;
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error loading images: {ex.Message}");
        }
    }

    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {

        if (e.CurrentSelection.FirstOrDefault() is ItemCategoryDto selected)
        {
            SelectedCategory = selected;
            IsDropdownVisible = false;           
            CategoryEntryChange.Text = selected.Category;
        }
    }

    private void OnSexSelected(object sender, SelectionChangedEventArgs e)
    {

        if (e.CurrentSelection.FirstOrDefault() is String selected)
        {
            SelectedSex = selected;
            IsSexDropdownVisible = false;
            SexEntryChange.Text = selected;
        }
    }

    private void CopyEntriesToDto()
    {
        var itemDto = new ItemDto();


        //itemDto.Id = _selectedItemId;
        //itemDto.Name = ItemName.Text;
        //itemDto.Description = ItemDescription.Text;
        //itemDto.Categori = SelectedCategory;
        //itemDto.Sex = SelectedSex;
        //itemDto.Number = int.Parse(NumberOfItemsEntry.Text);
        //itemDto.Color = ColorEntry.Text;
        //itemDto.Size = SizeEntry.Text;
        //itemDto.Lendable = IsLendable;
    }

    private void CopyDtoToEntryFields(ItemDto itemDto)
    {

        ItemName.Text = itemDto.Name;
        ItemDescription.Text = itemDto.Description;

        if (itemDto.CategoryId != null && itemDto.CategoryId != 0)
        {
            var itemCategoryDto = new ItemCategoryDto();

            itemCategoryDto.Id = (int) itemDto.CategoryId;
            itemCategoryDto.Category = itemDto.CategoryText;

            SelectedCategory = itemCategoryDto;
        }
             
        SelectedSex = itemDto.Sex;
        NumberOfItemsEntry.Text = itemDto.Number.ToString();

        ColorEntry.Text = itemDto.Color;
        SizeEntry.Text = itemDto.Size;
        IsLendable = itemDto.Lendable;
    }

    private void CopyEntryFieldsToDto(ItemDto itemDto)
    {
        itemDto.Id = _selectedItemId;
        itemDto.Name = ItemName.Text ;
       itemDto.Description = ItemDescription.Text;

        if (SelectedCategory != null)
        {
            itemDto.CategoryId = (int) SelectedCategory.Id;
            itemDto.CategoryText = SelectedCategory.Category;
        }

        itemDto.Sex = SelectedSex;
        itemDto.Number = int.Parse(NumberOfItemsEntry.Text);

        itemDto.Color = ColorEntry.Text;
        itemDto.Size = SizeEntry.Text ;
        itemDto.Lendable = IsLendable ;
    }

    //  public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            var item = query["ItemToEdit"] as ItemInListDto;
            if (item != null)
            {
              
                this._selectedItemId = item.Id;
            }
            else
            {
                this._selectedItem = null;
                this._selectedItemId = 0;
            }
        }
        else
        {
            this._selectedItem = null;
            this._selectedItemId = 0;
        }
    }

    private async Task FetchItemById(int id)
    {
        try
        {
           
            var response = await _httpClient.GetStringAsync("http://192.168.8.105:5000/Home/itembyid?id=" + id);

           
            var item = JsonSerializer.Deserialize<ItemDto>(response);

            CopyDtoToEntryFields(item);

            _selectedItem = item;



        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error loading images: {ex.Message}");
        }
    }
}