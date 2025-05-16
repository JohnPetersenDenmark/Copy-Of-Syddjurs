
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
        //ItemName.Text = _textDistribute;

        // MakeListOfDestinations();

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

            var navParams = new Dictionary<string, object>
                {
                    { "ReceivedSharedText", ShareText },
                    { "TargetEntry", selectedEntry.EntryControl } // Pass target entry
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
                                 //EntryId = string.IsNullOrEmpty(e.EntryId) ? "(no id)" : e.EntryId,
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

    private async Task MakeListOfDestinations()
    {
        //var targets = new List<(Type, string)>();

        //var knownPages = new List<Type> 
        //{ 
        //    typeof(LoanPage),
        //     typeof(LoginPage),
        //    typeof(ItemPage) };


        var pageTypes = Assembly.GetExecutingAssembly()
                               .GetTypes()
                               .Where(t => typeof(ContentPage).IsAssignableFrom(t) && !t.IsAbstract);

        var destinationList = new List<Page>();

        foreach (var type in pageTypes)
        {
            try
            {
                var page = Activator.CreateInstance(type) as Page;
                if (page == null)
                    continue;

                var entries = FindCustomEntriesWithEntryId(page);
                if (entries.Any())
                {
                    destinationList.Add(page);
                }
            }
            catch
            {
                // Skip pages that cannot be instantiated
            }
        }
    }

    private static List<CustomEntry> FindCustomEntriesWithEntryId(Element root)
    {
        var result = new List<CustomEntry>();

        foreach (var child in root.Descendants())
        {
            if (child is CustomEntry entry)
            {
                result.Add(entry);

                //var entryId = entry.GetValue(CustomEntry.EntryIdProperty) as string;
                //if (!string.IsNullOrEmpty(entryId))
                //{
                //    result.Add(entry);
                //}
            }
        }

        return result;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}