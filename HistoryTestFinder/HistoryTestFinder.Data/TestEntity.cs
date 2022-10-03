using System;

namespace HistoryTestFinder
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public bool PassedOrFailed { get; set; }
        public string Link { get; set; }
        public string ReasonOfFailure { get; set; }
        public DateTime TestDatetime { get; set; }
        public double ExecutionTime { get; set; }

        public override string ToString()
        {
            var strPassedOrFailed = PassedOrFailed ? "Pass" : "Fail";
            return string.Format("{0},{1},{2},{3},{4},{5}", TestName, strPassedOrFailed, TestDatetime.Date, ExecutionTime, ReasonOfFailure, Link);
        }

    }
}
