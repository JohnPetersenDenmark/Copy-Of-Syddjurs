
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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new CustomShell());              
        }
    }
}