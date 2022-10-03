using HistoryFinder.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> tests = new List<string>
                {
                    //"01_Add_items_and_get_one_free",
                    //"02_Proximity_Message",
                    //"03_5perc_Discount_on_Total_ticket_Free_item"
                    "03_xlr_8539_ea_item_quantity_including_fraction",
                    //"01_xlr_9406_CouponReducesTaxAmountBrooklyn"
                };

            const string headline = "Name, Passed or Failed, Date, Execution time, Reason of failure, Link";

            Dictionary<string, List<TestEntity>> testEntities = new Dictionary<string, List<TestEntity>>();
            Dictionary<string, StringBuilder> csv = new Dictionary<string, StringBuilder>();

            foreach (var testName in tests)
            {
                testEntities.Add(testName, new List<TestEntity>());
                csv.Add(testName, new StringBuilder());
            }

            Dictionary<int, HtmlDocument> htmlDocuments = new Dictionary<int, HtmlDocument>();
            var web = new HtmlWeb();
            Parallel.For(1000, 3090, i =>
            {
                Console.WriteLine(i);
                var urlPrefix = @"https://storage.googleapis.com/com-emerald-ccmoffice-cug01-qa_cloudbuild/newman-hack8-tests-nightly/10.10." +
                i.ToString() + @"/";

                var doc = web.Load(urlPrefix + @"ExecutionReport.html");
                if (doc.DocumentNode.ChildNodes.Count == 0)
                    return;
                htmlDocuments.Add(i, doc);
            });

            Parallel.ForEach(tests, testName =>
            {
                //if folder "C\\Reports" doesn't exist, create


                var filePath = "C:\\Reports\\report " + testName + ".csv";
                csv[testName].AppendLine(headline);
                Console.WriteLine(testName);
                Console.WriteLine();


                Parallel.ForEach(htmlDocuments, htmlDocument =>
                {
                    var i = htmlDocument.Key;
                    Console.WriteLine(testName + " - " + i);

                    var doc = htmlDocument.Value;
                    var td = doc.DocumentNode.SelectNodes("//tr//td").Where(x => x.InnerText.Contains(testName)).FirstOrDefault();
                    if (td == null)
                        return;
                    try
                    {
                        var tr = td.ParentNode;
                        var urlPrefix = @"https://storage.googleapis.com/com-emerald-ccmoffice-cug01-qa_cloudbuild/newman-hack8-tests-nightly/10.10." +
                            i.ToString() + @"/";
                        testEntities[testName].Add(new TestEntity
                        {
                            Id = i,
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
                testEntities[testName] = testEntities[testName].OrderBy(x => x.Id).ToList();
                foreach (var testEntity in testEntities[testName])
                {
                    csv[testName].AppendLine(testEntity.ToString());
                }
                File.WriteAllText(filePath, csv[testName].ToString());
                csv[testName].Clear();
            });

            Console.WriteLine("For loop ended");
        }
    }
}

