using Microsoft.Maui.Controls;
using Syddjurs.CustomControls;
using Syddjurs.Utilities;
using System.ComponentModel;
using System.Reflection;

namespace Syddjurs.Pages;

public partial class TestPage : ContentPage, IQueryAttributable,  INotifyPropertyChanged
{

    string _sharedtext;
    string _receiveSharedTextId;

    private string _testEntryText;
    public string TestEntryText
    {
        get => _testEntryText;
        set
        {

            _testEntryText = value;
            OnPropertyChanged(nameof(TestEntryText));

        }
    }

    private string _testCustomEntryText;
    public string TestCustomEntryText
    {
        get => _testCustomEntryText;
        set
        {

            _testCustomEntryText = value;
            OnPropertyChanged(nameof( TestCustomEntryText));

        }
    }

    private string _TestAnotherCustomEntryText;
    public string TestAnotherCustomEntryText
    {
        get => _TestAnotherCustomEntryText;
        set
        {

            _TestAnotherCustomEntryText = value;
            OnPropertyChanged(nameof(TestAnotherCustomEntryText));

        }
    }


    


    private string _genericProperty;
    public string GenericProperty
    {
        get => _genericProperty;
        set
        {

            _genericProperty = value;
            OnPropertyChanged(nameof(GenericProperty));

        }
    }

    public TestPage()
	{       
        InitializeComponent();
        Loaded += TestPage_Loaded;
        BindingContext = this;      
    }

    private void TestPage_Loaded(object? sender, EventArgs e)

    {
        var entries = this.Descendants().OfType<CustomEntry>().ToList();
        foreach (var customEntry in entries)
        {
            if (customEntry.ReceiveSharedTextId == _receiveSharedTextId)
            {
                customEntry.Text = _sharedtext;
                break;
            }

        }

    }

   
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
         
            if (query.TryGetValue("ReceivedSharedText", out var sharedText) && query.TryGetValue("ReceiveSharedTextId", out var ReceiveSharedTextId))
            {
                _sharedtext = sharedText?.ToString();
                _receiveSharedTextId = ReceiveSharedTextId.ToString();
    
            }
        }
       
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}