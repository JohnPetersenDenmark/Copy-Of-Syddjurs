
namespace Syddjurs;

public  partial class CustomShell : Shell
{
	public CustomShell()
	{
		InitializeComponent();

        Navigated += OnShellNavigated;
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