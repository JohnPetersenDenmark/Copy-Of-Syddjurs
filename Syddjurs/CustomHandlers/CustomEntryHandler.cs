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
            //[nameof(CustomEntry.CustomInputType)] = MapKeyboardInputType
        };

        private static void MapKeyboardInputType(CustomEntryHandler handler, CustomEntry entry)
        {
#if ANDROID
            InputTypes inputType;

            //if (handler.PlatformView is AppCompatEditText editText)
            //{
            //    switch (entry.CustomInputType)
            //    {
            //        case CustomKeyboardInputType.Email:
            //            inputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
            //            break;
            //        case CustomKeyboardInputType.Numeric:
            //            inputType = InputTypes.ClassNumber;
            //            break;
            //        case CustomKeyboardInputType.Normal:
            //            inputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine | InputTypes.TextVariationNormal;
            //            break;
            //        default:
            //            inputType = InputTypes.ClassText  | InputTypes.TextFlagMultiLine | InputTypes.TextVariationNormal;
            //                        //| InputTypes.TextVariationNormal;
            //                        //| InputTypes.TextFlagCapSentences
            //                        //| InputTypes.TextFlagMultiLine
            //                        //| InputTypes.TextVariationNormal;
            //            break;
            //    }

            //    editText.SetRawInputType(inputType);
            //    editText.InputType = inputType;
            //    editText.ImeOptions = ImeAction.Done;
            //}
#endif
        }
        

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

            if (VirtualView is CustomEntry customEntry)
            {
                var hashCode = customEntry.GetHashCode();   
            }

            if (nativeView is AppCompatEditText editText)
            {
                // Set InputType to allow full character input
                editText.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextFlagCapSentences;

                editText.FocusChange += OnFocusChanged;

                nativeView.LongClickable = true;

                nativeView.LongClick += NativeView_LongClick;
            }                     
        }

        private void NativeView_LongClick(object? sender, LongClickEventArgs e)
        {
            if (VirtualView is CustomEntry customEntry)
            {
                customEntry.OnLongPressed();
            }
        }
#endif



#if ANDROID
        private void OnFocusChanged(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            //List<Drawable> layerList = new List<Drawable>();

            //var customEntry = this.VirtualView as CustomEntry;
            //var focusedUnderlineColor = customEntry.UnderlineColor;
            //var unFocusedUnderlineColor = customEntry.UnderlineColor;

            //if (sender is AndroidX.AppCompat.Widget.AppCompatEditText editText)
            //{
            //    var existingBackground = editText.Background;

            //    if (existingBackground is Android.Graphics.Drawables.LayerDrawable layerDrawable)
            //    {
            //        for (int i = 0; i < layerDrawable.NumberOfLayers; i++)
            //        {
            //            var layer = layerDrawable.GetDrawable(i);
            //            if ( layer is not UnderlineDrawable underLineDrawable)
            //            {
            //                layerList.Add(layer);
            //            }                       
            //        }
            //    }

            //    var currentUnderlineColor = e.HasFocus ? focusedUnderlineColor : unFocusedUnderlineColor;

            //    var underlineDrawable = new UnderlineDrawable( currentUnderlineColor.ToPlatform());

            //    underlineDrawable.SetBounds(0, 0, editText.Width, editText.Height);

            //    layerList.Add(underlineDrawable);

            //    var newBackGround = new Android.Graphics.Drawables.LayerDrawable(layerList.ToArray());

            //    // Post to ensure layout is ready
            //    editText.Post(() =>
            //    {
            //        editText.SetBackground(newBackGround);
            //        editText.SetCursorVisible(true);
            //    });
            //}         
        }
#endif


#if ANDROID

        protected override void DisconnectHandler(AppCompatEditText nativeView)
        {           
            nativeView.FocusChange -= OnFocusChanged;
            nativeView.LongClick -= NativeView_LongClick;

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
