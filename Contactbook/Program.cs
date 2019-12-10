using System;

namespace Contactbook
{
    public class Program
    {
        static void Main()
        {
            //TODO: Refactoring komplett
            //TODO: Trennung von Input und Logik für WPF

            ShowList showList = new ShowList();
            ContactBook contactbook = new ContactBook();
            CsvReader reader = new CsvReader();
            SQLConnection sql = new SQLConnection();

            while (true)
            {
                Console.WriteLine("\nType 'Add', 'Edit', 'Remove', 'List', 'Quit', 'Help', 'Clear' or 'Import'");

                long countContacts = sql.GetTableRowCount("contacts");
                long countLocations = sql.GetTableRowCount("locations");

                Console.WriteLine($"There are currently {countContacts} contacts and {countLocations} locations in the database.\n");

                string input = Console.ReadLine();

                // ADD METHOD
                if (input == "Add")
                {
                    Console.WriteLine("\nWhat do you want to add?\n1. Contact\n2. Location\n");
                    input = Console.ReadLine();

                    if (input == "1")
                        contactbook.AddContact(contactbook, sql, countLocations);
                    else if (input == "2")
                        contactbook.AddOrGetLocation(contactbook, sql, countLocations);
                    else
                        Console.WriteLine("WARNING: Invalid Input.\n");
                }
                // EDIT AND MERGE METHODS
                else if (input == "Edit")
                {
                    if (countContacts > 0 || countLocations > 0)
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
                                if (countContacts > 0)
                                    ContactBookInputControl.EditContactCommand(contactbook, sql, countContacts);
                                else
                                    Console.WriteLine("\nWARNING: There is no contact that can be edited.\n");
                            }
                            else if (a == "2")
                            {
                                if (countLocations > 0)
                                    ContactBookInputControl.EditLocationCommand(contactbook, sql, countLocations);
                                else
                                    Console.WriteLine("\nWARNING: There is no location that can be edited.\n");
                            }
                        }
                        //MERGE

                        else if (i == "2" && countContacts > 1)
                        {
                            ContactBookInputControl.MergeCommand(contactbook, sql);
                        }
                        else
                            Console.WriteLine("\nWARNING: Invalid Input or not enough contacts.");
                    }
                    else
                        Console.WriteLine("\nWARNING: You need atleast a contact or a location to edit or merge something.");
                }

                // REMOVE METHOD
                else if (input == "Remove")
                {
                    if (countContacts > 0 || countLocations > 0)
                    {
                        Console.WriteLine("\nWhat do you want to remove?\n1. Contact\n2. Location\n3. Everything\n");
                        var b = Console.ReadLine();
                        if (b == "1")
                        {
                            if (countContacts > 0)
                                contactbook.RemoveContact(contactbook, countContacts, sql);
                            else
                                Console.WriteLine("\nWARNING: There is no contact that can be removed.\n");
                        }
                        else if (b == "2")
                        {
                            if (countLocations > 0)
                                contactbook.RemoveLocation(contactbook, countLocations, sql);
                            else
                                Console.WriteLine("\nWARNING: There is no location that can be removed.\n");
                        }
                        else if (b == "3")
                        {
                            if (countContacts > 0 || countLocations > 0)
                            {
                                contactbook.RemoveEverything(sql);
                            }
                            else
                                Console.WriteLine("\nWARNING: There is nothing that can be deleted from the table.\n");
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
                    if (countContacts > 0 || countLocations > 0)
                        showList.ListWanted(contactbook, sql, countContacts, countLocations);
                    else
                        Console.WriteLine("\nWARNING: There is nothing that can be listed.\n");
                }

                // QUIT METHOD
                else if (input == "Quit")
                    break;

                else if (input == "Clear")
                    Console.Clear();

                // HELP METHOD
                else if (input == "Help")
                {
                    Console.WriteLine("\nTyping 'Add' lets you add a new contact or a location to the database.");
                    Console.WriteLine("Typing 'Edit' lets you edit any value of a contact or location or merge two existing contacts.");
                    Console.WriteLine("Typing 'Remove' lets you remove an existing contact or location from the contactbook.");
                    Console.WriteLine("Typing 'List' shows you various List options.");
                    Console.WriteLine("Typing 'Quit' closes the program.");
                    Console.WriteLine("Typing 'Help' shows this text again.");
                    Console.WriteLine("Typing 'Import' lets you import a CSV-File.");
                    Console.WriteLine("Typing 'Clear' lets you clear the console screen.");
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
                    {
                        csvFileName = "correcttest";
                        reader.ImportEntriesFromCsvIntoList(contactbook, csvFileName, sql);
                    }
                    else if (fileNameInput == "2")
                    {
                        csvFileName = "errortest";
                        reader.ImportEntriesFromCsvIntoList(contactbook, csvFileName, sql);
                    }
                    else
                        Console.WriteLine("WARNING: Wrong input.");
                }
                else
                    Console.WriteLine($"\nWARNING: {input} is not a valid input. \n");
            }
        }
    }
}