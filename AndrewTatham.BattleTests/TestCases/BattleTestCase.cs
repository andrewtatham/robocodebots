using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using AndrewTatham.BattleTests.Fixtures;
using Robocode.Control;

namespace AndrewTatham.BattleTests.TestCases
{
    public class BattleTestCase
    {
        public BattleTestCase()
        {

        }

        public BattleTestCase(
            string myRobot,
            IEnumerable<string> enemyRobots
            )
        {
            MyRobot = myRobot;
            EnemyRobots = enemyRobots.OrderBy(name => name);
            AllRobots = new[] { MyRobot }.Union(EnemyRobots);
            EnemyRobotsCsv = EnemyRobots.Aggregate((r1, r2) => r1 + "," + r2);
            AllRobotsCsv = AllRobots.Aggregate((r1, r2) => r1 + "," + r2);

            NumberOfRounds = 3;
            BattleType = EnemyRobots.Count() == 1 ? BattleType.OneVsOne : BattleType.Melee;
            BattlefieldSpecification = BattleType == BattleType.OneVsOne
                ? new BattlefieldSpecification(800, 600)
                : new BattlefieldSpecification(1000, 1000);
#if DEBUG
            TimeOut = TimeSpan.FromMinutes(10);
#else
            TimeOut = TimeSpan.FromSeconds(30);
#endif
        }

        public BattleType BattleType { get; set; }

        public string MyRobot { get; set; }

        public IEnumerable<string> EnemyRobots { get; set; }

        public IEnumerable<string> AllRobots { get; set; }

        public string AllRobotsCsv { get; set; }

        public int NumberOfRounds { get; set; }

        public BattlefieldSpecification BattlefieldSpecification { get; set; }

        public TimeSpan TimeOut { get; set; }

  

        public string EnemyRobotsCsv { get; set; }

        public override string ToString()
        {
            return AllRobotsCsv;
        }

        public string Serialize()
        {
            var jss = new JavaScriptSerializer();
            return jss.Serialize(this);
        }

        public static BattleTestCase Deserialize(string data)
        {
            var jss = new JavaScriptSerializer();
            return jss.Deserialize<BattleTestCase>(data);
        }
    }
}