using Syddjurs.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;

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
            var response = await _httpClient.GetStringAsync("http://192.168.8.105:5000/Home/itemsforlist");

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
            //loanItem.Lender = Preferences.Get("UserName", "");
            
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

    
}