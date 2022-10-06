using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTestFinder.Business
{
    public class PageLoader
    {
        Dictionary<int, HtmlDocument> htmlDocuments;
        string url;

        public PageLoader(string _url)
        {
            htmlDocuments = new Dictionary<int, HtmlDocument>();
            url = _url;
        }

        public Dictionary<int, HtmlDocument> LoadPageFromWeb(int indexStart, int indexEnd)
        {            
            var web = new HtmlWeb();
            Parallel.For(indexStart, indexEnd, i =>
            {
                Console.WriteLine(i);
                var urlPrefix = url + i.ToString();

                var doc = web.Load(urlPrefix + @"/ExecutionReport.html");
                if (doc.DocumentNode.ChildNodes.Count == 0)
                    return;
                htmlDocuments.Add(i, doc);                
            });

            return htmlDocuments;
        }
    }
}
