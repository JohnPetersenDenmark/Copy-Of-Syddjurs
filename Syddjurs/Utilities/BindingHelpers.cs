using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Utilities
{
    public static class BindingHelpers
    {
        public static BindingBase GetBinding(this BindableObject obj, BindableProperty property)
        {
            var beforeBindingBase = obj.GetValue(property);
            var bindingBase = (BindingBase)obj.GetValue(property);
            if (bindingBase == null)
            {
                return null;
            }

            return bindingBase;


            //return obj?.GetValue(property) is BindingBase ? (BindingBase)obj.GetValue(property) : null;
        }


    }
}
