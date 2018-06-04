using System.Collections.Generic;
using System.Linq;
using AndrewTatham.BattleTests.Fixtures;
using AndrewTatham.BattleTests.TestCases;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests.BattleTests.Fixtures
{
    [TestFixture]
    public class BattleResultAdapterUnitTestFixture
    {
        [Test]
        [TestCase("WonBot")]
        [TestCase("LostBot")]
        [TestCase("NoShowBot")]
        [TestCase("ErrorBot")]
        public void OneVsOne(string enemyName)
        {
            var myRobot = "MyBot";
            var enemyRobotNamess = new[]
            {
                enemyName
            };

            var testCase = new BattleTestCase(myRobot, enemyRobotNamess);

            var errors = enemyName == "ErrorBot" ? new[] { "Blah ErrorBot Blah" } : new string[] { };

            var sortedResults = new List<BattleResults>();
            if (enemyName == "LostBot") sortedResults.Add(new BattleResults("LostBot", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
            sortedResults.Add(new BattleResults("MyBot", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
            if (enemyName == "WonBot") sortedResults.Add(new BattleResults("WonBot", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));



            var adapter = new BattleResultAdapter();

            var outcomes = adapter.RecordOutcomes(testCase, sortedResults, errors, true);

            var o = outcomes.Single();

            Assert.AreEqual(BattleType.OneVsOne, o.BattleType);
            Assert.AreEqual(enemyName, o.EnemyName);
            Assert.AreEqual(myRobot, o.MyRobotName);

            switch (enemyName)
            {
                case "WonBot":
                    Assert.AreEqual(OutcomeType.Won, o.OutcomeType);
                    break;

                case "LostBot":
                    Assert.AreEqual(OutcomeType.Lost, o.OutcomeType);
                    break;

                case "NoShowBot":
                    Assert.AreEqual(OutcomeType.NoShow, o.OutcomeType);
                    break;

                case "ErrorBot":
                    Assert.AreEqual(OutcomeType.Error, o.OutcomeType);
                    Assert.AreEqual("Blah ErrorBot Blah", o.Error);
                    break;

                default:
                    Assert.Fail(enemyName);
                    break;
            }
        }

        [Test]
        public void Melee()
        {
            var myRobot = "MyBot";
            var enemyRobotNamess = new[]
            {
                "WonBot",
                "LostBot",
                "NoShowBot",
                "ErrorBot"
            };

            var testCase = new BattleTestCase(myRobot, enemyRobotNamess);

            var errors = new[] { "Blah ErrorBot Blah" };

            var sortedResults = new[]
            {
                new BattleResults("LostBot",0,0,0,0,0,0,0,0,0,0,0),
                new BattleResults("MyBot",0,0,0,0,0,0,0,0,0,0,0),
                new BattleResults("WonBot",0,0,0,0,0,0,0,0,0,0,0)
            };





            var adapter = new BattleResultAdapter();

            var outcomes = adapter.RecordOutcomes(testCase, sortedResults, errors, true);

        

            //Assert.AreEqual(4, mra.Outcomes.Count());
            var outcomeDic = outcomes.ToDictionary(k => k.EnemyName);

            Assert.AreEqual(OutcomeType.Lost, outcomeDic["LostBot"].OutcomeType);
            Assert.AreEqual(OutcomeType.Won, outcomeDic["WonBot"].OutcomeType);
            Assert.AreEqual(OutcomeType.NoShow, outcomeDic["NoShowBot"].OutcomeType);
            Assert.AreEqual(OutcomeType.Error, outcomeDic["ErrorBot"].OutcomeType);

            Assert.AreEqual("Blah ErrorBot Blah", outcomeDic["ErrorBot"].Error);
        }


        [Test]
        [TestCase("HangBot")]
        [TestCase("HangBot,HangBot,HangBot")]
        public void RecordUnfinishedBusiness(string enemyNames)
        {
            var adapter = new BattleResultAdapter();

            var btc = new BattleTestCase("A", enemyNames.Split(','));

            var outcomes = adapter.RecordUnfinishedBusiness(btc);

            Assert.IsNotNull(outcomes);


        
        }

    }
}