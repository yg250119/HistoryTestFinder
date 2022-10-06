using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HistoryTestFinder.Business
{
    public static class Program
    {
        public static void Execute(List<string> tests)
        {
            var location = "C:\\Reports";
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }


            Dictionary<string, List<TestEntity>> testEntities = new Dictionary<string, List<TestEntity>>();
            Dictionary<string, CsvBuilder> csvBuilder = new Dictionary<string, CsvBuilder>();

            foreach (var testName in tests)
            {
                testEntities.Add(testName, new List<TestEntity>());

                csvBuilder.Add(testName, new CsvBuilder(location, testName));
            }


            Dictionary<int, HtmlDocument> htmlDocuments = new Dictionary<int, HtmlDocument>();
            PageLoader pageLoader = new PageLoader(@"https://storage.googleapis.com/com-emerald-ccmoffice-cug01-qa_cloudbuild/newman-hack8-tests-nightly/10.10.");
            htmlDocuments = pageLoader.LoadPageFromWeb(1000,4000);

            Parallel.ForEach(tests, testName =>
            {
                //There is a bug here, it make duplicate lines
                HtmlPageExtractor pageExtractor = new HtmlPageExtractor(testName);
                Parallel.ForEach(htmlDocuments, htmlDocument =>
                {
                    testEntities[testName].AddRange(pageExtractor.Parse(htmlDocument.Value, htmlDocument.Key));
                });


                testEntities[testName] = testEntities[testName].OrderBy(x => x.Id).ToList();
                foreach (var testEntity in testEntities[testName])
                {
                    csvBuilder[testName].AppendLine(testEntity.ToString());
                }
                csvBuilder[testName].Write();
            });
        }
    }
}
