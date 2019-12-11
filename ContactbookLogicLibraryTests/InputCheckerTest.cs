using ContactbookLogicLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactbookLogicLibraryTests
{
    [TestClass]
    public class InputCheckerTest
    {
        [TestMethod]
        //TODOL: write unittest
        public void NoEmptyInputCheckGetsInput()
        {
            //ARRANGE
            bool NoEmptyInput = false;
            string testString = "12345";

            //ACT
            NoEmptyInput = InputChecker.NoEmptyInputCheck(testString);

            //ASSERT
            Assert.IsTrue(NoEmptyInput);
        }

        [TestMethod]
        public void NoEmptyInputCheckEmptyInput()
        {
            //ARRANGE
            bool NoEmptyInput = false;
            string testString = "";

            //ACT
            NoEmptyInput = InputChecker.NoEmptyInputCheck(testString);

            //ASSERT
            Assert.IsFalse(NoEmptyInput);
        }

        [TestMethod]
        public void NoEmptyInputCheckNullInput()
        {
            //ARRANGE
            bool NoEmptyInput = false;
            string testString = null;

            //ACT
            NoEmptyInput = InputChecker.NoEmptyInputCheck(testString);

            //ASSERT
            Assert.IsFalse(NoEmptyInput);
        }


    }
}
