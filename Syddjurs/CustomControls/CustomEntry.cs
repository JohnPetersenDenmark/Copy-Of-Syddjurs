


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

       

        public CustomEntry()
        {
           
        }

        public event EventHandler? LongPressed;

        public void OnLongPressed()
        {
            LongPressed?.Invoke(this, EventArgs.Empty);
        }

        //    public static readonly BindableProperty EntryIdProperty =
        //   BindableProperty.Create(nameof(EntryId), typeof(string), typeof(CustomEntry), null);

        //    public string EntryId
        //    {
        //        get => (string)GetValue(EntryIdProperty);
        //        set => SetValue(EntryIdProperty, value);
        //    }

        //   public static readonly BindableProperty BindingKeyProperty =
        //BindableProperty.Create(
        //    nameof(BindingKey),
        //    typeof(string),
        //    typeof(CustomEntry),
        //    default(string));

        //   public string BindingKey
        //   {
        //       get => (string)GetValue(BindingKeyProperty);
        //       set => SetValue(BindingKeyProperty, value);
        //   }

        private static readonly Dictionary<string, CustomEntry> _sharedTextRegistry = new();

        public static readonly BindableProperty ReceivesSharedTextProperty =
            BindableProperty.Create(
                nameof(ReceivesSharedText),
                typeof(bool),
                typeof(CustomEntry),
                false,
                propertyChanged: OnReceivesSharedTextChanged);

        public bool ReceivesSharedText
        {
            get => (bool)GetValue(ReceivesSharedTextProperty);
            set => SetValue(ReceivesSharedTextProperty, value);
        }

        public static string SharedTextId { get; private set; }

        public static CustomEntry FindBySharedTextId(string id)
        {
            return _sharedTextRegistry.TryGetValue(id, out var entry) ? entry : null;
        }

        private static void OnReceivesSharedTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomEntry entry && (bool)newValue)
            {
                entry.EnsureSessionScopedSharedTextId();
            }
        }

        private void EnsureSessionScopedSharedTextId()
        {
            if (!string.IsNullOrEmpty(SharedTextId))
                return; // Already initialized

            SharedTextId = Guid.NewGuid().ToString();
            _sharedTextRegistry[SharedTextId] = this;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Re-register if context changes (optional, useful for dynamic pages)
            if (ReceivesSharedText && !string.IsNullOrEmpty(SharedTextId))
            {
                _sharedTextRegistry[SharedTextId] = this;
            }
        }



        public static readonly BindableProperty TestFordevelopmentProperty =
      BindableProperty.Create(
      propertyName: nameof(TestFordevelopment),
      returnType: typeof(string),
      declaringType: typeof(CustomEntry),
      defaultValue: "",  // Default color
      defaultBindingMode: BindingMode.TwoWay);

        // Property Wrapper
        public string TestFordevelopment
        {
            get => (string)GetValue(TestFordevelopmentProperty);
            set => SetValue(TestFordevelopmentProperty, value);
        }


        public static readonly BindableProperty UnderlineColorProperty =
            BindableProperty.Create(
            propertyName: nameof(UnderlineColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Colors.Black,  // Default color
            defaultBindingMode: BindingMode.TwoWay);

        // Property Wrapper
        public Color UnderlineColor
        {
            get => (Color)GetValue(UnderlineColorProperty);
            set => SetValue(UnderlineColorProperty, value);
        }

        //    public static readonly BindableProperty CustomInputTypeProperty =
        //BindableProperty.Create(
        //    nameof(CustomInputType),
        //    typeof(CustomKeyboardInputType),
        //    typeof(CustomEntry),
        //    CustomKeyboardInputType.Normal);

        //    public CustomKeyboardInputType CustomInputType
        //    {
        //        get => (CustomKeyboardInputType)GetValue(CustomInputTypeProperty);
        //        set => SetValue(CustomInputTypeProperty, value);
        //    }
        //}
    }
}
