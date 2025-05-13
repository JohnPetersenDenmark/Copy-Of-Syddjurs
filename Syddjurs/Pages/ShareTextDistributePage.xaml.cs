
using System.ComponentModel;

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

    public ShareTextDistributePage()
	{ 
        BindingContext = this;
        InitializeComponent();
      
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
         ShareText = _textDistribute;
        //ItemName.Text = _textDistribute;

    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ReceivedSharedText", out var sharedText))
        {
            // string text = sharedText.ToString();
            _textDistribute = sharedText.ToString();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}