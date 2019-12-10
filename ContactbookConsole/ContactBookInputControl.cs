using ContactData;
using System;

namespace Contactbook
{
    public static class ContactBookInputControl
    {

        // Add Contact Command
        public static void AddContactCommand(ContactBook contactbook, SQLConnection sql, long countLocations)
        {
            Console.WriteLine("\nPlease enter the Name of the new Contact.");
            var name = InputChecker.NoEmptyInputCheck();

            Location location = AddOrGetLocationCommand(contactbook, sql, countLocations);

            Console.WriteLine("\nPlease enter the phone number for the new Contact");
            long phoneNumber = InputChecker.PhoneNumberCheck();

            Console.WriteLine("\nPlease enter a mail address for the new Contact");
            var mailAddress = InputChecker.MailFormatCheck();

            Console.WriteLine("\nPlease enter the gender for the new Contact ('Male' or 'Female')");
            var gender = InputChecker.GenderCheck();

            contactbook.AddContact(contactbook, sql, countLocations, name, location, phoneNumber, mailAddress, gender);
        }

        public static Location AddOrGetLocationCommand(ContactBook contactbook, SQLConnection sql, long countLocations)
        {
            Console.WriteLine("\nPlease enter the address of the new contact.");
            var address = InputChecker.NoEmptyInputCheck();

            Console.WriteLine("\nPlease enter the cityname of the new contact");
            var cityName = InputChecker.NoEmptyInputCheck();

            Location location = contactbook.AddOrGetLocation(contactbook, sql, countLocations, address, cityName);
            return location;
        }

        // Edit Contact Command
        public static void EditContactCommand(ContactBook contactbook, SQLConnection sql, long countContacts)
        {
            Console.WriteLine("\nPlease enter the index of the contact you wish to edit.");
            sql.ReadContactsTable();

            bool z = int.TryParse(Console.ReadLine(), out int inputindex);
            if (z && inputindex <= countContacts && inputindex > 0)
            {

                Console.WriteLine("\nPlease enter the number of what you want to edit.\n1. Name\n2. PhoneNumber\n3. MailAddress");
                var c = Console.ReadLine();
                if (c == "1" || c == "2" || c == "3")
                {
                    contactbook.EditContact(inputindex, c, sql);
                }
                else
                    Console.WriteLine("WARNING: Invalid Input.");
            }
            else
                Console.WriteLine("WARNING: Invalid Index.");
        }

        // Edit Location Command
        public static void EditLocationCommand(ContactBook contactbook, SQLConnection sql, long countLocations)
        {
            Console.WriteLine($"Please enter the index of the location you wish to edit.");
            sql.ReadLocationsTable();

            bool z = int.TryParse(Console.ReadLine(), out int inputindex);

            if (z && inputindex <= countLocations && inputindex > 0)
            {

                Console.WriteLine("Please enter the number of what you want to edit.\n1. Address\n2. City\n");
                var c = Console.ReadLine();
                if (c == "1" || c == "2")
                    contactbook.EditLocation(inputindex, c, sql);
                else
                    Console.WriteLine("WARNING: Invalid Input.");
            }
            else
                Console.WriteLine("WARNING: Invalid Index.");
        }

        // Merge Command
        public static void MergeCommand(ContactBook contactbook, SQLConnection sql)
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
                    contactbook.MergeContacts(temp1, temp2, sql);
                }
                else
                    Console.WriteLine("WARNING: Invalid Input!");
            }
            else
                Console.WriteLine("WARNING: Invalid Index!");
        }

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
