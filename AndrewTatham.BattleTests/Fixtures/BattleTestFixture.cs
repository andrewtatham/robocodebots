using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AndrewTatham.BattleTests.TestCases;
using NUnit.Framework;
using Robocode.Control;
using Robocode.Control.Events;

namespace AndrewTatham.BattleTests.Fixtures
{
    [TestFixture]
    //[Timeout(7200)]
    //#if DEBUG
    //    [Explicit]
    //#else
    //    //[Ignore]
    //#endif
    public class BattleTestFixture
    {
        private static RobocodeEngine _robocode;

        private static int _testCount = 1;
        private static readonly object Obj = new object();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("TestFixtureSetUp Started");
            if (_robocode == null)
            {
                Console.WriteLine("TestFixtureSetUp Creating Robocode Instance");
                _robocode = new RobocodeEngine(@"C:\robocode");
                Assert.IsNotNull(_robocode);
#if DEBUG
                var mn = Environment.MachineName.ToUpper();
                var visibility = new Dictionary<string, bool>
                {
                    { "ANDREWDESKTOP", true },
                    { "ANDREWLAPTOP", true },
                    { "LDSWKS0920", false }
                };

                _robocode.Visible = visibility[mn];
#else
                _robocode.Visible = false;
#endif

                Console.WriteLine("TestFixtureSetUp Subscribing");
                _robocode.BattleCompleted += _robocode_BattleCompleted;
                _robocode.BattleError += _robocode_BattleError;
                _robocode.BattleMessage += _robocode_BattleMessage;
            }

            bool x = TestCaseFactory.CheckForUnfinishedBusiness();
            if (x)
            {
                // ????
            }


            Console.WriteLine("TestFixtureSetUp Done");
        }

        [TearDown]
        public void TearDown()
        {
            lock (Obj)
            {
                if (_testCount++ % 128 == 0)
                {
                    Console.WriteLine("TearDown Report");
                    TestCaseFactory.GenerateScoreReport();
                }
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.WriteLine("TestFixtureTearDown Started");
            if (_robocode != null)
            {
                Console.WriteLine("TestFixtureTearDown Unsubscribing");
                _robocode.BattleCompleted -= _robocode_BattleCompleted;
                _robocode.BattleError -= _robocode_BattleError;
                _robocode.BattleMessage -= _robocode_BattleMessage;

                Console.WriteLine("TestFixtureTearDown Closing Robocode Instance");
                _robocode.Close();
                _robocode = null;
            }

            Console.WriteLine("TestFixtureTearDown Generating score report");
            TestCaseFactory.GenerateScoreReport();

            Console.WriteLine("TestFixtureTearDown Done");
        }

        private static void _robocode_BattleMessage(BattleMessageEvent evnt)
        {
            Console.WriteLine("BattleMessage: " + evnt.Message);
        }

        private AutoResetEvent _wait;
        private readonly List<BattleErrorEvent> _battleErrorEvents = new List<BattleErrorEvent>();
        private BattleCompletedEvent _battleCompletedEvent;

        private void _robocode_BattleError(BattleErrorEvent evnt)
        {
            Console.WriteLine("BattleErrorEvent: " + evnt.Error);
            _battleErrorEvents.Add(evnt);
        }

        private void _robocode_BattleCompleted(BattleCompletedEvent evnt)
        {
            Console.WriteLine("BattleCompletedEvent");
            _battleCompletedEvent = evnt;
            _wait.Set();
        }

        [Test]
        //[Parallelizable]
        //[TimeoutAttribute(0)]
        [Category("Battle")]
        [TestCaseSource(typeof(TestCaseFactory), "TestCases")]
        public void Battle(BattleTestCase testCase)
        {
            TestCaseFactory.SaveToDisk(testCase);


            RobotSpecification[] robots = _robocode.GetLocalRepository(testCase.AllRobotsCsv);

            var battle = new BattleSpecification(
                testCase.NumberOfRounds,
                testCase.BattlefieldSpecification,
                robots);
            _battleErrorEvents.Clear();
            _battleCompletedEvent = null;

            using (_wait = new AutoResetEvent(false))
            {
                _robocode.RunBattle(battle, false);

                _wait.WaitOne(testCase.TimeOut);
            }
            

            bool finished = _battleCompletedEvent != null;

            var adapter = new BattleResultAdapter();

            var outcomes = adapter.RecordOutcomes(
                testCase,
                finished ? _battleCompletedEvent.SortedResults : null,
                _battleErrorEvents.Select(e => e.Error),
                finished);

            TestCaseFactory.AddResult(outcomes);

            TestCaseFactory.ForgetAnyUnfinishedBusiness();


            var grouped = outcomes.GroupBy(o => o.OutcomeType).ToDictionary(g => g.Key);

            if (grouped.ContainsKey(OutcomeType.Lost))
            {
                var robotsThatBeatMe = grouped[OutcomeType.Lost];
                Assert.Fail("Lost to: " + robotsThatBeatMe.Select(e => e.EnemyName).Aggregate((n1, n2) => n1 + ", " + n2));
            }
            else if (grouped.ContainsKey(OutcomeType.Won))
            {
                var robotsThatIBeat = grouped[OutcomeType.Won];
                Console.WriteLine("Beat: " + robotsThatIBeat.Select(e => e.EnemyName).Aggregate((n1, n2) => n1 + ", " + n2));
            }
            else
            {
                Assert.Inconclusive();
            }
        }
    }
}