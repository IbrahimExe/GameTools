using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

// Assignment 1
// NAME: Ibrahim Ishaq
// STUDENT NUMBER: 2343922

namespace Assignment_2_b
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            WeaponCollection weapons = new WeaponCollection();

            // Variables and flags

            // The path to the input file to load.
            string inputFile = string.Empty;

            // The path of the output file to save.
            string outputFile = string.Empty;

            // The flag to determine if we overwrite the output file or append to it.
            bool appendToFile = false; // We now use Save method in WeaponCollection

            // The flag to determine if we need to display the number of entries
            bool displayCount = false;

            // The flag to determine if we need to sort the results via name.
            bool sortEnabled = false;

            // The column name to be used to determine which sort comparison function to use.
            string sortColumnName = string.Empty;

            // The results to be output to a file or to the console
            // List<Weapon> results = new List<Weapon>(); // We now use WeaponCollection

            for (int i = 0; i < args.Length; i++)
            {
                // h or --help for help to output the instructions on how to use it
                if (args[i] == "-h" || args[i] == "--help")
                {
                    Console.WriteLine("-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine("-o <path> or --output <path> : saves result in the output file path specified (optional)");

                    // TODO: include help info for count
                    //"-c or --count : displays the number of entries in the input file (optional).";
                    Console.WriteLine("-c or --count : displays the number of entries");

                    // TODO: include help info for append
                    //"-a or --append : enables append mode when writing to an existing output file (optional)";
                    Console.WriteLine("-a or --append : appends to an existing output file");

                    // TODO: include help info for sort
                    //"-s or --sort <column name> : outputs the results sorted by column name";
                    Console.WriteLine("-s or --sort <column name> : sorts results by column name");

                    break;
                }
                else if (args[i] == "-i" || args[i] == "--input")
                {
                    // Check to make sure there's a second argument for the file name.
                    if (args.Length > i + 1)
                    {
                        // stores the file name in the next argument to inputFile
                        ++i;
                        inputFile = args[i];

                        if (string.IsNullOrEmpty(inputFile))
                        {
                            // TODO: print no input file specified.
                            Console.WriteLine("No input file specified!");
                            return;
                        }
                        else if (!File.Exists(inputFile))
                        {
                            // TODO: print the file specified does not exist.
                            Console.WriteLine("Input file does not exist!");
                            return;
                        }
                        else
                        {
                            // This function returns a List<Weapon> once the data is parsed.
                            // Assignment 1 Method of parsing:
                            // results = Parse(inputFile);

                            // Assignment 2 Method of parsing:
                            if (!weapons.Load(inputFile))
                            {
                                Console.WriteLine("Failed to load the input file.");
                                return;
                            }
                        }
                    }
                }
                else if (args[i] == "-s" || args[i] == "--sort")
                {
                    // TODO: set the sortEnabled flag and see if the next argument is set for the column name
                    sortEnabled = true;

                    // TODO: set the sortColumnName string used for determining if there's another sort function.
                    if (args.Length > i + 1)
                    {
                        ++i;
                        sortColumnName = args[i];
                    }
                    else
                    {
                        Console.WriteLine("No column name specified for sorting!");
                        return;
                    }
                }
                else if (args[i] == "-c" || args[i] == "--count")
                {
                    displayCount = true;
                }
                else if (args[i] == "-a" || args[i] == "--append")
                {
                    // TODO: set the appendToFile flag
                    appendToFile = true; // No longer used for assignment 2a
                }
                else if (args[i] == "-o" || args[i] == "--output")
                {
                    // validation to make sure we do have an argument after the flag
                    if (args.Length > i + 1)
                    {
                        // increment the index.
                        ++i;
                        string filePath = args[i];
                        if (string.IsNullOrEmpty(filePath))
                        {
                            // TODO: print No output file specified.
                            Console.WriteLine("No output file specified!");
                            return;
                        }
                        else
                        {
                            // TODO: set the output file to the outputFile
                            outputFile = filePath;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The argument Arg[{0}] = [{1}] is invalid", i, args[i]);
                    return;
                }
            }

            if (sortEnabled)
            {
                // TODO: add implementation to determine the column name to trigger a different sort. (Hint: column names are the 4 properties of the weapon class)
                Console.WriteLine("Sorting by {0}.", sortColumnName);

                // Assignment 1 sorting method:
                // results.Sort(Weapon.CompareByName);

                // Assignment 2a sorting method:
                weapons.SortBy(sortColumnName);
            }

            if (displayCount)
            {
                // Assignment 1:
                // Console.WriteLine("There are {0} entries", results.Count);

                // Assignment 2a:
                Console.WriteLine("There are {0} entries", weapons.Count);
            }

            // Assignment 1:
            // if (results.Count > 0)

            // Assignment 2a:
            if (weapons.Count > 0)
            {
                if (!string.IsNullOrEmpty(outputFile))
                {
                    // Assignment 1:
                    // FileStream fs;

                    // Assignment 2a: Persistence handled by WeaponCollection and IPersistence

                    // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                    if (weapons.Save(outputFile))
                    {
                        Console.WriteLine("File saved to " + outputFile);
                    }
                    else
                    {
                        Console.WriteLine("Failed to save output file.");
                    }
                }
                else
                //{
                //    fs = File.Open(outputFile, FileMode.Create);
                //}

                //// opens a stream writer with the file handle to write to the output file.
                //using (StreamWriter writer = new StreamWriter(fs))
                //{
                //    // Hint: use writer.WriteLine
                //    // TODO: write the header of the output "Name,Type,Rarity,BaseAttack"
                //    writer.WriteLine("Name,Type,Rarity,BaseAttack");

                //    // TODO: use the writer to output the results.
                //    foreach (Weapon w in results)
                //    {
                //        writer.WriteLine(w.ToString());
                //    }

                //    // TODO: print out the file has been saved.
                //    Console.WriteLine("File saved to " + outputFile);
                {

                    // prints out each entry in the weapon list results.
                    foreach (Weapon weapon in weapons)
                    {
                        //Console.WriteLine(results[i]);
                        Console.WriteLine(weapon);
                    }
                }
            }

            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Reads the file and line by line parses the data into a List of Weapons
        /// </summary>
        /// <param name="fileName">The path to the file</param>
        /// <returns>The list of Weapons</returns>
        public static List<Weapon> Parse(string fileName)
        {
            // Assignment 1:
            //using (StreamReader reader = new StreamReader(fileName))
            //{
            //    // Skip the first line because header does not need to be parsed.
            //    // Name,Type,Rarity,BaseAttack

            //    string header = reader.ReadLine();

            //    // The rest of the lines looks like the following:
            //    // Skyward Blade,Sword,5,46
            //    while (reader.Peek() > 0)
            //    {
            //        string line = reader.ReadLine();
            //        // string[] values = line.Split(',');
            //        string[] values = line.Split(',');

            //        Weapon weapon = new Weapon();
            //        // TODO: validate that the string array the size expected.
            //        if (values.Length < 4)
            //            continue;

            //        // Populate the properties of the Weapon
            //        // Populate the properties of the Weapon
            //        weapon.Name = values[0];
            //        weapon.Type = values[1];

            //        // TODO: use TryParse for stats/number values.
            //        int.TryParse(values[2], out int rarity);
            //        int.TryParse(values[3], out int baseAttack);

            //        weapon.Rarity = rarity;
            //        weapon.BaseAttack = baseAttack;

            //        // TODO: Add the Weapon to the list
            //        output.Add(weapon);
            //    }
            //}

            // return output;

            // Assignment 2:
            // Parsing has been moved into Weapon.TryParse()
            // and WeaponCollection.Load()

            return new List<Weapon>();
        }
    }
}