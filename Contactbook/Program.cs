using System;

namespace Contactbook
{
    public class Program
    {
        static void Main()
        {
            ShowList showList = new ShowList();
            ContactBook contactbook = new ContactBook();
            CsvReader reader = new CsvReader();



            while (true)
            {
                Console.WriteLine("\nType 'Add', 'Edit', 'Remove', 'List', 'Quit', 'Help' or 'Import'");
                Console.WriteLine($"\nThere are currently {contactbook.contactsList.Count} contacts and {contactbook.locationsList.Count} locations in the contactbook.\n");
                string input = Console.ReadLine();

                // ADD METHOD
                if (input == "Add")
                {
                    Console.WriteLine("\nWhat do you want to add?\n1. Contact\n2. Location\n");
                    input = Console.ReadLine();

                    if (input == "1")
                        ContactBookInputControl.ContactCommand(contactbook);
                    else if (input == "2")
                        ContactBookInputControl.LocationCommand(contactbook);
                    else
                        Console.WriteLine("WARNING: Invalid Input.\n");
                }
                // EDIT AND MERGE METHODS
                else if (input == "Edit")
                {
                    if (contactbook.contactsList.Count > 0 || contactbook.locationsList.Count > 0)
                    {
                        Console.WriteLine("\nWhat do you want to do?\n1. Edit a contact or location\n2. Merge a contact\n");
                        var i = Console.ReadLine();

                        //EDIT
                        if (i == "1")
                        {
                            Console.WriteLine("\nWhat do you want to edit?\n1. Contact\n2. Location\n");
                            var a = Console.ReadLine();
                            if (a == "1")
                            {
                                if (contactbook.contactsList.Count > 0)
                                    ContactBookInputControl.EditContactCommand(contactbook);
                                else
                                    Console.WriteLine("\nWARNING: There is no contact that can be edited.\n");
                            }
                            else if (a == "2")
                            {
                                if (contactbook.locationsList.Count > 0)
                                    ContactBookInputControl.EditLocationCommand(contactbook);
                                else
                                    Console.WriteLine("\nWARNING: There is no location that can be edited.\n");
                            }
                        }
                        //MERGE
                        else if (i == "2" && contactbook.contactsList.Count > 1)
                        {
                            ContactBookInputControl.MergeCommand(contactbook);
                        }
                        else
                            Console.WriteLine("WARNING: Invalid Input or not enough contacts.");
                    }
                    else
                        Console.WriteLine("WARNING: You need atleast a contact or a location to edit or merge something.");
                }

                // REMOVE METHOD
                else if (input == "Remove")
                {
                    if (contactbook.contactsList.Count > 0 || contactbook.locationsList.Count > 0)
                    {
                        Console.WriteLine("\nWhat do you want to remove?\n1. Contact\n2. Location\n");
                        var b = Console.ReadLine();
                        if (b == "1")
                        {
                            if (contactbook.contactsList.Count > 0)
                                ContactBookInputControl.RemoveContactCommand(contactbook);
                            else
                                Console.WriteLine("\nWARNING: There is no contact that can be removed.\n");
                        }
                        else if (b == "2")
                        {
                            if (contactbook.locationsList.Count > 0)
                                ContactBookInputControl.RemoveLocationCommand(contactbook);
                            else
                                Console.WriteLine("\nWARNING: There is no location that can be removed.\n");
                        }
                        else
                            Console.WriteLine("WARNING: Invalid Input.\n");
                    }
                    else
                        Console.WriteLine("\nWARNING: There is nothing that can be removed.\n");
                }
                // LIST METHOD
                else if (input == "List")
                {
                    if (contactbook.contactsList.Count > 0 || contactbook.locationsList.Count > 0)
                        showList.ListWanted(contactbook);
                    else
                        Console.WriteLine("\nWARNING: There is nothing that can be listed.\n");
                }

                // QUIT METHOD
                else if (input == "Quit")
                    break;

                // HELP METHOD
                else if (input == "Help")
                {
                    Console.WriteLine("\nTyping 'Add' lets you add a new contact or a location to the contactbook.");
                    Console.WriteLine("Typing 'Edit' lets you edit any value of a contact or location or merge two existing contacts.");
                    Console.WriteLine("Typing 'Remove' lets you remove an existing contact or location from the contactbook.");
                    Console.WriteLine("Typing 'List' shows you various List options.");
                    Console.WriteLine("Typing 'Quit' closes the program.");
                    Console.WriteLine("Typing 'Help' shows this text again.");
                    Console.WriteLine("Typing 'Import' lets you import a CSV-File.");
                    Console.WriteLine("'WARNING' indicates wrong input - 'INFO' indicates changed values.\n");
                }

                //IMPORT METHOD
                else if (input == "Import")
                {
                    Console.WriteLine("Do you want to import 1. correcttest.csv or 2. errortest.csv?\nType 1 or 2\n");
                    string csvFileName = "";
                    string fileNameInput = Console.ReadLine();
                    Console.WriteLine("");
                    if (fileNameInput == "1")
                        csvFileName = "correcttest";
                    else if (fileNameInput == "2")
                        csvFileName = "errortest";
                    else
                        Console.WriteLine("WARNING: Wrong input.");

                    reader.ImportEntriesFromCsvIntoList(contactbook, csvFileName);
                }
                else
                    Console.WriteLine($"\nWARNING: {input} is not a valid input. \n");
            }
        }
    }
}
