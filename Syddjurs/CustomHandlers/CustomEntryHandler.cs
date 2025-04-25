using Microsoft.Maui.Handlers;
using Syddjurs.CustomControls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;




#if ANDROID
using Android.Graphics.Drawables;
using AndroidX.ConstraintLayout.Helper.Widget;
using Android.Graphics;
using Android.Content.Res;
using MauiColor = Microsoft.Maui.Graphics;
using AndroidResourceAttributes = Android.Resource.Attribute;
using Android.Widget;
using static Android.Views.View;
using Android.Text;
using Android.Views.InputMethods;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using AndroidX.AppCompat.Widget;
#elif IOS
using UIKit;
using CoreGraphics;
#elif WINDOWS
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
#endif



namespace Syddjurs.CustomHandlers
{
    public class CustomEntryHandler : EntryHandler
    {
        public static IPropertyMapper<CustomEntry, CustomEntryHandler> MyMapper = new PropertyMapper<CustomEntry, CustomEntryHandler>(EntryHandler.Mapper)
        {
            [nameof(CustomEntry.UnderlineColor)] = MapUnderlineColor,
        };


        public CustomEntryHandler() : base(MyMapper)
        {

        }


        private static void MapUnderlineColor(CustomEntryHandler handler, CustomEntry entry)
        {
#if ANDROID
            if (handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText appCompatEditText)
            {
                // Post to ensure changes are done on the UI thread
                appCompatEditText.Post(() =>
                {
                    List<Drawable> layerList = new List<Drawable>();

                    // 1. Save the existing background
                    var existingBackground = appCompatEditText.Background;

                    if (existingBackground is Android.Graphics.Drawables.LayerDrawable layerDrawable)
                    {
                        for (int i = 0; i < layerDrawable.NumberOfLayers; i++)
                        {
                            var layer = layerDrawable.GetDrawable(i);
                            if (layer is not InsetDrawable underLineDrawable)
                            {
                                layerList.Add(layer);
                            }
                        }
                    }

                 

                    // 2. Create a new drawable for the underline
                    var underlineDrawable = new UnderlineDrawable(entry.UnderlineColor.ToPlatform());
                    

                    // 3. Set bounds for the underline drawable (width, height should match the EditText)
                    underlineDrawable.SetBounds(0, 0, appCompatEditText.Width, appCompatEditText.Height);


                    layerList.Add(underlineDrawable);

                   
                    var newBackGround = new Android.Graphics.Drawables.LayerDrawable(layerList.ToArray());

                    // 5. Apply the layered drawable as the new background
                    appCompatEditText.SetBackground(newBackGround);
                  

                    // Optionally, invalidate the view to ensure it gets redrawn immediately
                    appCompatEditText.Invalidate();
                });
            }
#endif
        }




#if ANDROID
        protected override void ConnectHandler(AppCompatEditText nativeView)
        {
            base.ConnectHandler(nativeView);


            if (nativeView is AndroidX.AppCompat.Widget.AppCompatEditText editText)
            {
                editText.FocusChange += OnFocusChanged;
            }

           
        }
#endif
#if ANDROID
        private void OnFocusChanged(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            List<Drawable> layerList = new List<Drawable>();

            var customEntry = this.VirtualView as CustomEntry;
            var focusedUnderlineColor = customEntry.UnderlineColor;
            var unFocusedUnderlineColor = customEntry.UnderlineColor;

            if (sender is AndroidX.AppCompat.Widget.AppCompatEditText editText)
            {
                var existingBackground = editText.Background;

                if (existingBackground is Android.Graphics.Drawables.LayerDrawable layerDrawable)
                {
                    for (int i = 0; i < layerDrawable.NumberOfLayers; i++)
                    {
                        var layer = layerDrawable.GetDrawable(i);
                        if ( layer is not UnderlineDrawable underLineDrawable)
                        {
                            layerList.Add(layer);
                        }                       
                    }
                }

                var currentUnderlineColor = e.HasFocus ? focusedUnderlineColor : unFocusedUnderlineColor;

                var underlineDrawable = new UnderlineDrawable( currentUnderlineColor.ToPlatform());

                underlineDrawable.SetBounds(0, 0, editText.Width, editText.Height);

                layerList.Add(underlineDrawable);

                var newBackGround = new Android.Graphics.Drawables.LayerDrawable(layerList.ToArray());

                // Post to ensure layout is ready
                editText.Post(() =>
                {
                    editText.SetBackground(newBackGround);
                    editText.SetCursorVisible(true);
                });
            }         
        }
#endif


#if ANDROID

        protected override void DisconnectHandler(AppCompatEditText nativeView)
        {           
            nativeView.FocusChange -= OnFocusChanged;

            base.DisconnectHandler(nativeView);
        }

#endif

    }

#if ANDROID
    public class UnderlineDrawable : Android.Graphics.Drawables.Drawable
        {

        private readonly Android.Graphics.Paint _paint;

            public UnderlineDrawable(Android.Graphics.Color color)
            {
                _paint = new Android.Graphics.Paint
                {
                    Color = color,
                    StrokeWidth = 2,
                    AntiAlias = true,                    
                };
            }

            public override void Draw(Android.Graphics.Canvas canvas)
            {
                var bounds = Bounds;
                int y = bounds.Bottom - 5;
                canvas.DrawLine(bounds.Left, y, bounds.Right, y, _paint);
            }

            public override void SetAlpha(int alpha)
            {
                _paint.Alpha = alpha;
            }

            public override void SetColorFilter(Android.Graphics.ColorFilter colorFilter)
            {
                _paint.SetColorFilter(colorFilter);
            }

            public override int Opacity => (int)Android.Graphics.Format.Translucent;
    }
#endif
}
