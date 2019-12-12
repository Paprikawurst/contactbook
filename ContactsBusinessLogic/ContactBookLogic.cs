using System.Collections.Generic;
using ContactData;

namespace ContactbookLogicLibrary
{
    public class ContactBookLogic
    {

        //TODO: confirmation messages etc. (maybe use events, tuples with boolean return value or premethods which check input)
        //ADD CONTACT METHOD
        public bool AddContact(ContactBookLogic contactbooklogic, SQLConnection sql, long countLocations, string name, Location location, long phoneNumber, string mailAddress, string gender)
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
            }
            return conIsDupe;
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
        public void EditContact(int inputindex, string c, SQLConnection sql, string newValue)
        {

            //TODOL: message successfully changed old to new for all 3 options on correct position
            bool newValueIsNumber = long.TryParse(newValue, out long newphoneno); //TODOL: this does not work if name is int
            var CommandText = "";
            if (c == "1")
            {
                CommandText = $"UPDATE contacts SET Name = '{newValue}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
            }
            else if (c == "2")
            {
                CommandText = $"UPDATE contacts SET phoneNumber = '{newphoneno}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText); 

            }
            else if (c == "3")
            {
                CommandText = $"UPDATE contacts SET MailAddress = '{newValue}' WHERE ContactID = {inputindex};";
                sql.ExecuteNonQuery(CommandText);
            }

            Contact contact = sql.OutputSingleContact(inputindex);
            CommandText = $"SELECT COUNT(*) FROM contacts c INNER JOIN locations l ON c.LocationID = l.LocationID WHERE c.Name = '{contact.Name}' AND c.PhoneNumber = {contact.PhoneNumber} AND c.LocationID = {contact.LocationID} AND c.MailAddress = '{contact.MailAddress}' AND c.Gender = '{contact.Gender}' ";
            long dupecount = sql.ExecuteScalar(CommandText);

            if (dupecount >= 2)
            {
                CommandText = $"DELETE FROM contacts WHERE ContactID = '{inputindex}';";
                sql.ExecuteNonQuery(CommandText);
             //TODOL: message Contact with index: {inputindex} was a duplicate after editing and got removed.
            }

            //TODOL: show old and new value after edit
            //GET OLD VALUE SAMPLE
            //CommandText = $"SELECT Name FROM contacts WHERE ContactID = {inputindex};";
            //string beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);
        }

        public void EditLocation(int inputindex, string c, SQLConnection sql, string newValue)
        {
            var CommandText = "";
            var beforeEditValue = "";
            bool newValueIsCorrectInput = false;

            Location location = sql.OutputSingleLocation(inputindex);
            CommandText = $"SELECT COUNT(*) FROM contacts c INNER JOIN locations l WHERE l.LocationID = c.LocationID AND l.Address = '{location.Address}' AND l.CityName = '{location.CityName}';";
            long locHasConCount = sql.ExecuteScalar(CommandText);

            if (locHasConCount == 0)
            {
                if (c == "1")
                {
                    CommandText = $"SELECT Address FROM locations WHERE LocationID = {inputindex};";
                    beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                    newValueIsCorrectInput = InputChecker.NoEmptyInputCheck(newValue);
                    CommandText = $"UPDATE locations SET Address = '{newValue}' WHERE LocationID = {inputindex};";
                }
                else if (c == "2")
                {
                    CommandText = $"SELECT CityName FROM locations WHERE LocationID = {inputindex};";
                    beforeEditValue = sql.GetBeforeEditValueString(inputindex, CommandText);

                    newValueIsCorrectInput = InputChecker.NoEmptyInputCheck(newValue);
                    CommandText = $"UPDATE locations SET CityName = '{newValue}' WHERE LocationID = {inputindex};";
                }

                sql.ExecuteNonQuery(CommandText);
                //TODOL: message Contactname successfully changed from {beforeEditValue} to {newValue}!
                CommandText = $"SELECT COUNT(*) FROM locations l WHERE l.Address = '{location.Address}' AND l.CityName = '{location.CityName}';";
                long dupecount = sql.ExecuteScalar(CommandText);

                if (dupecount >= 2)
                {
                    CommandText = $"DELETE FROM locations WHERE LocationID = '{inputindex}';";
                    sql.ExecuteNonQuery(CommandText);
                    //TODOL: message Location with index: {inputindex} was a duplicate after editing and got removed.
                }
            }
                //TODOL: message WARNING: You can't edit a location that is linked to an existing contact
        }

        //----------------------------------------MERGE----------------------------------------------------------------------------------

        public void MergeContacts(int temp1, int temp2, SQLConnection sql)
        {
            var CommandText = $"UPDATE contacts SET LocationID = (SELECT LocationID FROM contacts WHERE ContactID = {temp1}) WHERE ContactID = {temp2};";
            sql.ExecuteNonQuery(CommandText);
            //TODOL: message Contact's address and city successfully merged.
        }

        //----------------------------------------REMOVE METHOD------------------------------------------------------------------------------

        public void RemoveContact(ContactBookLogic contactbooklogic, long countContact, SQLConnection sql, int value)
        {
  
                var CommandText = $"DELETE FROM contacts WHERE ContactID = '{value}';";
                sql.ExecuteNonQuery(CommandText);
                //TODOL: contact successfully deleted message + invalid input message
        }

        public void RemoveLocation(ContactBookLogic contactbooklogic, long countLocation, SQLConnection sql, int value)
        {

            var CommandText = $"SELECT COUNT(l.LocationID) FROM locations l, contacts c WHERE {value} = c.LocationID AND {value} = l.LocationID;"; // check ob locationID von der zu löschenden location in contacts vorhanden ist - wenn ja -> nicht löschen
            long count = sql.ExecuteScalar(CommandText);

            CommandText = $"SELECT * FROM locations WHERE LocationID = {value}";
            long c = sql.ExecuteScalar(CommandText);
            if (count == 0 && c > 0)
            {
                CommandText = $"DELETE FROM locations WHERE LocationID = {value};";
                sql.ExecuteNonQuery(CommandText);
                //TODOL: location successfully deleted message
            }
            //TODOL: all messages for : alternatives else 1. no location with index value found 2. you can not delete location that is associated to a contact 3. invalid input
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
