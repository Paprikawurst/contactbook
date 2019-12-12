using ContactbookLogicLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactbookLogicLibraryTests
{
    [TestClass]
    public class InputCheckerTest
    {

        //NOEMPTYINPUTCHECK
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

        //MAILFORMATCHECK
        [TestMethod]
        public void MailFormatCheckRightInput()
        {
            bool CorrectMailAddress = false;
            string testString = "a@b.c";

            CorrectMailAddress = InputChecker.MailFormatCheck(testString);

            Assert.IsTrue(CorrectMailAddress);
        }

        [TestMethod]
        public void MailFormatCheckNullInput()
        {
            bool CorrectMailAddress = false;
            string testString = null;

            CorrectMailAddress = InputChecker.MailFormatCheck(testString);

            Assert.IsFalse(CorrectMailAddress);
        }

        public void MailFormatCheckEmptyStringInput()
        {
            bool CorrectMailAddress = false;
            string testString = "";

            CorrectMailAddress = InputChecker.MailFormatCheck(testString);

            Assert.IsFalse(CorrectMailAddress);
        }

        //GENDERCHECK
        [TestMethod]
        public void GenderCheckWrongInput()
        {
            bool CorrectGender = false;
            string testString = "test";

            CorrectGender = InputChecker.GenderCheck(testString);

            Assert.IsFalse(CorrectGender);
        }

        [TestMethod]
        public void GenderCheckMaleInput()
        {
            bool CorrectGender = false;
            string testString = "Male";

            CorrectGender = InputChecker.GenderCheck(testString);

            Assert.IsTrue(CorrectGender);
        }

        [TestMethod]
        public void GenderCheckFemaleInput()
        {
            bool CorrectGender = false;
            string testString = "Female";

            CorrectGender = InputChecker.GenderCheck(testString);

            Assert.IsTrue(CorrectGender);
        }

        [TestMethod]
        public void GenderCheckNullInput()
        {
            bool CorrectGender = false;
            string testString = null;

            CorrectGender = InputChecker.GenderCheck(testString);

            Assert.IsFalse(CorrectGender);
        }

        [TestMethod]
        public void MailFormatCheckWrongInput()
        {
            bool CorrectMailAddress = false;
            string testString = "test";

            CorrectMailAddress = InputChecker.MailFormatCheck(testString);

            Assert.IsFalse(CorrectMailAddress);
        }
    }
}
