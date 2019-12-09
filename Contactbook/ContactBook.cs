using System;
using System.Collections.Generic;
using ContactData;

namespace Contactbook
{
    public class ContactBook
    {
        public List<ContactData.Location> locationsList = new List<ContactData.Location>();
        public List<ContactData.Contact> contactsList = new List<ContactData.Contact>();

        //----------------------------------------ADD CONTACT-----------------------------------------------------
        public void AddContact(int contactIndexNumber, string name, ContactData.Location location, long phoneNumber, string mailAdress, string gender)
        {

            foreach (var l in locationsList)
                if (location.Adress == l.Adress && location.City.CityName == l.City.CityName)
                    l.HasContact = true;

            switch (gender)
            {
                case "male":
                    var man = new ContactData.Man          // man oder contact?
                    {
                        ContactIndexNumber = contactIndexNumber,
                        ContactName = name,
                        Location = location,
                        PhoneNumber = phoneNumber,
                        MailAdress = mailAdress
                    };

                    bool ConIsDuplicate = InputChecker.ContactDuplicateCheck(contactsList, man);
                    if (ConIsDuplicate)
                    {
                        Console.WriteLine("\nINFO: Contact is duplicate and will not be added.\n");
                    }
                    else if (!ConIsDuplicate)
                    {
                        contactsList.Add(man);
                        Console.WriteLine($"\nINFO: {man.ContactName} successfully added to the contactbook.\n");
                    }
                    break;

                case "female":
                    var woman = new ContactData.Woman           // woman oder contact?
                    {
                        ContactIndexNumber = contactIndexNumber,
                        ContactName = name,
                        Location = location,
                        PhoneNumber = phoneNumber,
                        MailAdress = mailAdress
                    };

                    ConIsDuplicate = InputChecker.ContactDuplicateCheck(contactsList, woman);
                    if (ConIsDuplicate)
                    {
                        Console.WriteLine("\nINFO: Contact is duplicate and will not be added.\n");
                    }
                    else if (!ConIsDuplicate)
                    {
                        contactsList.Add(woman);
                        Console.WriteLine($"\nINFO: {woman.ContactName} successfully added to the contactbook.\n");
                    }
                    break;

                default:
                    break;

            }
        }


        //------------------------------------GET OR ADD LOCATION-----------------------------------------------------------------------
        public Location GetOrAddLocation(int locationIndexNumber, string adress, string cityName)
        {
            Location location;

            City city = new City()
            {
                CityName = cityName
            };

            foreach (var l in locationsList)
            {
                if (adress == l.Adress && city.CityName == l.City.CityName)
                {
                    Console.WriteLine("\nINFO: Location is duplicate and will not be added.\n");
                    return l;
                }
            }

            location = new Location()
            {
                LocationIndexNumber = locationIndexNumber,
                Adress = adress,
                City = city,
                HasContact = false
            };

            locationsList.Add(location);
            return location;
        }

