using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syddjurs.CustomControls;

namespace Syddjurs.Utilities
{
    public class EntryValidationBehavior : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            // Validate as int after converting the string to an int
            bool isValid = int.TryParse(e.NewTextValue, out var number);

            // Optionally, you can check a range if needed
            // bool isValid = number > 0 && number < 100;

            entry.BackgroundColor = isValid ? Colors.Transparent : Colors.LightPink;

        }
    }
}
