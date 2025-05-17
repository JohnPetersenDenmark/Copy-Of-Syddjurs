using Syddjurs.CustomControls;
using System.ComponentModel;
using System.Reflection;

namespace Syddjurs.Pages;

public partial class TestPage : ContentPage, IQueryAttributable,  INotifyPropertyChanged
{

    string _sharedtext;
    Entry _targetEntry;

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


    public TestPage()
	{
		InitializeComponent();
        Loaded += TestPage_Loaded;
        BindingContext = this;
	}

    private void TestPage_Loaded(object? sender, EventArgs e)

    {
        base.OnAppearing();

        if (_targetEntry is CustomEntry custom && !string.IsNullOrEmpty(custom.BindingKey))
        {
            var prop = GetType().GetProperty(custom.BindingKey, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.CanWrite)
            {
                var targetType = prop.PropertyType;

                try
                {
                    var convertedValue = Convert.ChangeType(_sharedtext, targetType);
                    prop.SetValue(this, convertedValue);
                }
                catch (Exception ex)
                {
                    // Optional: log or handle conversion errors
                    Console.WriteLine($"Error setting property '{custom.BindingKey}': {ex.Message}");
                }
            }
        }

        _targetEntry?.Focus();
    }
    

    protected override void OnAppearing()
    {
        base.OnAppearing();
    

    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
         
            if (query.TryGetValue("ReceivedSharedText", out var sharedText) && query.TryGetValue("TargetEntry", out var targetEntry))
            {
                _sharedtext = sharedText?.ToString();
                _targetEntry = targetEntry as Entry;             
            }
        }
       
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}