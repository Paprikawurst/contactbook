using ContactbookLogicLibrary;
using ContactData;
using System;

namespace ContactbookConsole
{
    public static class ContactbookConsoleInputControl
    {

        // Add Contact Command
        public static void AddContactCommand(ContactBookLogic contactbooklogic, SQLConnection sql, long countLocations)
        {
            bool nameIsCorrectInput = false;
            bool inputIsNumber = false;
            bool mailAddressIsCorrectInput = false;
            bool genderIsCorrectInput = false;
            var name = "";
            var mailAddress = "";
            long phoneNumber = 0;
            var gender = "";


            while (!nameIsCorrectInput)
            {
                Console.WriteLine("\nPlease enter the Name of the new Contact.");
                name = Console.ReadLine();
                nameIsCorrectInput = InputChecker.NoEmptyInputCheck(name);
            }

            Location location = AddOrGetLocationCommand(contactbooklogic, sql, countLocations);

            while (!inputIsNumber)
            {
                Console.WriteLine("\nPlease enter the phone number for the new Contact");
                inputIsNumber = long.TryParse(Console.ReadLine(), out phoneNumber);
            }

            while (!mailAddressIsCorrectInput)
            {
                Console.WriteLine("\nPlease enter a mailaddress for the new Contact");
                mailAddress = Console.ReadLine();
                mailAddressIsCorrectInput = InputChecker.MailFormatCheck(mailAddress);
            }

            while (!genderIsCorrectInput)
            {
                Console.WriteLine("\nPlease enter the gender for the new Contact ('Male' or 'Female')");
                gender = Console.ReadLine();
                genderIsCorrectInput = InputChecker.GenderCheck(gender);
            }

            bool conWasDupe = contactbooklogic.AddContact(contactbooklogic, sql, countLocations, name, location, phoneNumber, mailAddress, gender);
            if (conWasDupe)
            {
                Console.WriteLine("INFO: Contact was a duplicate and has not been added to the database.");
            }
        }

        //Add or Get Location Command
        public static Location AddOrGetLocationCommand(ContactBookLogic contactbooklogic, SQLConnection sql, long countLocations)
        {
            bool addressIsCorrectInput = false;
            bool cityNameIsCorrectInput = false;
            var address = "";
            var cityName = "";

            while (!addressIsCorrectInput)
            {
                Console.WriteLine("\nPlease enter the address of the new contact.");
                address = Console.ReadLine();
                addressIsCorrectInput = InputChecker.NoEmptyInputCheck(address);
            }

            while (!cityNameIsCorrectInput)
            {
                Console.WriteLine("\nPlease enter the cityname of the new contact.");
                cityName = Console.ReadLine();
                cityNameIsCorrectInput = InputChecker.NoEmptyInputCheck(cityName);
            }

            Location location = contactbooklogic.AddOrGetLocation(contactbooklogic, sql, countLocations, address, cityName);

            return location;
        }

