using Syddjurs.ViewModels;

namespace Syddjurs.Pages;

public partial class SettingsPage : ContentPage
{
    SettingsViewModel viewModel;

    public SettingsPage()
	{
		InitializeComponent();

        viewModel = new SettingsViewModel();

        this.Loaded += async (s, e) =>
        {
            

            viewModel.UserName = Preferences.Get("UserName", "");
            viewModel.Password = Preferences.Get("Password", "");
                                           
           
        };
      

        BindingContext = viewModel;
    }

  

    private void HideShowAddressFields(bool show)
    {
        entriesContainer.IsVisible = show;
       
     

    }
}