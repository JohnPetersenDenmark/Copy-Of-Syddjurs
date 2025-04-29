using Microsoft.VisualBasic;
using Syddjurs.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Syddjurs.Pages;

public partial class LoanPage : ContentPage
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<ItemInListDto> Items { get; set; }
    public ObservableCollection<LoanItemDto> LoanItemList { get; set; }

    private List<ItemInListDto> ItemRemovedList;

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

    private LoanItemDto _selectedLoanItem;

    public LoanItemDto SelectedLoanItem
    {
        get => _selectedLoanItem;
        set
        {
            _selectedLoanItem = value;
            OnILoantemSelected(value);
            OnPropertyChanged(nameof(SelectedLoanItem));
        }
    }

    

    public LoanPage()
    {
        InitializeComponent();

        _httpClient = new HttpClient();
        Items = new ObservableCollection<ItemInListDto>(); // Initialize the collection

        LoanItemList = new ObservableCollection<LoanItemDto>(); // Initialize the collection

        ItemRemovedList = new List<ItemInListDto> ();

        Loaded += ItemListPage_Loaded;


        BindingContext = this;
    }

    private void ItemListPage_Loaded(object? sender, EventArgs e)
    {
        GetItemsForList();
    }

    private async void GetItemsForList()
    {
        try
        {
            var response = await _httpClient.GetStringAsync("http://10.110.240.4:5000/Home/itemsforlist");

            var items = JsonSerializer.Deserialize<List<ItemInListDto>>(response);


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

    private void OnItemSelected(ItemInListDto selectedItem)
    {
        if (selectedItem == null) return;

            var itemInLoanList = LoanItemList.FirstOrDefault(p => p.ItemId == selectedItem.Id);

            if (itemInLoanList != null)
            {
                return;
            }

            var loanItem = new LoanItemDto();

            loanItem.ItemId = selectedItem.Id;
            loanItem.ItemName = selectedItem.Name;
            loanItem.Note = "";
            loanItem.Number = 1;
            loanItem.AvailabeNumber = (int)SelectedItem.Number;
            
            
            LoanItemList.Add(loanItem);

            ItemRemovedList.Add(selectedItem);

            Items.Remove(selectedItem);


    }

    private void OnILoantemSelected(LoanItemDto selectedLoanItem)
    {
        if (selectedLoanItem == null) return;

        foreach (var loanItem in LoanItemList)
        {
            if (loanItem.Equals(SelectedLoanItem))
            {
                loanItem.IsSelected = true;
            }
            else
            {
                loanItem.IsSelected = false;
            }
        }  
    }

    private void OnRemoveLoanItemClicked(object sender, EventArgs e)
    {
        if (SelectedLoanItem == null) return;

        var item = ItemRemovedList.FirstOrDefault(p => p.Id == SelectedLoanItem.ItemId);

        Items.Add(item);
        ItemRemovedList.Remove(item);
        LoanItemList.Remove(SelectedLoanItem);

    }

    //private void OnEntryNumberOfItemsChanged(object sender, TextChangedEventArgs e)
    //{
    //    var newText = e.NewTextValue;

    //    // Do your validation here
    //    if (string.IsNullOrWhiteSpace(newText))
    //    {
    //        // maybe show error or disable a button
    //    }
    //    else
    //    {
    //        // everything is good
    //    }
    //}

    private async void SaveClicked(object sender, EventArgs e)
    {

        var loan = new LoanUploadDto();      

        loan.Lender = Preferences.Get("UserName", "");
        var now = DateTime.Now;

        loan.LoanDate = now.ToString("d", new CultureInfo("da-DK"));


        foreach (var loanItem in LoanItemList)
        {
            var loanItemLine = new LoanItemLinesUploadDto();
            loanItemLine.ItemId = loanItem.ItemId;
            loanItemLine.Note = loanItem.Note;
            loanItemLine.Number = loanItem.Number;

            loan.LoanItemLines.Add(loanItemLine);
        }

        var json = JsonSerializer.Serialize(loan);

        var content = new StringContent(json, Encoding.UTF8, "application/json");


        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("http://10.110.240.4:5000/Home/uploadloan", content);
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

    }


}