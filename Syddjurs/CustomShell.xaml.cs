

namespace Syddjurs;

public  partial class CustomShell : Shell
{
    //public static readonly BindableProperty IsAdministratorProperty =
    //   BindableProperty.Create(nameof(IsAdministrator), typeof(bool), typeof(CustomShell), false, propertyChanged: OnIsAdminChanged);


    //public bool IsAdministrator
    //{
    //    get => (bool)GetValue(IsAdministratorProperty);
    //    set => SetValue(IsAdministratorProperty, value);
    //}

    public CustomShell()
    {
        InitializeComponent();

        Navigated += OnShellNavigated;

        // *********************** BindingContext = this;
    }

    private async void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (e.Current.Location.OriginalString == "//items/itemList")
        {
            // Reset the navigation stack of the current tab
            await Shell.Current.GoToAsync("///items/itemList");
        }
    }

    //private static void OnIsAdminChanged(BindableObject bindable, object oldValue, object newValue)
    //{
    //    var customShell = (CustomShell)bindable;
    //    bool isAdmininistrator = (bool)newValue;

    //    var adminItem = customShell.Items.FirstOrDefault(i => i.Route == "items");

    //    if (adminItem != null)
    //        adminItem.IsVisible = isAdmininistrator;
}

