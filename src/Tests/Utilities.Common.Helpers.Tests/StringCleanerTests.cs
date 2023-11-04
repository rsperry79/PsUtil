using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utilities.Common.Helpers.Tests
{
    /// <summary>
    /// Defines the <see cref="StringCleanerTests" />
    /// </summary>
    [TestClass]
    public class StringCleanerTests
    {
        /// <summary>
        /// The Initialize
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            FunctionsAssemblyResolver.RedirectAssembly(); // needed for .net standard libraries
        }

        /// <summary>
        /// The CleanControlAndSurrogateTests
        /// </summary>
        [TestMethod]
        public void CleanControlAndSurrogateTests()
        {
            // TODO test better
            string toTest = "Lung ching tea or Longjing tea (龙井茶; lóngjǐng chá) is a specialty green tea from China. The name means “dragon well”, from the name of the well that traditionally provided the best water for making this tea.	Lung Ching Dragonwell. Continue Shopping or View Cart. Lung Ching, meaning ‘Dragonwell’, is a famous speciality Chinese green tea, named after the famous Dragon’s Well in Zhejiang, where the tea was first made. It has an emerald leaf and a remarkably fresh but mellow taste.";
            string results = StringCleaner.CleanControlAndSurrogates(toTest);

            Assert.AreNotEqual(toTest, results);
        }
    }
}
