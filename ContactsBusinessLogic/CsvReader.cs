using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContactData;

namespace ContactbookLogicLibrary
{
    public class CsvReader //TODO: separate whole class console output from logic
    {
        public void ImportEntriesFromCsvIntoList(ContactBookLogic contactbooklogic, string csvFileName, SQLConnection sql)
        {
            int csvLoop = 1;
            var csvFilePath = $@"C:\Users\nwolff\Desktop\git\contactbook\CsvFiles\{csvFileName}.csv";
            //TODOL: Importing CSV-File from: {csvFilePath}

            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                string csvLine;
                sr.ReadLine();
                while ((csvLine = sr.ReadLine()) != null)
                {
                    ++csvLoop;
                    var a = csvLine.Split(','); // turns csvLine into SplitStringArray
                    a = a.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    //CSV CONTACT
                    if (a.Length == 6)
                    {
                        ReadAndAddContactFromCsvLine(csvLine, sql);
                    }

                    //CSV LOCATION
                    else if (a.Length == 2)
                    {
                        ReadAndAddLocationFromCsvFile(csvLine, sql);
                    }

                    //   TODOL: else message Invalid Entry on line  {csvLoop} : {csvLine}");
                }
            }
        }

        public void ReadAndAddContactFromCsvLine(string csvLine, SQLConnection sql)
        {
            string[] parts = csvLine.Split(',');
            string namePart = InputChecker.CsvEmptyInputCheck(parts[0]);
            string addressPart = InputChecker.CsvEmptyInputCheck(parts[1]);
            string cityNamePart = InputChecker.CsvEmptyInputCheck(parts[2]);
            long.TryParse(parts[3], out long a);
            long phoneNumberPart = a;
            string mailAddressPart = InputChecker.CsvMailFormatCheck(parts[4]);
            string genderPart = InputChecker.CsvGenderCheck(parts[5]);

            if (phoneNumberPart != 0 && genderPart != "wronginput" && namePart != "wronginput" && addressPart != "wronginput" && cityNamePart != "wronginput")
            {
                Location location = new Location()
                {
                    Address = addressPart,
                    CityName = cityNamePart,
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
                }
                //    TODOL: Message INFO: Location is duplicate and will not be added

                long LocationID = sql.GetLocationID(location);


                var contact = new Contact
                {
                    Name = namePart,
                    LocationID = (int)LocationID,
                    PhoneNumber = phoneNumberPart,
                    MailAddress = mailAddressPart,
                    Gender = genderPart
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
                    CommandText = $"INSERT INTO contacts(Name, LocationID, PhoneNumber, MailAddress, Gender) VALUES('{contact.Name}', '{contact.LocationID}', '{contact.PhoneNumber}', '{contact.MailAddress}', '{contact.Gender}');";
                    sql.ExecuteNonQuery(CommandText);
                }
                // TODOL: Message INFO: Contact on {csvLine} is duplicate and will not be added
            }
            //   TODOL: message WARNING: Wrong input on line: {csvLine}

        }

        public void ReadAndAddLocationFromCsvFile(string csvLine, SQLConnection sql)
        {
            string[] parts = csvLine.Split(',');
            parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string addressPart = InputChecker.CsvEmptyInputCheck(parts[0]);
            string cityNamePart = InputChecker.CsvEmptyInputCheck(parts[1]);

            if (addressPart != "wronginput" && cityNamePart != "wronginput")
            {
                Location location = new Location()
                {
                    Address = addressPart,
                    CityName = cityNamePart,
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
                }
                    //TODOL: message INFO: Location is duplicate and will not be added
            }
                //TODOL: message WARNING: Wrong input on line: {csvLine}
        }
    }
}
