using System;
using System.Collections.Generic;
using System.Linq;
using Interview.ServiceInterface.Scraping;
using ServiceStack;
using ServiceStack.Script;
using ServiceStack.DataAnnotations;
using Interview.ServiceModel;
using ServiceStack.OrmLite;
using System.Threading.Tasks;

namespace Interview.ServiceInterface
{
    [Exclude(Feature.Metadata)]
    [FallbackRoute("/{PathInfo*}", Matches="AcceptsHtml")]
    public class FallbackForClientRoutes
    {
        public string PathInfo { get; set; }
    }

    public class MyServices : Service
    {
        private readonly IDataScraper _scraper;
        public MyServices(IDataScraper scraper)
        {
            _scraper = scraper;
        }
        //Return index.html for unmatched requests so routing is handled on client
        public object Any(FallbackForClientRoutes request) => 
            new PageResult(Request.GetPage("/"));

        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }

    }
}
