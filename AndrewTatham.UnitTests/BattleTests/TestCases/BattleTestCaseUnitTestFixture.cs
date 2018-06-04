using AndrewTatham.BattleTests.TestCases;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.BattleTests.TestCases
{
    [TestFixture]
    public class BattleTestCaseUnitTestFixture
    {
        [Test]
        public void Serialization()
        {

            string expectedMe = "A";
            string[] enemies = { "1", "2" };

            var btc = new BattleTestCase(expectedMe, enemies);

            var serialized = btc.Serialize();

            Assert.IsNotNullOrEmpty(serialized);

            var deserialized = BattleTestCase.Deserialize(serialized);

            Assert.IsNotNull(deserialized);

        
        }
    }
}
