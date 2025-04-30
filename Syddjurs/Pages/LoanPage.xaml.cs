
using Microsoft.VisualBasic;
using Syddjurs.Models;
using Syddjurs.Utilities;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Syddjurs.Pages;

public partial class LoanPage : ContentPage, IQueryAttributable
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<ItemInListDto> Items { get; set; }
    public ObservableCollection<LoanItemDto> LoanItemList { get; set; }

    private List<ItemInListDto> ItemRemovedList;

    private ItemInListDto _selectedItem;

    private int _selectedLoanId;
    private LoanForLIstDto _selectedLoan;

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
        GetLoanItemsLines(_selectedLoanId);
    }

    private async void GetItemsForList()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://10.110.240.4:5000/Home/itemsforlist");
            var response = await _httpClient.SendWithTokenAsync(request);

            var items = JsonSerializer.Deserialize<List<ItemInListDto>>(await response.Content.ReadAsStringAsync());

       
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
        if (_selectedLoan != null)
        {
            loan.Id = _selectedLoan.Id;
        }
        else
        {
            loan.Id = 0;
        }


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

            var request = new HttpRequestMessage(HttpMethod.Post, "http://http://10.110.240.4:5000/Home/uploadloan");
            var response = await _httpClient.SendWithTokenAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Lånet er gemt", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Lånet blev ikke gemt", "OK");
            }
        }

        catch (Exception ex)
        {

        }

    }

    private async void GetLoanItemsLines(int loanId)
    {
        try
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "\"http://10.110.240.4:5000/Home/loanitemlines?loanId=\" + loanId");
            var response = await _httpClient.SendWithTokenAsync(request);

            var loanItemLines = JsonSerializer.Deserialize<List<LoanItemLinesUploadDto>>(await response.Content.ReadAsStringAsync());

           

            LoanItemDto loanItemLineDto;
            LoanItemList.Clear();
            foreach (var loanItemLine in loanItemLines)
            {
                loanItemLineDto = new LoanItemDto();

                loanItemLineDto.Note = loanItemLine.Note;
                loanItemLineDto.Number = loanItemLine.Number;
                loanItemLineDto.ItemName = loanItemLine.ItemName;
                loanItemLineDto.Id = loanItemLine.LoanId;
                loanItemLineDto.ItemId = loanItemLine.ItemId;

                var item = Items.Where(p => p.Id == loanItemLine.ItemId).FirstOrDefault();

                if (item != null)
                {
                    loanItemLineDto.AvailabeNumber = (int)item.Number;
                    ItemRemovedList.Add(item);

                    Items.Remove(item);
                }

                LoanItemList.Add(loanItemLineDto);
                             
            }

            //IsDropdownVisible = true;
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., API failure, deserialization issues)
            Console.WriteLine($"Error loading images: {ex.Message}");
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            var loan = query["LoanToEdit"] as LoanForLIstDto;
            if (loan != null)
            {

                this._selectedLoanId = loan.Id;
                this._selectedLoan = loan;

             
            }
            else
            {
                this._selectedLoan = null;
                this._selectedLoanId = 0;
            }
        }
        else
        {
            this._selectedLoan = null;
            this._selectedLoanId = 0;
        }       
    }

}