
#if ANDROID
using com.companyname.syddjurs;
#endif
using Microsoft.Maui.Controls;
using Syddjurs.Pages;

namespace Syddjurs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ItemPage), typeof(ItemPage));
            Routing.RegisterRoute(nameof(LoanPage), typeof(LoanPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
            Routing.RegisterRoute("sharetext", typeof(ShareTextDistributePage));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {

           


            var window = new Window(new CustomShell());

#if ANDROID
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                if (!string.IsNullOrEmpty(MainActivity.SharedText))
                {
                    // Navigate using Shell routing with a query
                    // var encodedText = Uri.EscapeDataString(MainActivity.SharedText);

                    if (!MainActivity.IsSharedTextHandled)
                    {
                        var navigationParameter = new Dictionary<string, object>
                    {
                        { "ReceivedSharedText", MainActivity.SharedText }
                    };

                        await Shell.Current.GoToAsync("sharetext", navigationParameter);

                    }

                    // Optional: Clear shared text after use
                    MainActivity.IsSharedTextHandled = true;
                    MainActivity.SharedText = null;
                }
            });
#endif


            return window;             
        }
    }
}