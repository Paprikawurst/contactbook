using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContactData;

namespace Contactbook
{
    public class CsvReader
    {
        public List<ContactData.Contact> ImportEntriesFromCsvIntoList(ContactBook contactbook, string csvFileName)
        {
            int csvLoop = 1;
            var csvFilePath = $@"C:\Users\nwolff\Desktop\Projekte\dotnet\{csvFileName}.csv";
            Console.WriteLine($"\nImporting CSV-File from: {csvFilePath}\n");

            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                string csvLine;
                sr.ReadLine();
                while ((csvLine = sr.ReadLine()) != null)
                {
                    ++csvLoop;
                    var a = csvLine.Split(';');
                    a = a.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    //CSV CONTACT
                    if (a.Length == 6)
                    {
                        ContactData.Contact contact = ReadContactFromCsvLine(csvLine);
                        bool invalidMailAdress = contact.MailAdress.Equals("");
                        bool invalidPhoneNumber = contact.PhoneNumber.Equals(0);
                        bool locIsDuplicate = InputChecker.LocationDuplicateCheck(contactbook.locationsList, contact.Location.Adress, contact.Location.City);
                        bool conIsDuplicate = InputChecker.ContactDuplicateCheck(contactbook.contactsList, contact);
                        var y = 0;
                        var x = 0;

                        if (locIsDuplicate)
                        {
                            Console.WriteLine($"\nINFO: Location on line {csvLoop} is duplicate and will not be added.\n");

                        }
                        else if (!locIsDuplicate)
                            contactbook.locationsList.Add(contact.Location);


                        foreach (var l in contactbook.locationsList)
                        {
                            if (contact.Location.Adress == l.Adress && contact.Location.City.CityName == l.City.CityName)
                            {
                                l.HasContact = true;
                            }
                        }

                        if (conIsDuplicate)
                        {
                            Console.WriteLine($"\nINFO: Contact on line {csvLoop} is duplicate and will not be added.\n");
                        }
                        else if (!conIsDuplicate && !locIsDuplicate && !invalidPhoneNumber && !invalidMailAdress)
                        {
                            contactbook.contactsList.Add(contact);
                        }
                        else if (!conIsDuplicate && locIsDuplicate && !invalidPhoneNumber && !invalidMailAdress)
                        {
                            contactbook.contactsList.Add(contact);
                            Console.WriteLine($"\nINFO: {contact.Location.Adress}, {contact.Location.City.CityName} is now assigned to {contact.ContactName}.\n");
                        }
                        else if (invalidPhoneNumber && !invalidMailAdress)
                        {
                            Console.WriteLine($"\nINFO: Contact on line {csvLoop} has an invalid phonenumber and will not be added.\n");
                        }
                        else if (!invalidPhoneNumber && invalidMailAdress)
                        {
                            Console.WriteLine($"\nINFO: Contact on line {csvLoop} has an invalid mailadress and will not be added.\n");
                        }
                        else if (invalidPhoneNumber && invalidMailAdress)
                        {
                            Console.WriteLine($"\nINFO: Contact on line {csvLoop} has an invalid phonenumber and an invalid mailadress and will not be added.\n");
                        }

                        foreach (var i in contactbook.contactsList)
                        {
                            i.ContactIndexNumber = y++;
                        }

                        foreach (var i in contactbook.locationsList)
                        {
                            i.LocationIndexNumber = x++;
                        }
                    }

                    //CSV LOCATION
                    else if (a.Length == 2)
                    {
                        Location location = ReadLocationFromCsvFile(csvLine);

                        bool IsDuplicate = InputChecker.LocationDuplicateCheck(contactbook.locationsList, location.Adress, location.City);

                        if (IsDuplicate)
                        {
                            Console.WriteLine($"\nINFO: Location on line {csvLoop} is duplicate and will not be added.\n");
                        }
                        else if (!IsDuplicate)
                        {
                            contactbook.locationsList.Add(location);
                        }

                        var y = 0;
                        foreach (var i in contactbook.locationsList)
                            i.LocationIndexNumber = y++;
                    }
                    else
                        Console.WriteLine($"WARNING: Invalid Entry on line  {csvLoop} : {csvLine}");
                }
            }
            Console.WriteLine("\nINFO: Index have been renewed.");
            return contactbook.contactsList;
        }

        public ContactData.Contact ReadContactFromCsvLine(string csvLine)
        {
            string[] parts = csvLine.Split(';');
            string namePart = parts[0];
            string adressPart = parts[1];
            string cityNamePart = parts[2];
            long.TryParse(parts[3], out long phoneNumberPart);
            string mailAdressPart = InputChecker.CsvMailFormatCheck(parts[4]);
            string genderPart = parts[5];
            int indexNumber = 0;

            Location location = ConstructLocation(indexNumber, adressPart, cityNamePart);

            if (genderPart == "male")
            {
                var man = new Man
                {
                    ContactIndexNumber = indexNumber,
                    ContactName = namePart,
                    Location = location,
                    PhoneNumber = phoneNumberPart,
                    MailAdress = mailAdressPart
                };

                return man;
            }

            else
            {
                var woman = new Woman
                {
                    ContactIndexNumber = indexNumber,
                    ContactName = namePart,
                    Location = location,
                    PhoneNumber = phoneNumberPart,
                    MailAdress = mailAdressPart
                };

                return woman;
            }
        }

        public Location ReadLocationFromCsvFile(string csvLine)
        {
            string[] parts = csvLine.Split(';');
            parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            int indexNumber = 0;
            string adressPart = parts[0];
            string cityNamePart = parts[1];

            Location location = ConstructLocation(indexNumber, adressPart, cityNamePart);
            return location;
        }

        public Location ConstructLocation(int indexNumber, string adress, string cityName)
        {
            City city = new City
            {
                CityName = cityName,
            };

            Location location = new Location
            {
                LocationIndexNumber = indexNumber,
                Adress = adress,
                City = city,
                HasContact = false,
            };
            return location;
        }
    }
}