using System;
using System.Collections.Generic;
using ContactData;

namespace ContactbookLogicLibrary
{
    public class ContactBookLogic
    {

        //TODOH: remove any console output (maybe use events?)
        //----------------------------------------ADD CONTACT-----------------------------------------------------
        public void AddContact(ContactBookLogic contactbooklogic, SQLConnection sql, long countLocations, string name, Location location, long phoneNumber, string mailAddress, string gender)
        {

            int LocationID = (int)sql.GetLocationID(location);

            Contact contact = new Contact
            {
                Name = name,
                LocationID = LocationID,
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
                //TODOL: info ob contact geaddet oder dupe war und nicht geaddet
            }
        }

        //---------------------------------------ADD OR GET LOCATION-----------------------------------------------------------------------
        public Location AddOrGetLocation(ContactBookLogic contactbooklogic, SQLConnection sql, long countLocations, string address, string cityName)
        {

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

                //TODOL: info ob location geaddet oder dupe war und nicht geaddet
            }
            return location;
        }

        //--------------------------------------------------EDIT------------------------------------------------------------------------
        public void EditContact(int inputindex, string c, SQLConnection sql)
        {

            //TODOL: message successfully changed old to new for all 3 options
            var newvalue = "";
            long newphoneno = 0;
            var CommandText = "";
            bool InputIsNumber = false;
            bool CorrectInput = false;
            if (c == "1")
            {
                //GET NEW VALUE
                while (!CorrectInput)
                {
                    Console.WriteLine($"Please enter the new value for the name.");
                    newvalue = Console.ReadLine();
                    CorrectInput = InputChecker.NoEmptyInputCheck(newvalue);
                }

                //ASSIGN NEW VALUE
                CommandText = $"UPDATE contacts SET Name = '{newvalue}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
            }
            else if (c == "2")
            {
                //GET NEW VALUE
                while (!InputIsNumber)
                {
                    Console.WriteLine($"Please enter the new value for the phonenumber.");
                    InputIsNumber = long.TryParse(Console.ReadLine(), out newphoneno);
                    //CorrectInput = InputChecker.PhoneNumberCheck(newphoneno);
                }

                //ASSIGN NEW VALUE
                CommandText = $"UPDATE contacts SET phoneNumber = '{newphoneno}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);

            }
            else if (c == "3")
            {
                //GET NEW VALUE
                while (!CorrectInput)
                {
                    Console.WriteLine($"Please enter the new value for the Mailaddress.");
                    newvalue = Console.ReadLine();
                    CorrectInput = InputChecker.MailFormatCheck(newvalue);
                }

                //ASSIGN NEW VALUE
                CommandText = $"UPDATE contacts SET MailAddress = '{newvalue}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
            }


            //get contact where contactid = inputindex - edit it
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

            //TODOL: show old and new value after edit
            //GET OLD VALUE SAMPLE
            //CommandText = $"SELECT Name FROM contacts WHERE ContactID = {inputindex};";
            //string beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);
        }

        public void EditLocation(int inputindex, string c, SQLConnection sql)
        {
            var CommandText = "";
            var newValue = "";
            var beforeEditValue = "";
            bool newValueIsCorrectInput = false;

            Location location = sql.OutputSingleLocation(inputindex);
            CommandText = $"SELECT COUNT(*) FROM contacts c INNER JOIN locations l WHERE l.LocationID = c.LocationID AND l.Address = '{location.Address}' AND l.CityName = '{location.CityName}';";
            long locHasConCount = sql.ExecuteScalarC(CommandText);

            if (locHasConCount == 0)
            {
                if (c == "1")
                {
                    Console.WriteLine($"Please enter the new value for the address.");

                    CommandText = $"SELECT Address FROM locations WHERE LocationID = {inputindex};";
                    beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                    newValueIsCorrectInput = InputChecker.NoEmptyInputCheck(newValue);
                    CommandText = $"UPDATE locations SET Address = '{newValue}' WHERE LocationID = {inputindex};";
                }
                else if (c == "2")
                {
                    Console.WriteLine($"Please enter the new value for the cityname.");

                    CommandText = $"SELECT CityName FROM locations WHERE LocationID = {inputindex};";
                    beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                    newValueIsCorrectInput = InputChecker.NoEmptyInputCheck(newValue);
                    CommandText = $"UPDATE locations SET CityName = '{newValue}' WHERE LocationID = {inputindex};";
                }

                sql.ExecuteNonQuery(CommandText);
                Console.WriteLine($"\nContactname successfully changed from {beforeEditValue} to {newValue}!\n");
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
            Console.WriteLine("Contact's address and city successfully merged.\n");
        }

        //----------------------------------------REMOVE METHOD------------------------------------------------------------------------------

        public void RemoveContact(ContactBookLogic contactbooklogic, long countContact, SQLConnection sql, int value)
        {
  
                var CommandText = $"DELETE FROM contacts WHERE ContactID = '{value}';";
                sql.ExecuteNonQuery(CommandText);
                //TODOL: contact successfully deleted message
            //TODOL: invalid input message
        }

        public void RemoveLocation(ContactBookLogic contactbooklogic, long countLocation, SQLConnection sql, int value)
        {

            var CommandText = $"SELECT COUNT(l.LocationID) FROM locations l, contacts c WHERE {value} = c.LocationID AND {value} = l.LocationID;"; // check ob locationID von der zu löschenden location in contacts vorhanden ist - wenn ja -> nicht löschen
            long count = sql.ExecuteScalar(CommandText);

            CommandText = $"SELECT * FROM locations WHERE LocationID = {value}";
            long c = sql.ExecuteScalarC(CommandText);
            if (count == 0 && c > 0)
            {
                CommandText = $"DELETE FROM locations WHERE LocationID = {value};";
                sql.ExecuteNonQuery(CommandText);
                //TODOL: location successfully deleted message
            }
            //TODOL: else 1. no location with index value found 2. you can not delete location that is associated to a contact 3. invalid input
        }

        public void RemoveEverything(SQLConnection sql)
        {
            var CommandText = $"DELETE FROM contacts;";
            sql.ExecuteNonQuery(CommandText);

            CommandText = $"DELETE FROM locations;";
            sql.ExecuteNonQuery(CommandText);

            CommandText = $"DELETE FROM sqlite_sequence;";
            sql.ExecuteNonQuery(CommandText);
        }
    }
}
