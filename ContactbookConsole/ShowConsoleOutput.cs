using ContactbookLogicLibrary;
using System;

namespace ContactbookConsole
{
    public class ShowConsoleOutput
    {
        //TODO: hier liste bekommen und ausgabe hier - damit es von SQL connection getrennt ist
        public void ListWanted(ContactBookLogic contactbooklogic, SQLConnection sql, long countContacts, long countLocations)
        {
            Console.WriteLine("\nDo you want a list of 1. only contacts, 2. only locations or 3. both?\nType 1, 2 or 3\n");
            var v = Console.ReadLine();

            if (v == "1")
            {
                if (countContacts > 0)
                    ShowContacts(contactbooklogic, sql);

                else
                    Console.WriteLine("\nWARNING: There are no contacts which could be displayed.\n");
            }
            else if (v == "2")
            {
                if (countLocations > 0)
                    ShowLocations(contactbooklogic, sql);

                else
                    Console.WriteLine("\nWARNING: There are no locations which could be displayed.\n");
            }
            else if (v == "3")
            {
                if (countContacts > 0 || countLocations > 0)
                    ShowBoth(contactbooklogic, sql);

                else
                    Console.WriteLine("\nWARNING: There are neither contacts nor locations which could be displayed.");
            }
            else
                Console.WriteLine("Invalid Input.");
        }

        //only contacts
        public void ShowContacts(ContactBookLogic contactbooklogic, SQLConnection sql)
        {
            Console.WriteLine("\nWhat List of contacts do you want to display?\n1. All contacts\n2. All contacts of a specific city\n3. All cities\n4. All male contacts\n5. All female contacts\nType 1, 2 or 3\n");
            var check = Console.ReadLine();

            if (check == "1")
            {
                Console.WriteLine("\nListing all contacts:\n");
                sql.ReadContactsTable();
                Console.WriteLine("");
            }
            else if (check == "2")
            {
                Console.WriteLine("");
                Console.WriteLine("\nWhich city do you want to display the contacts from?\nPossible cities are:\n");
                sql.ShowCitiesOfContacts();
                Console.WriteLine("");
                var chosenCity = Console.ReadLine();
                var CommandText = $"SELECT * FROM locations l INNER JOIN contacts c ON c.LocationID = l.LocationID WHERE l.CityName = '{chosenCity}';";
                long count = sql.ExecuteScalarC(CommandText);

                if (count > 0)
                {
                    Console.WriteLine($"\nListing all contacts from city: {chosenCity}\n");
                    sql.ShowChosenCityOfContacts(chosenCity);
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($"\nWARNING: {chosenCity} does not exist in the database\n");
                }
            }
            else if (check == "3")
            {
                Console.WriteLine("\nCities with contacts:\n");
                sql.ShowCitiesOfContacts();
            }
            else if (check == "4")
            {
                Console.WriteLine("\nMale contacts:\n");
                var gender = "Male";
                sql.ShowGenderSpecificList(gender);
            }
            else if (check == "5")
            {
                Console.WriteLine("\nFemale contacts:\n");
                var gender = "Female";
                sql.ShowGenderSpecificList(gender);
            }
            else
                Console.WriteLine("\nInvalid input!\n");
        }
        //only locations
        public void ShowLocations(ContactBookLogic contactbooklogic, SQLConnection sql)
        {
            Console.WriteLine("\nWhat List of locations do you want to display?\n1. All locations\n2. All locations of a specific city\n3. All cities\nType 1, 2 or 3\n");
            var check = Console.ReadLine();
            Console.WriteLine("");

            if (check == "1")
            {
                Console.WriteLine("\nListing all locations:\n");
                sql.ReadLocationsTable();
                Console.WriteLine("");
            }
            else if (check == "2")
            {
                Console.WriteLine("\nWhich city do you want to display the locations from?\nPossible cities are:\n");
                sql.ShowCitiesOfLocations();
                Console.WriteLine("");
                var chosenCity = Console.ReadLine();

                var CommandText = $"SELECT * FROM Locations l WHERE l.CityName = '{chosenCity}';";
                long count = sql.ExecuteScalarC(CommandText);

                if (count > 0)
                {
                    Console.WriteLine($"\nListing all locations from city: {chosenCity}\n");
                    sql.ShowChosenCityOfLocation(chosenCity);
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($"\nWARNING: {chosenCity} does not exist in the database\n");
                }
            }
            else if (check == "3")
            {
                Console.WriteLine("\nCities with locations:\n");
                sql.ShowCitiesOfLocations();
            }
            else
                Console.WriteLine("Invalid Input!");
        }
        //contacts and locations
        public void ShowBoth(ContactBookLogic contactbooklogic, SQLConnection sql)
        {
            Console.WriteLine("\nWhat List do you want to display?\n1. All Contacts and Locations\n2. All contacts and locations of a specific city\n3. All cities\nType 1, 2 or 3\n");
            var check = Console.ReadLine();

            if (check == "1")
            {
                Console.WriteLine("\nListing all contacts and locations...\n");

                Console.WriteLine("Contacts:");
                sql.ReadContactsTable();
                Console.WriteLine("");

                Console.WriteLine("\nLocations:");   
                sql.ReadLocationsTable();
                Console.WriteLine("");


            }
            else if (check == "2")
            {
                Console.WriteLine("\nWhich city do you want to display the contacts and locations from?\nPossible cities are:\n");
                sql.ShowCitiesOfLocations();
                Console.WriteLine("");

                var chosenCity = Console.ReadLine();

                var CommandText = $"SELECT * FROM Locations l WHERE l.CityName = '{chosenCity}';";
                long count = sql.ExecuteScalarC(CommandText);

                if (count > 0)
                {
                    Console.WriteLine($"Showing Contacts and Locations of the city: {chosenCity}");

                    Console.WriteLine("\nContacts:");
                    sql.ShowChosenCityOfContacts(chosenCity);

                    Console.WriteLine("\nLocations:");
                    sql.ShowChosenCityOfLocation(chosenCity);
                }
                else
                {
                    Console.WriteLine($"\nWARNING: {chosenCity} does not exist in the database\n");
                }
            }
            else if (check == "3")
            {
                Console.WriteLine("\nCities with contacts:\n");
                sql.ShowCitiesOfContacts();

                Console.WriteLine("\nCities with locations:\n");
                sql.ShowCitiesOfLocations();
            }
            else
                Console.WriteLine("Invalid Input.");
        }
    }
}

