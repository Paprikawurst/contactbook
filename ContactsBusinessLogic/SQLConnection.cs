using ContactData;
using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace ContactbookLogicLibrary
{
    //TODOH: separate Console-SQL from rest + refactoring hier und zusammenfassen von methoden
    public class SQLConnection
    {
        const string CONNECTION_STRING = "Data Source=C:\\Users\\nwolff\\Desktop\\git\\contactbook\\ContactbookConsole\\contactbookDB.db;";

        public void ExecuteNonQuery(string CommandText)
        {
            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                cmd.CommandText = $"{CommandText}";
                cmd.ExecuteNonQuery(); // Non Query for DROP, INSERT or DELETE - if no result set is wanted
            }
        }

        public long ExecuteScalar(string CommandText)
        {
            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                cmd.CommandText = $"{CommandText}";
                var x = (long?)cmd.ExecuteScalar();
                if (x == null)
                {
                    x = 0;
                    return (long)x;
                }
                else
                    return (long)x;
            }
        }

        public long GetTableRowCount(string CountFrom)
        {
            string CommandText = $"SELECT COUNT(*) FROM {CountFrom}";

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                cmd.CommandText = $"SELECT COUNT(*) FROM {CountFrom}";
                long count = (long)cmd.ExecuteScalar();
                return count;
            }
        }

        public void ReadContactsTable() // output all contacts with their locations
        {
            string CommandText = "SELECT c.ContactID, c.Name, l.Address, l.CityName, c.PhoneNumber, c.MailAddress, c.Gender FROM contacts c INNER JOIN locations l ON l.LocationID = c.LocationID;";
            List<Contact> contactsList = new List<Contact>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open(); //TODOH: output whole reader data somehow in list maybe and return
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Contact contact = new Contact
                    {
                        ContactID = reader.GetInt32(0),

                    };
                    
                    //contactsList.Add(Contact)
                    Console.WriteLine($" {reader.GetInt32(0)}. {reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)} {reader.GetInt32(4)} {reader.GetString(5)} {reader.GetString(6)}");
                }
            }
        }

        public List<Location> ReadLocationsTable() // output all locations
        {
            string CommandText = "SELECT l.LocationID, l.Address, l.CityName FROM locations l";
            List<Location> locationsList = new List<Location>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CommandText = $"SELECT COUNT(*) FROM locations l INNER JOIN contacts c ON l.LocationID = c.LocationID WHERE l.LocationID = {reader.GetInt32(0)}";
                    long hasContactC = ExecuteScalar(CommandText);


                    if (hasContactC == 0) //TODO: repair output
                    {
                        Location location = new Location
                        {
                            LocationID = reader.GetInt32(0),
                            Address = reader.GetString(1),
                            CityName = reader.GetString(2),
                            HasContact = false
                        };

                        locationsList.Add(location);
                    }
                    else
                    {
                        Location location = new Location
                        {
                            LocationID = reader.GetInt32(0),
                            Address = reader.GetString(1),
                            CityName = reader.GetString(2),
                            HasContact = true
                        };

                        locationsList.Add(location);
                    }
                }
            }
            return locationsList;
        }

        public List<string> ShowCitiesOfContacts() // output all cities of contacts
        {
            string CommandText = "SELECT DISTINCT l.CityName FROM contacts c INNER JOIN locations l ON c.LocationID = l.LocationID";
            List<string> cityList = new List<string>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) //TODOH: repair output
                {
                    cityList.Add(reader.GetString(0));
                }
            }
            return cityList;
        }//TODOH: output sichtbar machen wo die methode aufgerufen wird

        public List<string> ShowCitiesOfLocations() // output all cities of locations
        {
            string CommandText = "SELECT DISTINCT l.CityName FROM locations l";
            List<string> citiesOfLocations = new List<string>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {


                connection.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) //TODOH: output sichtbar machen wo die methode aufgerufen wird
                {
                    citiesOfLocations.Add(reader.GetString(0));
                }
            }
            return citiesOfLocations;
        }

        public void ShowChosenCityOfContacts(string cityName) // output all cities of locations
        {

            string CommandText = $"SELECT c.ContactID, c.Name, l.Address, l.CityName, c.PhoneNumber, c.MailAddress, c.Gender FROM locations l INNER JOIN contacts c ON l.LocationID = c.LocationID WHERE l.CityName = '{cityName}' ;";

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) // TODOH: separate console from logic in list and return somehow
                {
                    Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)} {reader.GetInt32(4)} {reader.GetString(5)} {reader.GetString(6)}");
                }
            }
        }

        public void ShowChosenCityOfLocation(string cityName)
        {
            string CommandText = $"SELECT l.LocationID, l.Address, l.CityName FROM locations l WHERE l.CityName = '{cityName}'";

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    CommandText = $"SELECT COUNT(*) FROM locations l INNER JOIN contacts c ON l.LocationID = c.LocationID WHERE l.LocationID = {reader.GetInt32(0)}";
                    long hasContactC = ExecuteScalar(CommandText);

                    if (hasContactC == 0) //TODOH: separate console from logic - how does this work [count(*) checken]
                    {
                        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} {reader.GetString(2)}");
                    }
                    else
                        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} {reader.GetString(2)} - has contact");
                }
            }

        }
        public void ShowGenderSpecificList(string gender)
        {
            string CommandText = $"SELECT c.ContactID, c.Name, l.Address, l.CityName, c.PhoneNumber, c.MailAddress FROM contacts c INNER JOIN locations l ON l.LocationID = c.LocationID AND c.Gender = '{gender}';";
            List<Contact> genderList = new List<Contact>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) //TODOH: separate console from logic list output - somehow in list and return
                {
                    Contact contact = new Contact
                    {

                    };
                    Console.WriteLine($" {reader.GetInt32(0)}. {reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)} {reader.GetInt32(4)} {reader.GetString(5)}");
                }
            }
        }

        public long GetLocationID(Location location)
        {
            string CommandText = $"SELECT l.LocationID FROM locations l WHERE l.Address == '{location.Address}' AND l.CityName == '{location.CityName}';";

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            using (var cmd = new SQLiteCommand(CommandText, connection))
            {
                connection.Open();
                long count = (long)cmd.ExecuteScalar();
                return count;
            }
        }

        public string GetBeforeEditValueString(int inputindex, string CommandText1)
        {
            string beforeEditValue = "";
            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SQLiteCommand(CommandText1, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            beforeEditValue = reader.GetString(0);
                        }
                    }
                }
            }
            return beforeEditValue;
        }

        public int GetBeforeEditValueInt(int inputindex, string CommandText1)
        {
            int beforeEditValue = 0;
            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SQLiteCommand(CommandText1, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            beforeEditValue = reader.GetInt32(0);
                        }
                    }
                }
            }
            return beforeEditValue;
        }

        public List<Location> OutputLocationTableToList()
        {
            var locationlist = new List<Location>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                string CommandText = "SELECT l.Address, l.CityName FROM locations l";
                using (var command = new SQLiteCommand(CommandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var location = new Location
                            {
                                Address = reader.GetString(0),
                                CityName = reader.GetString(1)
                            };

                            locationlist.Add(location);
                        }
                    }
                }
            }
            return locationlist;
        }

        public List<Contact> OutputContactTableToList()
        {
            var contactslist = new List<Contact>();

            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                string CommandText = "SELECT c.Name, c.LocationID, c.PhoneNumber, c.MailAddress, c.Gender FROM contacts c";
                using (var command = new SQLiteCommand(CommandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var contact = new Contact
                            {
                                Name = reader.GetString(0),
                                LocationID = reader.GetInt32(1),
                                PhoneNumber = reader.GetInt32(2),
                                MailAddress = reader.GetString(3),
                                Gender = reader.GetString(4)
                            };

                            contactslist.Add(contact);
                        }
                    }
                }
            }
            return contactslist;
        }

        public Contact OutputSingleContact(int inputindex)
        {
            Contact contact = new Contact();
            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            {

                connection.Open();
                string CommandText = $"SELECT c.Name, c.LocationID, c.PhoneNumber, c.MailAddress, c.Gender FROM contacts c WHERE c.ContactID = {inputindex}";
                using (var command = new SQLiteCommand(CommandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        contact.Name = reader.GetString(0);
                        contact.LocationID = reader.GetInt32(1);
                        contact.PhoneNumber = reader.GetInt32(2);
                        contact.MailAddress = reader.GetString(3);
                        contact.Gender = reader.GetString(4);
                    }
                }
            }

            return contact;
        }

        public Location OutputSingleLocation(int inputindex)
        {
            Location location = new Location();
            using (var connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                string CommandText = $"SELECT Address, CityName FROM locations WHERE LocationID = {inputindex}";
                using (var command = new SQLiteCommand(CommandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        location.Address = reader.GetString(0);
                        location.CityName = reader.GetString(1);
                    }
                }
            }
            return location;
        }
    }
}