using Microsoft.Maui.Controls;
using Syddjurs.CustomControls;
using Syddjurs.Utilities;
using System.ComponentModel;
using System.Reflection;

namespace Syddjurs.Pages;

public partial class TestPage : ContentPage, IQueryAttributable,  INotifyPropertyChanged
{

    string _sharedtext;
    string _targetEntryId;

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

        //var entries = this.Descendants().OfType<CustomEntry>().ToList();
        //foreach (var customEntry in entries)
        //{
        //    if (CustomEntry.SharedTextId == (_targetEntryId))
        //    {
        //        customEntry.Text = _sharedtext;
        //    }
        //}

        var targetCustomEntry = CustomEntry.FindBySharedTextId(_targetEntryId);

        if (targetCustomEntry != null)
        {
            targetCustomEntry.Text = _sharedtext;
        }

        //GenericProperty = "bb";
        //MyEntryNoBinding.SetBinding(Entry.TextProperty, GenericProperty);



        //if (_targetEntry is Entry )
        //{
        //    var entries = this.Descendants().OfType<Entry>().ToList();
        //    foreach (var entry in entries)
        //    {
        //        if (entry.Equals(_targetEntry))
        //        {
        //            entry.Text = _sharedtext;
        //        }
        //    }
        //}

        //else if (_targetEntry is Entry)
        //{
        //    {
        //        var entries = this.Descendants().OfType<Entry>().ToList();
        //        foreach (var entry in entries)
        //        {
        //            if (entry.Equals(_targetEntry))
        //            {
        //                entry.Text = _sharedtext;
        //            }
        //        }
        //    }


        //}
        // var x = entries.FirstOrDefault(_targetEntry);


        //MyEntryNoBinding.Text = "ggggggggg";

        //MyEntry.Text = "hhhhh";

        //MyCustomEntry.Text = "custom entry";

        // GenericProperty = "aa";


        //Dispatcher.Dispatch(async () =>
        //{
        //    await Task.Delay(50); // Let MAUI finish processing UI/bindings

        //    if (_targetEntry is Entry entry)
        //    {
        //        entry.RemoveBinding(Entry.TextProperty); // remove binding
        //        await Task.Delay(100);                     // let removal settle

        //        entry.Text = _sharedtext;                // now safe to set manually
        //        entry.Focus();                           // optional
        //    }
        //});
    }

    //    if (_targetEntry is CustomEntry )
    //        {

    //            // GenericProperty = _sharedtext;
    //              _targetEntry.RemoveBinding(Entry.TextProperty);


    //            // _targetEntry.SetBinding(Entry.TextProperty, GenericProperty);

    //            // _targetEntry.BindingContext = this;
    //           // _targetEntry.RemoveBinding(Entry.TextProperty);
    //            BindingContext = null; // optionally, disconnect the whole context
    //            _targetEntry.BindingContext = null;
    //            _targetEntry.Text = _sharedtext;

    //            // _targetEntry.Text = _sharedtext;
    //            //// Now manually set Text
    //            //customEntry.Text = _sharedtext ?? "TESTING 123";

    //            //customEntry.InvalidateMeasure(); // <- Force layout
    //            //((VisualElement)customEntry.Parent)?.InvalidateMeasure();

    //            //System.Diagnostics.Debug.WriteLine("Set Text to: " + customEntry.Text);

    //            //customEntry.Focus();
    //        }
    //        else if (_targetEntry is Entry entry)
    //        {
    //            // _targetEntry.RemoveBinding(Entry.TextProperty);
    //            entry.Text = _sharedtext;
    //            entry.Focus();
    //        }
    //    });
    //    //Dispatcher.Dispatch(() =>
    //    //{
    //    //    if (_targetEntry is CustomEntry customEntry)
    //    //    {
    //    //        customEntry.RemoveBinding(Entry.TextProperty);

    //    //        // Test value
    //    //        customEntry.Text = "TESTING 123";

    //    //        System.Diagnostics.Debug.WriteLine("Set Text to: " + customEntry.Text);
    //    //    }
    //    //});

    //    // if (_targetEntry is CustomEntry || _targetEntry is Entry)
    //    // {
    //    //   //  _targetEntry.RemoveBinding(Entry.TextProperty);
    //    // }

    //    //_targetEntry.Text = _sharedtext;






    //    //


    //    //    var bindingBase = custom.GetBinding(CustomEntry.TextProperty);

    //    //    if (bindingBase != null && custom.BindingContext != null)
    //    //    {
    //    //        if (bindingBase is Binding binding)
    //    //        {
    //    //            string  boundPropertyName = binding.Path;
    //    //            var entryBindingContext = custom.BindingContext;
    //    //            var prop = entryBindingContext.GetType().GetProperty(boundPropertyName);

    //    //            if (prop != null && prop.CanWrite)
    //    //            {
    //    //                prop.SetValue(entryBindingContext, _sharedtext);
    //    //            }

    //    //        }                         
    //    //    }
    //    //    else
    //    //    {
    //    //        // No binding, assign Text directly
    //    //        custom.Text = _sharedtext;
    //    //    }

    //    //    custom.Focus();
    //    //}


    //    //if (_targetEntry is CustomEntry custom && !string.IsNullOrEmpty(custom.BindingKey))
    //    //{
    //    //    var prop = GetType().GetProperty(custom.BindingKey, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //    //    if (prop != null && prop.CanWrite)
    //    //    {
    //    //        var targetType = prop.PropertyType;

    //    //        try
    //    //        {
    //    //            var convertedValue = Convert.ChangeType(_sharedtext, targetType);
    //    //            prop.SetValue(this, convertedValue);
    //    //        }
    //    //        catch (Exception ex)
    //    //        {
    //    //            // Optional: log or handle conversion errors
    //    //            Console.WriteLine($"Error setting property '{custom.BindingKey}': {ex.Message}");
    //    //        }
    //    //    }
    //    //}

    //    //_targetEntry?.Focus();
    //}
    

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Dispatcher.Dispatch(async () =>
        //{
        //    await Task.Delay(200); // Allow UI to settle

        //    if (_targetEntry is CustomEntry customEntry)
        //    {
        //        customEntry.RemoveBinding(Entry.TextProperty);
        //        customEntry.SetBinding(Entry.TextProperty, _sharedtext);

        //        // Now manually set Text
        //        customEntry.Text = _sharedtext ?? "TESTING 123";

        //        customEntry.InvalidateMeasure(); // <- Force layout
        //        ((VisualElement)customEntry.Parent)?.InvalidateMeasure();

        //        System.Diagnostics.Debug.WriteLine("Set Text to: " + customEntry.Text);

        //        customEntry.Focus();
        //    }
        //    else if (_targetEntry is Entry entry)
        //    {
        //        // _targetEntry.RemoveBinding(Entry.TextProperty);
        //        entry.Text = _sharedtext;
        //        entry.Focus();
        //    }
        //});

    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
         
            if (query.TryGetValue("ReceivedSharedText", out var sharedText) && query.TryGetValue("TargetEntryId", out var targetEntry))
            {
                _sharedtext = sharedText?.ToString();
                _targetEntryId = targetEntry.ToString();

               // _targetEntry = targetEntry as CustomEntry;
               // _targetEntry = targetEntry;
            }
        }
       
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}