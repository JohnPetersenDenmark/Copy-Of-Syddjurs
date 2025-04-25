using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Maui.Controls;
using Android.Graphics;

namespace Syddjurs
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public  class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Microsoft.Maui.ApplicationModel.Platform.Init(this, savedInstanceState);

            // Set status bar color
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#8e1157")); // Your desired hex color

            // Optional: Set navigation bar color
            Window.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#8e1157"));

            // Optional: Make status bar icons dark or light
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            //{
            //    var flags = (StatusBarVisibility)Window.DecorView.SystemUiVisibility;
            //    flags |= StatusBarVisibility.LightStatusBar; // Light text = remove this
            //    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags;
            //}
        }

        public void SetSystemBarColorsBasedOnTheme(bool isDarkTheme)
        {
            var window = Window;

            if (isDarkTheme)
            {
                window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#8e1157"));
                window.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#8e1157"));

                // Light icons for dark background
                window.DecorView.SystemUiVisibility = 0;
            }
            else
            {
                window.SetStatusBarColor(Android.Graphics.Color.White);
                window.SetNavigationBarColor(Android.Graphics.Color.White);

                // Dark icons for light background (Android 6.0+)
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                }
            }
        }
    }
}
