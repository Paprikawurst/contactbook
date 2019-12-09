using System;
using System.Collections.Generic;
using ContactData;

namespace Contactbook
{
    public class ShowList
    {
        public void ListWanted(ContactBook contactbook)
        {
            Console.WriteLine("\nDo you want a list of 1. only contacts,2. only locations or 3. both?\nType 1, 2 or 3\n");
            var v = Console.ReadLine();

            if (v == "1")
            {
                if (contactbook.contactsList.Count > 0)
                    ShowContacts(contactbook);

                else
                    Console.WriteLine("\nWARNING: There are no contacts which could be displayed.\n");
            }
            else if (v == "2")
            {
                if (contactbook.locationsList.Count > 0)
                    ShowLocations(contactbook);

                else
                    Console.WriteLine("\nWARNING: There are no locations which could be displayed.\n");
            }
            else if (v == "3")
            {
                if (contactbook.locationsList.Count > 0 || contactbook.contactsList.Count > 0)
                    ShowBoth(contactbook);

                else
                    Console.WriteLine("\nWARNING: There are neither contacts nor locations which could be displayed.");
            }
            else
                Console.WriteLine("Invalid Input.");
        }

        //only contacts
        public void ShowContacts(ContactBook contactbook)
        {
            Console.WriteLine("\nWhat List of contacts do you want to display?\n1. All contacts\n2. All contacts of a specific city\n3. All cities\n4. All male contacts\n5. All female contacts\nType 1, 2 or 3\n");
            var check = Console.ReadLine();

            if (check == "1")
            {
                Console.WriteLine("\nListing all contacts:\n");
                foreach (var entry in contactbook.contactsList)
                {
                    Console.WriteLine($"{entry.ContactIndexNumber + 1}: {entry.ContactName}, {entry.Location.Adress}, {entry.Location.City.CityName}, {entry.PhoneNumber}, {entry.MailAdress}");
                }
                Console.WriteLine("");
            }
            else if (check == "2")
            {
                Console.WriteLine("\nWhich city do you want to display the contacts from?\nPossible cities are:\n");
                ShowCitiesOfContacts(contactbook);
                Console.WriteLine("");

                var chosenCity = Console.ReadLine();
                var cityList = new List<Contact>();

                foreach (var u1 in contactbook.contactsList)
                {
                    if (chosenCity == u1.Location.City.CityName)
                        cityList.Add(u1);
                }

                if (cityList.Count > 0)
                {
                    Console.WriteLine($"\nThere are {cityList.Count} contacts based in {chosenCity} available:\n");
                    foreach (var entry in cityList)
                        Console.WriteLine($"{entry.ContactIndexNumber + 1}: {entry.ContactName}, {entry.Location.Adress}, {entry.PhoneNumber}, {entry.MailAdress}");
                }
                else
                {
                    Console.WriteLine($"WARNING: There is no city named '{chosenCity}'");
                }
            }
            else if (check == "3")
            {
                Console.WriteLine("\nCities with contacts:\n");
                ShowCitiesOfContacts(contactbook);
            }
            else if (check == "4")
            {
                Console.WriteLine("Male contacts:");
                foreach (Contact contact in contactbook.contactsList)
                    if (contact is Man)
                    {
                        Console.WriteLine($"{contact.ContactIndexNumber + 1}: {contact.ContactName}, {contact.Location.Adress}, {contact.Location.City.CityName}, {contact.PhoneNumber}, {contact.MailAdress}");
                    }
            }
            else if (check == "5")
            {
                Console.WriteLine("Female contacts:");
                foreach (Contact contact in contactbook.contactsList)
                    if (contact is Woman)
                    {
                        Console.WriteLine($"{contact.ContactIndexNumber + 1}: {contact.ContactName}, {contact.Location.Adress}, {contact.Location.City.CityName}, {contact.PhoneNumber}, {contact.MailAdress}");
                    }
            }
            else
                Console.WriteLine("Invalid input!");
        }
        //only locations
        public void ShowLocations(ContactBook contactbook)
        {
            Console.WriteLine("\nWhat List of locations do you want to display?\n1. All locations\n2. All locations of a specific city\n3. All cities\nType 1, 2 or 3\n");
            var check = Console.ReadLine();
            Console.WriteLine("");

            if (check == "1")
            {
                Console.WriteLine("\nListing all locations:\n");
                foreach (var entry in contactbook.locationsList)
                {
                    if (entry.HasContact == false)
                        Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName}");
                    else if (entry.HasContact == true)
                        Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName} - has a contact");
                }
                Console.WriteLine("");
            }
            else if (check == "2")
            {
                Console.WriteLine("\nWhich city do you want to display the locations from?\nPossible cities are:");
                ShowCitiesOfLocations(contactbook);
                Console.WriteLine("");

                var chosenCity = Console.ReadLine();
                Console.WriteLine("");
                var cityList = new List<Location>();

                foreach (var u1 in contactbook.locationsList)
                {
                    if (chosenCity == u1.City.CityName)
                        cityList.Add(u1);
                }
                if (cityList.Count > 0)
                {
                    Console.WriteLine($"There are {cityList.Count} contacts based in {chosenCity} available:\n");
                    foreach (var entry in cityList)
                    {
                        if (entry.HasContact == false)
                            Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName}");
                        else if (entry.HasContact == true)
                            Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName} - has a contact");
                    }
                }
                else
                {
                    Console.WriteLine($"WARNING: There is no city named '{chosenCity}'");
                }
            }
            else if (check == "3")
            {
                Console.WriteLine("\nCities with locations:\n");
                ShowCitiesOfLocations(contactbook);
            }
            else
                Console.WriteLine("Invalid Input!");
        }
        //contacts and locations
        public void ShowBoth(ContactBook contactbook)
        {
            Console.WriteLine("\nWhat List do you want to display?\n1. All Contacts and Locations\n2. All contacts and locations of a specific city\n3. All cities\nType 1, 2 or 3\n");
            var check = Console.ReadLine();

            if (check == "1")
            {
                Console.WriteLine("\nListing all contacts and locations:\n");
                Console.WriteLine("Contacts:");
                foreach (var entry in contactbook.contactsList)
                {
                    Console.WriteLine($"{entry.ContactIndexNumber + 1}: {entry.ContactName}, {entry.Location.Adress}, {entry.Location.City.CityName}, {entry.PhoneNumber}, {entry.MailAdress}");
                }

                Console.WriteLine("\n\nLocations:");
                foreach (var entry in contactbook.locationsList)
                {
                    if (entry.HasContact == false)
                        Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName}");
                    else if (entry.HasContact == true)
                        Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName} - has a contact");
                }
            }
            else if (check == "2")
            {
                Console.WriteLine("\nWhich city do you want to display the contacts and locations from?");
                Console.WriteLine("\nPossible cities are:");
                ShowCitiesOfLocations(contactbook);
                Console.WriteLine("");

                var chosenCity = Console.ReadLine();
                Console.WriteLine("");
                var cityContactsList = new List<Contact>();
                var cityLocationsList = new List<Location>();

                foreach (var u1 in contactbook.contactsList)
                {
                    if (chosenCity == u1.Location.City.CityName)
                        cityContactsList.Add(u1);
                }

                foreach (var u1 in contactbook.locationsList)
                {
                    if (chosenCity == u1.City.CityName)
                        cityLocationsList.Add(u1);
                }
                if (cityLocationsList.Count > 0)
                {
                    Console.WriteLine($"There are {cityContactsList.Count} contacts based in {chosenCity} available:\n");

                    Console.WriteLine("\nContacts:");
                    foreach (var entry in cityContactsList)
                        Console.WriteLine($"{entry.ContactIndexNumber + 1}: {entry.ContactName}, {entry.Location.Adress}, {entry.PhoneNumber}, {entry.MailAdress}");

                    Console.WriteLine("\nLocations:");
                    foreach (var entry in cityLocationsList)
                    {
                        if (entry.HasContact == false)
                            Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName}");
                        else if (entry.HasContact == true)
                            Console.WriteLine($"{entry.LocationIndexNumber + 1}: {entry.Adress}, {entry.City.CityName} - has a contact");
                    }
                }
                else
                {
                    Console.WriteLine($"WARNING: There is no city named '{chosenCity}'");
                }
            }
            else if (check == "3")
            {
                Console.WriteLine("\nCities with contacts:\n");
                ShowCitiesOfContacts(contactbook);

                Console.WriteLine("\nCities with locations:\n");
                ShowCitiesOfLocations(contactbook);
            }
            else
                Console.WriteLine("Invalid Input.");
        }

        //city summary output 
        private void ShowCitiesOfContacts(ContactBook contactbook)
        {
            var uniqueCities = new List<Contact>();
            foreach (var u1 in contactbook.contactsList)
            {
                bool cityDupe = false;
                foreach (var u2 in uniqueCities)
                {
                    if (u1.Location.City.CityName == u2.Location.City.CityName)
                        cityDupe = true;
                }
                if (!cityDupe)
                    uniqueCities.Add(u1);
            }
            foreach (var entry in uniqueCities)
                Console.WriteLine($"{entry.Location.City.CityName}");
        }

        private void ShowCitiesOfLocations(ContactBook contactbook)
        {
            var uniqueCities = new List<City>();
            foreach (var u1 in contactbook.locationsList)
            {
                bool cityDupe = false;
                foreach (var u2 in uniqueCities)
                {
                    if (u1.City.CityName == u2.CityName)
                        cityDupe = true;
                }
                if (!cityDupe)
                    uniqueCities.Add(u1.City);
            }
            foreach (var entry in uniqueCities)
                Console.WriteLine($"{entry.CityName}");
        }
    }
}

