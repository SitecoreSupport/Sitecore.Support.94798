using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Search;
using Sitecore.Pipelines.Search;

namespace Sitecore.Support.Pipelines.Search
{
    /// <summary>
    /// Resolves path searches
    /// </summary>
    public class PathResolver
    {
        /// <summary>
        /// Runs the processor.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Process(SearchArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (!args.TextQuery.StartsWith("/", System.StringComparison.InvariantCulture) || string.IsNullOrEmpty(args.TextQuery))
            {
                return;
            }
            Item item = args.Database.GetItem(args.TextQuery, args.ContentLanguage);
            if (item == null && !args.TextQuery.StartsWith("/sitecore", System.StringComparison.InvariantCulture))
            {
                item = args.Database.GetItem("/sitecore" + args.TextQuery, args.ContentLanguage);
            }
            if (item == null && !args.TextQuery.StartsWith("/sitecore/content", System.StringComparison.InvariantCulture))
            {
                item = args.Database.GetItem("/sitecore/content/" + args.TextQuery, args.ContentLanguage);
            }
            if (item == null)
            {
                return;
            }
            SearchResult result = SearchResult.FromItem(item);
            args.Result.AddResultToCategory(result, Translate.Text("Direct Hit"));
            if (!args.TextQuery.EndsWith("/", System.StringComparison.InvariantCulture))
            {
                return;
            }
            foreach (Item item2 in item.Children)
            {
                result = SearchResult.FromItem(item2);
                args.Result.AddResultToCategory(result, Translate.Text("Children"));
            }
        }
    }
}