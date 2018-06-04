using System;
using System.Collections.Generic;
using AndrewTatham.BattleTests.Fixtures;
using AndrewTatham.BattleTests.Reports;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.BattleTests.Reports
{
    [TestFixture]
    public class ScoreReportDataUnitTestFixture
    {
        [Test]
        public void Test()
        {
            var utcNow = DateTime.UtcNow;
            var outcomes = new List<Outcome>
            {
                new Outcome
                {
                    MyRobotName = "1",
                    BattleType = BattleType.OneVsOne,
                    TimeStamp = utcNow,
                    OutcomeType = OutcomeType.Won
                },

                new Outcome
                {
                    MyRobotName = "1",
                    BattleType = BattleType.Melee,
                    TimeStamp = utcNow,
                    OutcomeType = OutcomeType.Lost
                }
            };

            var scores = new ScoreReportData(outcomes);

            Assert.IsNotNullOrEmpty(scores.WeeklyTotalChart);
            Assert.IsNotNullOrEmpty(scores.DailyTotalChart);

            Assert.IsNotNullOrEmpty(scores.WeeklyWinRatioChart);
            Assert.IsNotNullOrEmpty(scores.DailyWinRatioChart);

            Assert.IsNotNullOrEmpty(scores.WeeklyOneVsOneClassificationChart);
            Assert.IsNotNullOrEmpty(scores.WeeklyMeleeClassificationChart);
            Assert.IsNotNullOrEmpty(scores.DailyOneVsOneClassificationChart);
            Assert.IsNotNullOrEmpty(scores.DailyMeleeClassificationChart);
        }
    }
}