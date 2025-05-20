using Syddjurs.CustomControls;
using Syddjurs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public static class PageCustomEntryFinder
    {
        public static Dictionary<ContentPage, List<CustomEntry>> GetPagesWithCustomEntries()
        {
             var result = new Dictionary<ContentPage, List<CustomEntry>>();

           // var result = new Dictionary<ContentPage, List<Entry>>();

            // Get all non-abstract ContentPage types in the current assembly
            var pageTypes = Assembly.GetExecutingAssembly()
                                    .GetTypes()
                                    .Where(t => typeof(ContentPage).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var pageType in pageTypes)
            {
                try
                {
                    // Instantiate the page (must have parameterless constructor)
                    var pageInstance = Activator.CreateInstance(pageType) as ContentPage;

                    if (pageInstance == null)
                        continue;

                    // Find all CustomEntry descendants in the page
                    //var customEntries = pageInstance.Descendants()
                    //                               .OfType<CustomEntry>()
                    //                               .ToList();

                    // Find all CustomEntry descendants in the page
                    var customEntries = pageInstance.Descendants()
                                                   .OfType<CustomEntry>()
                                                   .ToList();

                    if (customEntries.Count > 0)
                        result.Add(pageInstance, customEntries);
                }
                catch
                {
                    // Skip pages that cannot be instantiated for some reason
                }
            }

            return result;
        }
    }
}
