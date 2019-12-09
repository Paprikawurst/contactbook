using System;
using System.Collections.Generic;
using ContactData;

namespace Contactbook
{
    public class ContactBook
    {
        //----------------------------------------ADD CONTACT-----------------------------------------------------
        public void AddContact(ContactBook contactbook, SQLConnection sql, long countLocations)
        {
            Console.WriteLine("\nPlease enter the Name of the new Contact.");
            var name = InputChecker.NoEmptyInputCheck();

            Location location = AddOrGetLocation(contactbook, sql, countLocations);

            long LocationID = sql.GetLocationID(location);

            Console.WriteLine("\nPlease enter the phone number for the new Contact");
            long phoneNumber = InputChecker.PhoneNumberCheck();

            Console.WriteLine("\nPlease enter a mail adress for the new Contact");
            var mailAddress = InputChecker.MailFormatCheck();

            Console.WriteLine("\nPlease enter the gender for the new Contact ('Male' or 'Female')");
            var gender = InputChecker.GenderCheck();

            Contact contact = new Contact
            {
                Name = name,
                LocationID = (int)LocationID,
                PhoneNumber = phoneNumber,
                MailAddress = mailAddress,
                Gender = gender
            };
            List<Contact> contactslist = sql.OutputContactTableToList();
            var conIsDupe = false;

            foreach (var con1 in contactslist)
            {
                if ((contact.Name == con1.Name) && (contact.LocationID == con1.LocationID) && (contact.PhoneNumber == con1.PhoneNumber) && (contact.MailAddress == con1.MailAddress) && (contact.Gender == con1.Gender))
                    conIsDupe = true;
            }

            if (!conIsDupe)
            {
                var CommandText = $"INSERT INTO contacts(Name, LocationID, PhoneNumber, MailAddress, Gender) VALUES('{contact.Name}', '{LocationID}', '{contact.PhoneNumber}', '{contact.MailAddress}', '{contact.Gender}');";
                sql.ExecuteNonQuery(CommandText);

                Console.WriteLine("\nINFO: Contact successfully added!\n");
            }
            else
                Console.WriteLine("\nINFO: Contact is duplicate and will not be added!\n");
        }

        //---------------------------------------ADD OR GET LOCATION-----------------------------------------------------------------------
        public Location AddOrGetLocation(ContactBook contactbook, SQLConnection sql, long countLocations)
        {
            Console.WriteLine("\nPlease enter the address of the new contact.");
            var address = InputChecker.NoEmptyInputCheck();

            Console.WriteLine("\nPlease enter the cityname of the new contact");
            var cityName = InputChecker.NoEmptyInputCheck();

            Location location = new Location()
            {
                Address = address,
                CityName = cityName,
            };

            List<Location> locationslist = sql.OutputLocationTableToList();
            var locIsDupe = false;
            var CommandText = "";

            foreach (var loc1 in locationslist)
            {
                if ((location.Address == loc1.Address) && (location.CityName == loc1.CityName))
                    locIsDupe = true;
            }

            if (!locIsDupe)
            {
                CommandText = $"INSERT INTO locations(Address, CityName) VALUES ('{location.Address}', '{location.CityName}')";
                sql.ExecuteNonQuery(CommandText);

                Console.WriteLine("\nINFO: Location successfully added!\n");
            }
            else
            {
                Console.WriteLine("\nINFO: Location is duplicate and will not be added!\n");
            }

            return location;

        }

        //--------------------------------------------------EDIT------------------------------------------------------------------------
        public void EditContact(int inputindex, string c, SQLConnection sql)
        {
            var newvalue = "";
            var CommandText = "";
            if (c == "1")
            {
                Console.WriteLine($"Please enter the new value for the name.");

                CommandText = $"SELECT Name FROM contacts WHERE ContactID = {inputindex};";
                string beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                newvalue = InputChecker.NoEmptyInputCheck();
                CommandText = $"UPDATE contacts SET Name = '{newvalue}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine($"\nContactname successfully changed from {beforeEditValue} to {newvalue}!\n");
            }
            else if (c == "2")
            {
                Console.WriteLine($"Please enter the new value for the phonenumber.");

                CommandText = $"SELECT PhoneNumber FROM contacts WHERE ContactID = {inputindex};";
                int beforeEditValue = sql.GetBeforeEditValueInt(inputindex, CommandText);

                var newphoneno = InputChecker.PhoneNumberCheck();
                CommandText = $"UPDATE contacts SET phoneNumber = '{newphoneno}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine($"\nPhonenumber successfully changed from {beforeEditValue} to {newphoneno}!\n");
            }
            else if (c == "3")
            {
                Console.WriteLine($"Please enter the new value for the Mailaddress.");

                CommandText = $"SELECT MailAddress FROM contacts WHERE ContactID = {inputindex};";
                string beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);


                newvalue = InputChecker.MailFormatCheck();
                CommandText = $"UPDATE contacts SET MailAddress = '{newvalue}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine($"\nContactname successfully changed from {beforeEditValue} to {newvalue}!\n");
            }
            //get contact as string array where contactid = inputindex - edit it
            Contact contact = sql.OutputSingleContact(inputindex);
            //search through database and check if there is one with the same values - if yes delete it
            CommandText = $"SELECT COUNT(*) FROM contacts c INNER JOIN locations l ON c.LocationID = l.LocationID WHERE c.Name = '{contact.Name}' AND c.PhoneNumber = {contact.PhoneNumber} AND c.LocationID = {contact.LocationID} AND c.MailAddress = '{contact.MailAddress}' AND c.Gender = '{contact.Gender}' ";
            long dupecount = sql.ExecuteScalarC(CommandText);

