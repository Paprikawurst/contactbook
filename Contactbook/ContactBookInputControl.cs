using System;

namespace Contactbook
{
    public static class ContactBookInputControl
    {
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
    }
}
