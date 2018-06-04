using NUnit.Framework;

namespace AndrewTatham.UnitTests.BattleTests.TestCases
{
    [TestFixture]
    public class ClassifierUnitTestFixture
    {
        /*
        public class ClassifierTestCase
        {
            public IEnumerable<string> myRobots { get; set; }
            public IEnumerable<string> allRobots { get; set; }
            public IEnumerable<Score> scores { get; set; }
            public Dictionary<ClassificationKey, ClassificationValue> Expected { get; set; }
        }

        public static IEnumerable<ClassifierTestCase> TestCases = new List<ClassifierTestCase>()
        {
        };

        [Test, TestCaseSource("TestCases")]
        public void ClassifierTest(ClassifierTestCase testcase)
        {
            var c = new Classifier(
                testcase.myRobots,
                testcase.allRobots,
                new ScoreBoard(testcase.scores));

            CollectionAssert.AreEquivalent(testcase.Expected, c.Classifications);
        }
         * */
    }
}