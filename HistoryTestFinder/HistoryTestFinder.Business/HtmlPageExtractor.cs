using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTestFinder.Business
{
    public class HtmlPageExtractor
    {
        string testName;
        List<TestEntity> testEntities;
        public HtmlPageExtractor(string _testName)
        {
            testName = _testName;
            
        }

        public List<TestEntity> Parse(HtmlDocument htmlDocument, int index)
        {
            testEntities = new List<TestEntity>();
            var tds = htmlDocument.DocumentNode.SelectNodes("//tr//td").Where(x => x.InnerText.Contains(testName)).ToList();
            Parallel.ForEach(tds, td =>
            {
                if (td == null)
                    return;
                try
                {
                    var tr = td.ParentNode;
                    var urlPrefix = @"https://storage.googleapis.com/com-emerald-ccmoffice-cug01-qa_cloudbuild/newman-hack8-tests-nightly/10.10." +
                        index.ToString() + @"/";
                    testEntities.Add(new TestEntity
                    {
                        Id = index,
                        TestName = tr.ChildNodes[0].InnerText,
                        PassedOrFailed = tr.ChildNodes[1].InnerText.ToLower().Contains("pass"),
                        Link = urlPrefix + tr.ChildNodes[1].ChildNodes[1].Attributes[0].Value,
                        ReasonOfFailure = "\"" + tr.ChildNodes[2].InnerText.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"",
                        TestDatetime = DateTime.Parse(tr.ChildNodes[3].InnerText.Replace("\r", string.Empty).Replace("\n", string.Empty)),
                        ExecutionTime = double.Parse(tr.ChildNodes[4].InnerText)
                    });
                }
                catch
                {
                    return;
                }
            });

            if (tds.Count != testEntities.Count)
            {
                Console.WriteLine("Something wrong at index: " + index + "line: ");
            }
                
            return testEntities;
        }
    }
}
