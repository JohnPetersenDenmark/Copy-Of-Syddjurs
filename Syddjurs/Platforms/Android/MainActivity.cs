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
           
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            // Theme may have changed; update bar colors
            SetSystemBarColorsBasedOnTheme();
        }

   

        private void HandleSharedText(Intent intent)
        {
            if (intent.Action == Intent.ActionSend && intent.Type == "text/plain")
            {
                SharedText = intent.GetStringExtra(Intent.ExtraText);


                //Intent.cat
                //var sendingPackage = CallingPackage;
                //if (!string.IsNullOrEmpty(sendingPackage))
                //{
                //    var pm = PackageManager;
                //    var appInfo = pm.GetApplicationInfo(sendingPackage, 0);
                //    var appName = pm.GetApplicationLabel(appInfo);

                  
                //}
            }
        }

        private void SetSystemBarColorsBasedOnTheme()
        {
            var window = Window;

            bool isDarkTheme = (Resources.Configuration.UiMode & Android.Content.Res.UiMode.NightMask)
                               == Android.Content.Res.UiMode.NightYes;

            if (!isDarkTheme)
            {
                window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#8e1157"));
                window.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#8e1157"));
                window.DecorView.SystemUiVisibility = 0; // Light icons
            }
            else
            {
                window.SetStatusBarColor(Android.Graphics.Color.White);
                window.SetNavigationBarColor(Android.Graphics.Color.White);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar; // Dark icons
                }
            }
        }
    }
}
