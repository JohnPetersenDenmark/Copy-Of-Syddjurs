using Syddjurs.CustomHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.CustomControls
{
    public enum CustomKeyboardInputType
    {
        Normal,
        Email,
        Numeric
    }

    public class CustomEntry : Entry
    {

       // public string IsCustomEntry = "yes";

        public CustomEntry()
        {          
           
        }

        public static readonly BindableProperty UnderlineColorProperty =
            BindableProperty.Create(
            propertyName: nameof(UnderlineColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomShell),
            defaultValue: Colors.Black,  // Default color
            defaultBindingMode: BindingMode.TwoWay);

        // Property Wrapper
        public Color UnderlineColor
        {
            get => (Color)GetValue(UnderlineColorProperty);
            set => SetValue(UnderlineColorProperty, value);
        }

        public static readonly BindableProperty CustomInputTypeProperty =
    BindableProperty.Create(
        nameof(CustomInputType),
        typeof(CustomKeyboardInputType),
        typeof(CustomEntry),
        CustomKeyboardInputType.Normal);

        public CustomKeyboardInputType CustomInputType
        {
            get => (CustomKeyboardInputType)GetValue(CustomInputTypeProperty);
            set => SetValue(CustomInputTypeProperty, value);
        }
    }
}
