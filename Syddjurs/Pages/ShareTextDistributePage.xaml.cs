
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Syddjurs.CustomControls;
using Syddjurs.Models;
using Syddjurs.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Syddjurs.Pages;

public partial class ShareTextDistributePage : ContentPage, IQueryAttributable, INotifyPropertyChanged
{
    private string _textDistribute;

    private string _shareText;
    public string ShareText
    {
        get => _shareText;
        set
        {

            _shareText = value;
            OnPropertyChanged(nameof(ShareText));
            
        }
    }

    private PageInfo _selectedPage;
    public PageInfo SelectedPage
    {
        get => _selectedPage;
        set
        {
            if (_selectedPage != value)
            {
                _selectedPage = value;
                OnPageSelected(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPage)));
                // You could navigate immediately here or have a Command for navigation
            }
        }
    }

    private EntryInfo _selectedEntry;
    public EntryInfo SelectedEntry
    {
        get => _selectedEntry;
        set
        {
             _selectedEntry = value;
            if (_selectedEntry != null)
            {
                OnEntrySelected(_selectedEntry);
                _selectedEntry = null; // Reset selection
                OnPropertyChanged(nameof(SelectedEntry));
            }
        }
    }

    public ObservableCollection<PageInfo> AvailablePages { get; }

    public ShareTextDistributePage()
	{
        AvailablePages = new ObservableCollection<PageInfo>();
       
        InitializeComponent();

        BindingContext = this;



    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
         ShareText = _textDistribute;
    
        MakeListOfDestinationPages();

    }

    private void OnPageSelected(PageInfo selectedPage)
    {
        if (selectedPage == null)
        {
            return;
        }

        //var nameAsRoute = selectedPage.PageType.Name;

        var nameAsRoute = selectedPage.PageName;

        var navigationParams = new Dictionary<string, object>
            {
                { "ReceivedSharedText", ShareText }
        };

         Shell.Current.GoToAsync(nameAsRoute, navigationParams);
    }

    private async Task OnEntrySelected(EntryInfo selectedEntry)
    {
        if (selectedEntry == null)
        {
            return;
        }

        if (selectedEntry is EntryInfo entryInfo)
        {
            var targetPage = AvailablePages.FirstOrDefault(p => p.Entries.Contains(selectedEntry))?.PageInstance;
            if (targetPage == null)
                return;

            string route = targetPage.GetType().Name;

            var navParams = new Dictionary<string, Object>
                {
                    { "ReceivedSharedText", ShareText },
                    { "ReceiveSharedTextId", entryInfo.EntryControl.ReceiveSharedTextId } // Pass target entry
                };

            await Shell.Current.GoToAsync(route, navParams);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ReceivedSharedText", out var sharedText))
        {
            // string text = sharedText.ToString();
            _textDistribute = sharedText.ToString();
        }
    }

   

    private async Task MakeListOfDestinationPages()
    {
        var pagesWithEntries = PageCustomEntryFinder.GetPagesWithCustomEntries();

        //var pagesList = new List<PageInfo>();

        foreach (var kvp in pagesWithEntries)
        {
            var pageInstance = kvp.Key;
            var entries = kvp.Value
                             .Select(e => new EntryInfo
                             {                               
                                 EntryControl = e,
                                 EntryName = e.Placeholder
                             })
                             .ToList();

            AvailablePages.Add(new PageInfo
            {
                PageName = pageInstance.Title ?? pageInstance.GetType().Name,
                PageInstance = pageInstance,
                PageType = GetType(),
                Entries = entries
            });
        }

    }

   
   

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}