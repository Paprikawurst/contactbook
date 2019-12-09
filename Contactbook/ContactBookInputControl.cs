using System;

namespace Contactbook
{
    public static class ContactBookInputControl
    {
        // Contact Command
        public static void ContactCommand(ContactBook contactbook)
        {
            Console.WriteLine("\nPlease enter the Name of the new Contact.");
            var name = InputChecker.NameCheck();

            ContactData.Location location = LocationCommand(contactbook);

            Console.WriteLine("\nPlease enter the phone number for the new Contact");
            long phoneNumber = InputChecker.PhoneNumberCheck();

            Console.WriteLine("\nPlease enter a mail adress for the new Contact");
            var mailAdress = InputChecker.MailFormatCheck();

            Console.WriteLine("\nPlease enter whether the contact is male or female\n1. male\n2. female");
            var genderinput = Console.ReadLine();


            var contactIndexNumber = 0;

            foreach (var i in contactbook.contactsList)
                contactIndexNumber = contactIndexNumber + 1;

            contactbook.AddContact(contactIndexNumber, name, location, phoneNumber, mailAdress, genderinput); // Add contact if not duplicate
        }
        // Location Command
        public static ContactData.Location LocationCommand(ContactBook contactbook)
        {
            Console.WriteLine("\nPlease enter the street and house number of the new Contact.");
            var adress = InputChecker.AdressCheck();

            Console.WriteLine("\nPlease enter the city of the new contact");
            var cityName = InputChecker.NameCheck();

            var locationIndexNumber = 0;
            foreach (var i in contactbook.locationsList)
            {
                locationIndexNumber = locationIndexNumber + 1;
            }

            ContactData.Location location = contactbook.GetOrAddLocation(locationIndexNumber, adress, cityName); // Add location if not duplicate and get location
            return location;
        }

        // Edit Contact Command
        public static void EditContactCommand(ContactBook contactbook)
        {
            Console.WriteLine("\nPlease enter the index of the contact you wish to edit.");

            foreach (var o in contactbook.contactsList)
            {
                Console.WriteLine($"{o.ContactIndexNumber + 1}, {o.ContactName}, {o.Location.Adress}, {o.Location.City.CityName}, {o.PhoneNumber}, {o.MailAdress}");
            }
            Console.WriteLine("");

            bool z = int.TryParse(Console.ReadLine(), out int inputindex);

            if (z && inputindex <= contactbook.contactsList.Count && inputindex > 0)
            {
                inputindex = inputindex - 1;

                Console.WriteLine("\nPlease enter the number of what you want to edit.\n1. Name\n2. Adress\n3. City\n4. PhoneNumber\n5. MailAdress");
                var c = Console.ReadLine();
                if (c == "1" || c == "2" || c == "3" || c == "4" || c == "5")
                {
                    contactbook.EditContact(inputindex, c);
                }
                else
                    Console.WriteLine("Invalid Input.");
            }
            else
                Console.WriteLine("Invalid Index.");
        }

        // Edit Location Command
        public static void EditLocationCommand(ContactBook contactbook)
        {
            Console.WriteLine($"Please enter the index of the location you wish to edit.");

            foreach (var o in contactbook.locationsList)
                Console.WriteLine($"{o.LocationIndexNumber + 1}, {o.Adress}, {o.City.CityName}");

            bool z = int.TryParse(Console.ReadLine(), out int inputindex);

            if (z && inputindex > 0 && (inputindex <= contactbook.locationsList.Count || inputindex <= contactbook.contactsList.Count))
            {
                inputindex = inputindex - 1;

                Console.WriteLine("Please enter the number of what you want to edit.\n1. Adress\n2. City\n");
                var c = Console.ReadLine();
                if (c == "1" || c == "2")
                    contactbook.EditLocation(inputindex, c);
                else
                    Console.WriteLine("Invalid Input.");
            }
            else
                Console.WriteLine("Invalid Index.");
        }

        // Merge Command
        public static void MergeCommand(ContactBook contactbook)
        {
            Console.WriteLine("Please enter the index of the contact you want to merge with another.");

            foreach (var o in contactbook.contactsList)
                Console.WriteLine($"{o.ContactIndexNumber + 1}, {o.ContactName}, {o.Location.Adress}, {o.Location.City.CityName}");

            bool z = int.TryParse(Console.ReadLine(), out int temp1);

            if (z)
            {
                temp1 = temp1 - 1;
                Console.WriteLine($"Please enter the index of the contact that you want to merge {contactbook.contactsList[temp1].ContactName}," +
                                  $" {contactbook.contactsList[temp1].Location.Adress}," +
                                  $" {contactbook.contactsList[temp1].Location.City.CityName} with.\n");

                bool y = int.TryParse(Console.ReadLine(), out int temp2);
                if (y)
                {
                    temp2 = temp2 - 1;
                    contactbook.MergeContacts(temp1, temp2);
                }
                else
                    Console.WriteLine("WARNING: Invalid Input!");
            }
        }

        public static void RemoveContactCommand(ContactBook contactbook)
        {
            Console.WriteLine("Please enter the index of the contact you want to remove.");
            foreach (var o in contactbook.contactsList)
                Console.WriteLine($"{o.ContactIndexNumber + 1}, {o.ContactName}, {o.Location.Adress}, {o.Location.City.CityName}, {o.PhoneNumber}, {o.MailAdress}");

            var input = Console.ReadLine();
            bool numberCheck = int.TryParse(input, out var value);
            value = value - 1;
            if (numberCheck && value <= contactbook.contactsList.Count)
            {
                contactbook.RemoveContact(value);
            }
            else
                Console.WriteLine($"WARNING: There is no contact with index '{input}'.");
        }

        public static void RemoveLocationCommand(ContactBook contactbook)
        {
            Console.WriteLine("Please enter the index of the location you want to remove.");
            foreach (var o in contactbook.locationsList)
                Console.WriteLine($"{o.LocationIndexNumber + 1}, {o.City.CityName}, {o.Adress}");

            var input = Console.ReadLine();
            bool numberCheck = int.TryParse(input, out var value);
            if (numberCheck && value <= contactbook.locationsList.Count && value >= 0)
            {
                value = value - 1;
                contactbook.RemoveLocation(value);
            }
            else
                Console.WriteLine($"WARNING: There is no location with index '{input}'.");
        }
    }
}
