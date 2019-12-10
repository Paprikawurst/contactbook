using System;

namespace ContactbookLogicLibrary
{
    public static class InputChecker
    {
        // TODO: inputchecker für übergebenen string (true false? - erneute eingabe?) console.readline ablösen + separate console output

        //ADDRESS CHECK
        public static string NoEmptyInputCheck()
        {
            var input = "";
            bool InputNotEmpty = false;

            while (!InputNotEmpty)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    InputNotEmpty = true;
                }
                else
                    Console.WriteLine("\nWARNING: Input can not be empty!\n");
            }
            return input;
        }

        //MAILFORMAT CHECK
        public static string MailFormatCheck()
        {
            var mailAddress = "";
            bool mailAddressIsMailAddress = false;

            while (!mailAddressIsMailAddress)
            {
                var input = Console.ReadLine();
                if (input.Contains("@") && input.Contains(".") && (input.IndexOf("@") > 0) && (input.IndexOf("@") < input.IndexOf("."))
                    && input.IndexOf(".") > input.IndexOf("@") + 1 && (input.Length - 1 > input.IndexOf("."))) // Format muss a@b.c sein
                {
                    mailAddressIsMailAddress = true;
                    mailAddress = input;
                }
                else
                    Console.WriteLine("\nWARNING: Wrong Format. Correct Example: a@b.c\n");
            }
            return mailAddress;
        }

        //PHONENUMBERCHECK
        public static long PhoneNumberCheck()
        {
            long phoneNumber = 0;
            bool phoneNumberIsNumber = false;
            while (!phoneNumberIsNumber)
            {
                string check1 = Console.ReadLine();

                if (long.TryParse(check1, out long value))
                {
                    phoneNumber = value;
                    phoneNumberIsNumber = true;
                }

                else
                    Console.WriteLine("\nWARNING: Only numbers are allowed.\n");
            }
            return phoneNumber;
        }

        public static string GenderCheck()
        {
            string gender = "";
            bool genderIsGender = false;
            while (!genderIsGender)
            {
                string gender1 = Console.ReadLine();

                if (gender1 == "Male" || gender1 == "Female")
                {
                    gender = gender1;
                    genderIsGender = true;
                }
                else
                {
                    Console.WriteLine("\nWARNING: Only 'Male' and 'Female' gender is allowed at the moment.\n");
                }
            }
            return gender;
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
