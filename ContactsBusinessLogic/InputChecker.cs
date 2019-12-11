namespace ContactbookLogicLibrary
{
    public static class InputChecker
    {
        //NO EMPTY INPUT CHECK
        public static bool NoEmptyInputCheck(string input)
        {
            bool InputNotEmpty = false;

            if (!string.IsNullOrWhiteSpace(input))
            {
                InputNotEmpty = true;
            }
            return InputNotEmpty;
        }

        //MAILFORMAT CHECK
        public static bool MailFormatCheck(string input)
        {
            bool mailAddressIsMailAddress = false;

            if (input.Contains("@") && input.Contains(".") && (input.IndexOf("@") > 0) && (input.IndexOf("@") < input.IndexOf("."))
                && input.IndexOf(".") > input.IndexOf("@") + 1 && (input.Length - 1 > input.IndexOf("."))) // Format muss a@b.c sein
            {
                mailAddressIsMailAddress = true;
            }
            return mailAddressIsMailAddress;
        }


        //GENDER CHECK
        public static bool GenderCheck(string input)
        {
            bool genderIsGender = false;

            if (input == "Male" || input == "Female")
                genderIsGender = true;

            return genderIsGender;
        }

        //CSV EMPTY INPUT CHECK
        public static string CsvEmptyInputCheck(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                input = "wronginput";
                return input;
            }
            else
                return input;
        }


        //CSV GENDER CHECK
        public static string CsvGenderCheck(string input)
        {
            if (input == "Male" || input == "Female")
            {
                return input;
            }
            else
            {
                input = "wronginput";
                return input;
            }
        }

        //CSVMAILFORMAT CHECK
        public static string CsvMailFormatCheck(string input)
        {
            var mailAddress = "";
            bool mailAddressIsMailAddress = false;
            while (!mailAddressIsMailAddress)
            {
                if (input.Contains("@") && input.Contains(".") && (input.IndexOf("@") > 0) && (input.IndexOf("@") < input.IndexOf("."))
                  && input.IndexOf(".") > input.IndexOf("@") + 1 && (input.Length - 1 > input.IndexOf("."))) // Format muss a@b.c sein
                {
                    mailAddressIsMailAddress = true;
                    mailAddress = input;
                }
                else
                {
                    mailAddress = "";
                    mailAddressIsMailAddress = true;
                }
            }
            return mailAddress;
        }
    }
}