        //--------------------------------------------------EDIT------------------------------------------------------------------------
        public void EditContact(int inputindex, string c)
        {
            var newvalue = "";

            if (c == "1")
            {
                Console.WriteLine($"Please enter the new value for the name.");
                newvalue = Console.ReadLine();
                var before = contactsList[inputindex].ContactName;
                contactsList[inputindex].ContactName = newvalue;
                Console.WriteLine($"\nINFO: {before} changed to {newvalue}.\n");

            }
            else if (c == "2")
            {
                Console.WriteLine($"Please enter the new value for the adress.");
                newvalue = Console.ReadLine();
                var before = contactsList[inputindex].Location.Adress;
                contactsList[inputindex].Location.Adress = newvalue;
                Console.WriteLine($"\nINFO: {before} changed to {newvalue}.\n");
            }
            else if (c == "3")
            {
                Console.WriteLine($"Please enter the new value for the cityname.");
                newvalue = Console.ReadLine();
                var before = contactsList[inputindex].Location.City.CityName;
                contactsList[inputindex].Location.City.CityName = newvalue;
                Console.WriteLine($"\nINFO: {before} changed to {newvalue}.\n");

            }
            else if (c == "4")
            {
                Console.WriteLine($"Please enter the new value for the phonenumber.");
                var before = contactsList[inputindex].PhoneNumber;
                contactsList[inputindex].PhoneNumber = InputChecker.PhoneNumberCheck();
                Console.WriteLine($"\nINFO: {before} changed to {contactsList[inputindex].PhoneNumber}.\n");

            }
            else if (c == "5")
            {
                Console.WriteLine($"Please enter the new value for the mailadress.");
                var before = contactsList[inputindex].MailAdress;
                contactsList[inputindex].MailAdress = InputChecker.MailFormatCheck();
                Console.WriteLine($"\nINFO: {before} changed to {contactsList[inputindex].MailAdress}.\n");
            }

            var duplicateCount = 0;

            foreach (var a in contactsList)
            {
                if (a.ContactName == contactsList[inputindex].ContactName && a.Location.Adress == contactsList[inputindex].Location.Adress && a.Location.City.CityName == contactsList[inputindex].Location.City.CityName && a.MailAdress == contactsList[inputindex].MailAdress && a.PhoneNumber == contactsList[inputindex].PhoneNumber)
                {
                    duplicateCount++;
                    if (duplicateCount == 2)
                    {
                        contactsList.RemoveAt(inputindex);
                        Console.WriteLine("Contact matched another contact and was deleted from the list.");
                        break;
                    }
                }
            }
        }

        public void EditLocation(int inputindex, string c)
        {
            var newvalue = "";

            if (c == "1")
            {
                Console.WriteLine($"Please enter the new value for the adress.");
                newvalue = Console.ReadLine();
                var before = locationsList[inputindex].Adress;
                locationsList[inputindex].Adress = newvalue;
                Console.WriteLine($"\nINFO: {before} changed to {newvalue}.\n");

            }
            else if (c == "2")
            {
                Console.WriteLine($"Please enter the new value for the cityname.");
                newvalue = Console.ReadLine();
                var before = locationsList[inputindex].City.CityName;
                locationsList[inputindex].City.CityName = newvalue;
                Console.WriteLine($"\nINFO: {before} changed to {newvalue}.\n");
            }
            var duplicateCount = 0;
            foreach (var a in locationsList)
            {
                if (a.Adress == locationsList[inputindex].Adress && a.City.CityName == locationsList[inputindex].City.CityName)
                {
                    duplicateCount++;
                    if (duplicateCount == 2)
                    {
                        locationsList.RemoveAt(inputindex);
                        Console.WriteLine("Location matched another location and was deleted from the list.");
                        break;
                    }
                }
                // TODO: wenn hier locdupe auftritt nach dem edit wird die loc gelöscht 
                // und etwaige kontakte stehen ohne loc da + werden nichtmehr angezeigt
            }
        }

        //--------------------------------MERGE METHOD----------------------------------------------------------------------------------
        public void MergeContacts(int temp1, int temp2)
        {
            contactsList[temp1].Location = contactsList[temp2].Location;
            Console.WriteLine("Contact's adress and city successfully merged.\n");
        }

        //------------------------------------REMOVE METHOD------------------------------------------------------------------------------

        public void RemoveContact(int value)
        {
            Console.WriteLine($"\nINFO: Contact with index {value + 1} ({contactsList[value].ContactName}) has been removed.");
            contactsList[value].Location.HasContact = false;
            contactsList.RemoveAt(value);
            var indexNumber = 0;
            foreach (var i in contactsList)
            {
                i.ContactIndexNumber = ++indexNumber;
            }

            Console.WriteLine("\nINFO: Index have been renewed.");
        }

        public void RemoveLocation(int value)
        {
            if (!locationsList[value].HasContact)
            {
                Console.WriteLine($"\nINFO: Location with index {value + 1} ({locationsList[value].Adress}, {locationsList[value].City.CityName}) has been removed.");
                locationsList.RemoveAt(value);
                var indexNumber = 0;
                foreach (var i in locationsList)
                {
                    i.LocationIndexNumber = indexNumber++;
                }
                Console.WriteLine("\nINFO: Index have been renewed.");
            }
            else
                Console.WriteLine($"\nWARNING: Location with index {value + 1} can not be removed because it is assigned to an existing contact!");
        }
    }
}
