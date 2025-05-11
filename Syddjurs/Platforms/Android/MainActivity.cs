using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace com.companyname.syddjurs
{
    [Activity(
        Label = "Syddjurs",
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize
                             | ConfigChanges.Orientation
                             | ConfigChanges.UiMode
                             | ConfigChanges.ScreenLayout
                             | ConfigChanges.SmallestScreenSize
                             | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static string? SharedText { get; private set; }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HandleSharedText(Intent);
            // SetSystemBarColors("#8e1157");
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            if (intent != null)
                HandleSharedText(intent);
        }

        private void HandleSharedText(Intent intent)
        {
            if (intent.Action == Intent.ActionSend && intent.Type == "text/plain")
            {
                SharedText = intent.GetStringExtra(Intent.ExtraText);
            }
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