            if (dupecount >= 2) // there should always be a maximum of 2 
            {
                CommandText = $"DELETE FROM contacts WHERE ContactID = '{inputindex}';";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine($"\nWARNING: Contact with index: {inputindex} was a duplicate after editing and got removed.\n");
            }
        }

        public void EditLocation(int inputindex, string c, SQLConnection sql)
        {
            var CommandText = "";
            var newvalue = "";
            var beforeEditValue = "";

            Location location = sql.OutputSingleLocation(inputindex);
            CommandText = $"SELECT COUNT(*) FROM contacts c INNER JOIN locations l WHERE l.LocationID = c.LocationID AND l.Address = '{location.Address}' AND l.CityName = '{location.CityName}';";
            long locHasConCount = sql.ExecuteScalarC(CommandText);

            if (locHasConCount == 0)
            {
                if (c == "1")
                {
                    Console.WriteLine($"Please enter the new value for the adress.");

                    CommandText = $"SELECT Address FROM locations WHERE LocationID = {inputindex};";
                    beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                    newvalue = InputChecker.NoEmptyInputCheck();
                    CommandText = $"UPDATE locations SET Address = '{newvalue}' WHERE LocationID = {inputindex};";
                }
                else if (c == "2")
                {
                    Console.WriteLine($"Please enter the new value for the cityname.");

                    CommandText = $"SELECT CityName FROM locations WHERE LocationID = {inputindex};";
                    beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                    newvalue = InputChecker.NoEmptyInputCheck();
                    CommandText = $"UPDATE locations SET CityName = '{newvalue}' WHERE LocationID = {inputindex};";
                }

                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine($"\nContactname successfully changed from {beforeEditValue} to {newvalue}!\n");
                CommandText = $"SELECT COUNT(*) FROM locations l WHERE l.Address = '{location.Address}' AND l.CityName = '{location.CityName}';";
                long dupecount = sql.ExecuteScalarC(CommandText);

                if (dupecount >= 2) // there should never be more than 2 at any time
                {
                    CommandText = $"DELETE FROM locations WHERE LocationID = '{inputindex}';";
                    sql.ExecuteNonQuery(CommandText);
                    Console.WriteLine($"\nWARNING: Location with index: {inputindex} was a duplicate after editing and got removed.\n");
                }
            }
            else
                Console.WriteLine("WARNING: You can't edit a location that is linked to an existing contact");
        }

        //----------------------------------------MERGE----------------------------------------------------------------------------------

        public void MergeContacts(int temp1, int temp2, SQLConnection sql)
        {
            var CommandText = $"UPDATE contacts SET LocationID = (SELECT LocationID FROM contacts WHERE ContactID = {temp1}) WHERE ContactID = {temp2};";
            sql.ExecuteNonQuery(CommandText);
            Console.WriteLine("Contact's adress and city successfully merged.\n");
        }

        //----------------------------------------REMOVE METHOD------------------------------------------------------------------------------

        public void RemoveContact(ContactBook contactbook, long countContact, SQLConnection sql)
        {
            Console.WriteLine("Please enter the index of the contact you want to remove.");
            sql.ReadContactsTable();
            Console.WriteLine("");

            bool numberCheck = int.TryParse(Console.ReadLine(), out var value);
            if (numberCheck)
            {
                var CommandText = $"DELETE FROM contacts WHERE ContactID = '{value}';";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine("\nINFO: Contact successfully deleted!\n");
            }
            else
                Console.WriteLine($"\nWARNING: Invalid Input\n");
        }

        public void RemoveLocation(ContactBook contactbook, long countLocation, SQLConnection sql)
        {
            Console.WriteLine("\nPlease enter the index of the location you want to remove.\n");
            sql.ReadLocationsTable();
            Console.WriteLine("");

            bool numberCheck = int.TryParse(Console.ReadLine(), out var value);
            var CommandText = $"SELECT COUNT(l.LocationID) FROM locations l, contacts c WHERE {value} = c.LocationID AND {value} = l.LocationID;"; // check ob locationID von der zu löschenden location in contacts vorhanden ist - wenn ja -> nicht löschen
            long count = sql.ExecuteScalar(CommandText);

            CommandText = $"SELECT * FROM locations WHERE LocationID = {value}";
            long c = sql.ExecuteScalarC(CommandText);
            if (numberCheck && count == 0 && c > 0)
            {
                CommandText = $"DELETE FROM locations WHERE LocationID = {value};";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine("\nINFO: Location successfully deleted!\n");
            }
            else if (numberCheck && count == 0 && c == 0)
            {
                Console.WriteLine($"\nWARNING: No location with index: {value} found\n");
            }
            else if (count > 0)
                Console.WriteLine($"\nWARNING: You can not delete a location that is associated to a contact\n");
            else
                Console.WriteLine("\nWARNING: Invalid Input\n");
        }

        public void RemoveEverything(SQLConnection sql)
        {
            Console.WriteLine("\nIf you really want to empty the entire database enter 'y' now.\n");
            var input = Console.ReadLine();
            if (input == "y")
            {
                var CommandText = $"DELETE FROM contacts;";
                sql.ExecuteNonQuery(CommandText);

                CommandText = $"DELETE FROM locations;";
                sql.ExecuteNonQuery(CommandText);

                CommandText = $"DELETE FROM sqlite_sequence;";
                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine("\nINFO: Database successfully emptied.\n");
            }
        }
    }
}
