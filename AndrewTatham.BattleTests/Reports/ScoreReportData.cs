using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using AndrewTatham.BattleTests.Fixtures;
using AndrewTatham.BattleTests.TestCases;
using AndrewTatham.Helpers;

namespace AndrewTatham.BattleTests.Reports
{
    public class ScoreReportData
    {
        public string WeeklyTotalChart { get; private set; }

        public string DailyTotalChart { get; private set; }

        public string WeeklyWinRatioChart { get; private set; }

        public string DailyWinRatioChart { get; private set; }

        public string WeeklyOneVsOneClassificationChart { get; private set; }

        public string WeeklyMeleeClassificationChart { get; private set; }

        public string DailyOneVsOneClassificationChart { get; private set; }

        public string DailyMeleeClassificationChart { get; private set; }

        public string TableData { get; private set; }

        public ScoreReportData(IEnumerable<Outcome> outcomes)
        {
            var jss = new JavaScriptSerializer();

            var utcNow = DateTime.UtcNow;

            DailyTotalChart = jss.Serialize(outcomes.PivotChart(
                () => "Day",
                k1 => GetDay(utcNow, k1.TimeStamp),
                l1 => string.Format("Day {0}", l1),
                k2 => k2.BattleType,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0d));
            WeeklyTotalChart = jss.Serialize(outcomes.PivotChart(
                () => "Week",
                k1 => GetWeek(utcNow, k1.TimeStamp),
                l1 => string.Format("Week {0}", l1),
                k2 => k2.BattleType,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0d));

            WeeklyWinRatioChart = jss.Serialize(outcomes.PivotChart(
                () => "Week",
                k1 => GetWeek(utcNow, k1.TimeStamp),
                l1 => string.Format("Week {0}", l1),
                k2 => k2.BattleType,
                l2 => l2.ToString(),
                agg => new Score(agg).WinRatio.GetValueOrDefault(),
                () => 0d));

            DailyWinRatioChart = jss.Serialize(outcomes.PivotChart(
                 () => "Day",
                 k1 => GetDay(utcNow, k1.TimeStamp),
                 l1 => string.Format("Day {0}", l1),
                 k2 => k2.BattleType,
                 l2 => l2.ToString(),
                 agg => new Score(agg).WinRatio.GetValueOrDefault(),
                 () => 0d));

            var weeklyClassification = outcomes.GroupBy(o => new
            {
                o.MyRobotName,
                o.BattleType,
                o.EnemyName,
                WeekNo = GetWeek(utcNow, o.TimeStamp)
            })
            .Select(g => new
            {
                g.Key.BattleType,
                g.Key.WeekNo,
                new Score(g).Classification
            });

            var dailyClassification = outcomes
                .GroupBy(o => new
            {
                o.MyRobotName,
                o.BattleType,
                o.EnemyName,
                DayNo = GetDay(utcNow, o.TimeStamp)
            })
         .Select(g => new
         {
             g.Key.BattleType,
             g.Key.DayNo,
             new Score(g).Classification
         });

            WeeklyOneVsOneClassificationChart = jss.Serialize(weeklyClassification
                .Where(c => c.BattleType == BattleType.OneVsOne)
                .PivotChart(
                () => "Week",
                k1 => k1.WeekNo,
                l1 => string.Format("Week {0}", l1),
                k2 => k2.Classification,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0));
            WeeklyMeleeClassificationChart = jss.Serialize(weeklyClassification
                .Where(c => c.BattleType == BattleType.Melee)
                .PivotChart(
                () => "Week",
                k1 => k1.WeekNo,
                l1 => string.Format("Week {0}", l1),
                k2 => k2.Classification,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0));

            DailyOneVsOneClassificationChart = jss.Serialize(dailyClassification
                .Where(c => c.BattleType == BattleType.OneVsOne)
                .PivotChart(
                () => "Day",
                k1 => k1.DayNo,
                l1 => string.Format("Day {0}", l1),
                k2 => k2.Classification,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0));
            DailyMeleeClassificationChart = jss.Serialize(dailyClassification
                .Where(c => c.BattleType == BattleType.Melee)
                .PivotChart(
                () => "Day",
                k1 => k1.DayNo,
                l1 => string.Format("Day {0}", l1),
                k2 => k2.Classification,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0));

            TableData = jss.Serialize(outcomes

                .PivotChart(
                () => "Enemy",
                k1 => new { k1.MyRobotName, k1.EnemyName, k1.BattleType },
                l1 => string.Format("{0} {1}", l1.EnemyName, l1.BattleType),
                k2 => k2.OutcomeType,
                l2 => l2.ToString(),
                agg => agg.Count(),
                () => 0));
        }

        private int GetDay(DateTime utcNow, DateTime dateTime)
        {
            return Math.Max(0, (int)(utcNow - dateTime).TotalDays);
        }

        private int GetWeek(DateTime utcNow, DateTime dateTime)
        {
            return Math.Max(0, (int)(utcNow - dateTime).TotalDays / 7);
        }
    }

    //public class ScoreKey
    //{
    //    public string MyRobotName { get; set; }
    //    public BattleType BattleType { get; set; }
    //    public string EnemyName { get; set; }// optional
    //    public int? WeekNo { get; set; } // optional

    //    public override bool Equals(object obj)
    //    {
    //        var other = obj as ScoreKey;
    //        return MyRobotName == other.MyRobotName
    //            && BattleType == other.BattleType
    //            && EnemyName == other.EnemyName
    //            && WeekNo == other.WeekNo;
    //    }
    //    public override int GetHashCode()
    //    {
    //        return MyRobotName.GetHashCode()
    //            ^ BattleType.GetHashCode()
    //            ^ EnemyName.GetHashCode()
    //            ^ WeekNo.GetHashCode()
    //                ;
    //    }

    //}
}