using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HistoryTestFinder.Business
{
    public class CsvBuilder
    {
        const string headline = "Name, Passed or Failed, Date, Execution time, Reason of failure, Link";
        StringBuilder csv = new StringBuilder();
        string fullPathName = "";

        public CsvBuilder(string location, string testName)
        {
            fullPathName = Path.Combine(location, "report " + testName + ".csv");
            csv.AppendLine(headline);            
        }

        public void AppendLine(string line)
        {
            csv.AppendLine(line);
        }

        public void Write()
        {
            File.WriteAllText(fullPathName, csv.ToString());
            csv.Clear();
        }
    }
}
