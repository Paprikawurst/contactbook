using System;
using System.Collections.Generic;
using System.Linq;
using ContactData;

namespace Contactbook
{
    public static class InputChecker
    {
        //NAME CHECK
        public static string NameCheck()
        {
            var name = "";
            bool nameIsName = false;

            while (!nameIsName)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.All(char.IsLetter))    // kein leerer Input und nur Letter erlaubt
                {
                    nameIsName = true;
                    name = input;
                }
                else
                    Console.WriteLine("\nWARNING: Only alphabetical letters are allowed\n");
            }
            return name;
        }

        //ADRESS CHECK
        public static string AdressCheck()
        {
            var adress = "";
            bool adressIsAdress = false;

            while (!adressIsAdress)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))    // kein leerer Input
                {
                    adressIsAdress = true;
                    adress = input;
                }
                else
                    Console.WriteLine("\nWARNING: Only alphabetical letters are allowed\n");
            }
            return adress;
        }


        //MAILFORMAT CHECK
        public static string MailFormatCheck()
        {
            var mailAdress = "";
            bool mailAdressIsMailAdress = false;

            while (!mailAdressIsMailAdress)
            {
                var input = Console.ReadLine();
                if (input.Contains("@") && input.Contains(".") && (input.IndexOf("@") > 0) && (input.IndexOf("@") < input.IndexOf("."))
                    && input.IndexOf(".") > input.IndexOf("@") + 1 && (input.Length - 1 > input.IndexOf("."))) // Format muss a@b.c sein
                {
                    mailAdressIsMailAdress = true;
                    mailAdress = input;
                }

                else
                    Console.WriteLine("\nWARNING: Wrong Format. Correct Example: a@b.c\n");
            }
            return mailAdress;
        }

        //CSVMAILFORMAT CHECK
        public static string CsvMailFormatCheck(string input)
        {
            var mailAdress = "";
            bool mailAdressIsMailAdress = false;
            while (!mailAdressIsMailAdress)
            {
                if (input.Contains("@") && input.Contains(".") && (input.IndexOf("@") > 0) && (input.IndexOf("@") < input.IndexOf("."))
                  && input.IndexOf(".") > input.IndexOf("@") + 1 && (input.Length - 1 > input.IndexOf("."))) // Format muss a@b.c sein
                {
                    mailAdressIsMailAdress = true;
                    mailAdress = input;
                }
                else
                {
                    mailAdress = "";
                    mailAdressIsMailAdress = true;
                }
            }
            return mailAdress;
        }

        //PHONENUMBERCHECK
        public static long PhoneNumberCheck()
        {
            long phoneNumber = 0;
            bool phoneNumberIsNumber = false;
            while (phoneNumberIsNumber == false)
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

        //CSVPHONENUMBER CHECK
        public static long CsvPhoneNumberCheck(long a)
        {
            long phoneNumber = 0;
            var phoneNumberIsNumber = false;
            while (phoneNumberIsNumber == false)
            {
                string check1 = Console.ReadLine();

                if (long.TryParse(check1, out var value))
                {
                    phoneNumber = value;
                    phoneNumberIsNumber = true;
                }
                else
                {
                    phoneNumber = 0;
                    phoneNumberIsNumber = true;
                }
            }
            return phoneNumber;
        }

        //CONTACTDUPLICATECHECK
        public static bool ContactDuplicateCheck(List<Contact> contactsList, Contact contact)
        {
            bool IsDuplicate = false;
            bool a = false;
            while (!a)
            {
                foreach (var c in contactsList)
                {
                    if (contact.ContactName == c.ContactName && contact.Location.Adress == c.Location.Adress && contact.Location.City.CityName == c.Location.City.CityName
                        && contact.PhoneNumber == c.PhoneNumber && contact.MailAdress == c.MailAdress)
                    {
                        IsDuplicate = true;
                    }
                }
                a = true;
            }
            return IsDuplicate;
        }

        //LOCATIONDUPLICATECHECK
        public static bool LocationDuplicateCheck(List<Location> locationsList, string Adress, City city)
        {
            bool IsDuplicate = false;
            bool a = false;
            while (!a)
            {
                foreach (var l in locationsList)
                {
                    if (Adress == l.Adress && city.CityName == l.City.CityName)
                    {
                        IsDuplicate = true;
                    }
                }
                a = true;
            }
            return IsDuplicate;
        }
    }
}
