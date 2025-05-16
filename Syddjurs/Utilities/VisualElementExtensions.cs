using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Utilities
{
    public static class VisualElementExtensions
    {
        public static IEnumerable<Element> Descendants(this Element element)
        {
            if (element is IElementController controller)
            {
                foreach (var child in controller.LogicalChildren)
                {
                    yield return child;

                    foreach (var grandChild in child.Descendants())
                        yield return grandChild;
                }
            }
        }
    }
}
