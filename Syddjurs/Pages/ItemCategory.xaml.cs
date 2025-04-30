using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Syddjurs.Models;
using Syddjurs.Utilities;

namespace Syddjurs.Pages;

public partial class ItemCategory : ContentPage, INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;

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

    private bool _showChangeExistingCategoryText;
    public bool ShowChangeExistingCategoryText
    {
        get => _showChangeExistingCategoryText;
        set
        {
            //if (_showChangeExistingCategoryText != value)
            //{
            _showChangeExistingCategoryText = value;
            OnPropertyChanged(nameof(ShowChangeExistingCategoryText));
            //}
        }
    }



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
    public ItemCategory()
	{
		InitializeComponent();
        _httpClient = new HttpClient();

        CategoryList = new ObservableCollection<ItemCategoryDto>(); // Initialize the collection
        GetCategoriesAsync();

       
        BindingContext = this;

        Loaded += OnPageLoaded;

    }

    private void OnPageLoaded(object? sender, EventArgs e)
    {
        ShowChangeExistingCategoryText = false;
        IsDropdownVisible = false;
    }

    private void OnCategoryEntryTapped(object sender, EventArgs e)
    {
        IsDropdownVisible = !IsDropdownVisible;
    }

    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {

        if (e.CurrentSelection.FirstOrDefault() is ItemCategoryDto selected)
        {
            SelectedCategory = selected;
            IsDropdownVisible = false;
            ShowChangeExistingCategoryText = true;
            CategoryEntryChangeText.Text = selected.Category;
            CategoryEntryChange.Text = selected.Category;
        }
    }

    private async void OnChangeCategorySaveClicked(object sender, EventArgs e)
    {

        var categoryDto = new ItemCategoryDto();

        categoryDto.Category = CategoryEntryChangeText.Text;
        categoryDto.Id = SelectedCategory.Id;

        var json = JsonSerializer.Serialize(categoryDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://10.110.240.4:5000/Home/itemCategories");
            var response = await _httpClient.SendWithTokenAsync(request);
                        
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Kategorien er gemt", "OK");
                SelectedCategory.Category = CategoryEntryChangeText.Text;
                CategoryEntryChange.Text = CategoryEntryChangeText.Text;
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
        ShowChangeExistingCategoryText = false;
    }

    private async Task GetCategoriesAsync()
    {
        try
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "http://10.110.240.4:5000/Home/itemCategories");
            var response = await _httpClient.SendWithTokenAsync(request);

            var categories = JsonSerializer.Deserialize<List<ItemCategoryDto>>(await response.Content.ReadAsStringAsync());

            

            // Clear the collection and add the fetched data

            //  var returnList = new List<StampCategoryDto>();
            CategoryList.Clear();
            foreach (var category in categories)
            {
                //imageUpload.IsSelected = false;
                //imageUpload.SelectedBackgroundColor = _unSelectedCollectionViewItemBackgroundColor;
                //imageUpload.SelectedLabelTextColor = _unSelectedLabelTextColor;
                // CategoryList.Add(category);
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

    private async void OnAddSaveClicked(object sender, EventArgs e)
    {

        var categoryDto = new ItemCategoryDto();

        categoryDto.Category = CategoryEntryAdd.Text;
        categoryDto.Id = 0;

        var json = JsonSerializer.Serialize(categoryDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "http://10.110.240.4:5000/Home/uploadItemCategory");
            var response = await httpClient.SendWithTokenAsync(request);
            
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}