        // Edit Contact Command
        public static void EditContactCommand(ContactBookLogic contactbooklogic, SQLConnection sql, long countContacts)
        {
            Console.WriteLine("\nPlease enter the index of the contact you wish to edit.");
            sql.ReadContactsTable();

            bool correctIndex = int.TryParse(Console.ReadLine(), out int inputindex);
            if (correctIndex && inputindex <= countContacts && inputindex > 0)
            {
                var newValue = "";
                bool newValueIsCorrectInput = false;
                var CommandText = "";
                bool inputIsNumber = false;
                long phoneNumber = 0;

                Console.WriteLine("\nPlease enter the number of what you want to edit.\n1. Name\n2. PhoneNumber\n3. MailAddress");
                var c = Console.ReadLine();
                if (c == "1")
                {
                    while (!newValueIsCorrectInput)
                    {
                        Console.WriteLine($"Please enter the new value for the name.");
                        var name = Console.ReadLine();
                        newValueIsCorrectInput = InputChecker.NoEmptyInputCheck(newValue);
                    }

                    CommandText = $"UPDATE contacts SET Name = '{newValue}' WHERE ContactID = {inputindex};";
                    sql.ExecuteNonQuery(CommandText);
                }
                else if (c == "2")
                {
                    while (!inputIsNumber)
                    {
                        Console.WriteLine("\nPlease enter the phone number for the new Contact");
                        inputIsNumber = long.TryParse(Console.ReadLine(), out phoneNumber);
                    }

                    CommandText = $"UPDATE contacts SET phoneNumber = '{phoneNumber}' WHERE ContactID = {inputindex};";
                    sql.ExecuteNonQuery(CommandText);
                }
                else if (c == "3")
                {
                    while (!newValueIsCorrectInput)
                    {
                        Console.WriteLine($"Please enter the new value for the Mailaddress.");
                        var mailAddress = Console.ReadLine();
                        newValueIsCorrectInput = InputChecker.MailFormatCheck(mailAddress);
                    }

                    CommandText = $"UPDATE contacts SET MailAddress = '{newValue}' WHERE ContactID = {inputindex};";
                    sql.ExecuteNonQuery(CommandText);
                }
                else
                    Console.WriteLine("WARNING: Invalid Input.");
            }
            else
                Console.WriteLine("WARNING: Invalid Index.");
        }

        // Edit Location Command
        public static void EditLocationCommand(ContactBookLogic contactbooklogic, SQLConnection sql, long countLocations)
        {
            Console.WriteLine($"Please enter the index of the location you wish to edit.");
            sql.ReadLocationsTable();

            bool z = int.TryParse(Console.ReadLine(), out int inputindex);

            if (z && inputindex <= countLocations && inputindex > 0)
            {

                Console.WriteLine("Please enter the number of what you want to edit.\n1. Address\n2. City\n");
                var c = Console.ReadLine();
                if (c == "1" || c == "2")
                {
                    if (c == "1")
                    {
                        Console.WriteLine($"Please enter the new value for the address.");
                    }
                    else if (c == "2")
                    {
                        Console.WriteLine($"Please enter the new value for the cityname.");
                    }
                    var newvalue = Console.ReadLine(); //TODO: check ob richtiger input statt console.readline
                    contactbooklogic.EditLocation(inputindex, c, sql, newvalue);
                }

                else
                    Console.WriteLine("WARNING: Invalid Input.");
            }
            else
                Console.WriteLine("WARNING: Invalid Index.");
        }

        // Merge Command
        public static void MergeCommand(ContactBookLogic contactbooklogic, SQLConnection sql)
        {
            Console.WriteLine("Please enter the index of the contact which merges his location into a second contact.");

            sql.ReadContactsTable();

            bool z = int.TryParse(Console.ReadLine(), out int temp1);

            if (z)
            {
                Console.WriteLine($"Please enter the index of the contact that you want to merge locations bla bla of the contact with index : {temp1} with.\n");

                bool y = int.TryParse(Console.ReadLine(), out int temp2);
                if (y)
                {
                    contactbooklogic.MergeContacts(temp1, temp2, sql);
                }
                else
                    Console.WriteLine("WARNING: Invalid Input!");
            }
            else
                Console.WriteLine("WARNING: Invalid Index!");
        }


        // Help Command
        public static void HelpCommand()
        {
            Console.WriteLine("\nTyping 'Add' lets you add a new contact or a location to the database.");
            Console.WriteLine("Typing 'Edit' lets you edit any value of a contact or location or merge two existing contacts.");
            Console.WriteLine("Typing 'Remove' lets you remove an existing contact or location from the contactbook.");
            Console.WriteLine("Typing 'List' shows you various List options.");
            Console.WriteLine("Typing 'Quit' closes the program.");
            Console.WriteLine("Typing 'Help' shows this text again.");
            Console.WriteLine("Typing 'Import' lets you import a CSV-File.");
            Console.WriteLine("Typing 'Clear' lets you clear the console screen.");
            Console.WriteLine("'WARNING' indicates wrong input - 'INFO' indicates changed values.\n");
        }
    }
}
