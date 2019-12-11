using ContactbookLogicLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactbookLogicLibraryTests
{
    [TestClass]
    public class InputCheckerTest
    {
        [TestMethod]
        public void NoEmptyInputCheckGetsInput()
        {
            //ARRANGE
            bool NoEmptyInput = false;
            string testString = "test";

            //ACT
            NoEmptyInput = InputChecker.NoEmptyInputCheck(testString);

            //ASSERT
            Assert.IsTrue(NoEmptyInput);
        }

        [TestMethod]
        public void NoEmptyInputCheckEmptyInput()
        {
            bool NoEmptyInput = false;
            string testString = "";

            NoEmptyInput = InputChecker.NoEmptyInputCheck(testString);

            Assert.IsFalse(NoEmptyInput);
        }

        [TestMethod]
        public void NoEmptyInputCheckNullInput()
        {
            bool NoEmptyInput = false;
            string testString = null;

            NoEmptyInput = InputChecker.NoEmptyInputCheck(testString);

            Assert.IsFalse(NoEmptyInput);
        }
    }
}
