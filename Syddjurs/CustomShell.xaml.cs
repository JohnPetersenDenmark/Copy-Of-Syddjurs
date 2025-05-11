#if ANDROID
using com.companyname.syddjurs;
#endif

namespace Syddjurs;

public  partial class CustomShell : Shell
{
    
    public CustomShell()
    {
        InitializeComponent();

        Navigated += OnShellNavigated;


//#if ANDROID
//                    var activity = Platform.CurrentActivity as MainActivity;
//                    bool isDark = RequestedTheme == AppTheme.Dark;
//                    activity?.SetSystemBarColorsBasedOnTheme(isDark);

//                    RequestedThemeChanged += (s, e) =>
//                    {
//                        var act = Platform.CurrentActivity as MainActivity;
//                        act?.SetSystemBarColorsBasedOnTheme(e.RequestedTheme == AppTheme.Dark);
//                    };
//#endif

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

}

