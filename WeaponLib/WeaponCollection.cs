using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WeaponLib
{
    // Make public so XmlSerializer can work with it better
    [XmlRoot("WeaponCollection")]
    public class WeaponCollection : List<Weapon>, ICsvSerializable, IJsonSerializable, IXmlSerializable
    {
        // CSV:
        public bool LoadCSV(string path)
        {
            Clear();

            if (!File.Exists(path))
                return false;

            try
            {
                var lines = File.ReadAllLines(path);
                if (lines.Length <= 1)
                    return true;

                // Expect header at line 0; start from 1
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (Weapon.TryParse(line, out Weapon w))
                    {
                        this.Add(w);
                    }
                    else
                    {
                        Clear();
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                Clear();
                return false;
            }
        }

        public bool SaveAsCSV(string path)
        {
            try
            {
                using (var writer = new StreamWriter(path, false))
                {
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");
                    foreach (var w in this)
                        writer.WriteLine(w.ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // JSON:
        public bool LoadJSON(string path)
        {
            Clear();

            if (!File.Exists(path))
                return false;

            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                var items = JsonSerializer.Deserialize<List<Weapon>>(json, options);
                if (items == null)
                    return true; // empty file -> treat as empty collection

                foreach (var w in items)
                {
                    if (w == null)
                    {
                        Clear();
                        return false;
                    }
                }

                this.AddRange(items);
                return true;
            }
            catch
            {
                Clear();
                return false;
            }
        }

        public bool SaveAsJSON(string path)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                string json = JsonSerializer.Serialize<List<Weapon>>(this.ToList(), options);
                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // XML:
        public bool LoadXML(string path)
        {
            Clear();

            if (!File.Exists(path))
                return false;

            try
            {
                var serializer = new XmlSerializer(typeof(WeaponCollection));
                using (var fs = File.OpenRead(path))
                {
                    var obj = serializer.Deserialize(fs) as WeaponCollection;
                    if (obj == null)
                        return true; // empty xml -> treat as empty collection

                    // Basic validation
                    foreach (var w in obj)
                    {
                        if (w == null)
                        {
                            Clear();
                            return false;
                        }
                    }

                    this.AddRange(obj);
                    return true;
                }
            }
            catch
            {
                Clear();
                return false;
            }
        }

        public bool SaveAsXML(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(WeaponCollection));
                using (var fs = File.Open(path, FileMode.Create))
                {
                    serializer.Serialize(fs, this);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Save / Load by extension:
        public bool Save(string filename)
        {
            string ext = Path.GetExtension(filename)?.ToLowerInvariant();
            switch (ext)
            {
                case ".csv":
                    return SaveAsCSV(filename);
                case ".json":
                    return SaveAsJSON(filename);
                case ".xml":
                    return SaveAsXML(filename);
                default:
                    // unsupported extension -> false
                    return false;
            }
        }

        public bool Load(string filename)
        {
            string ext = Path.GetExtension(filename)?.ToLowerInvariant();
            switch (ext)
            {
                case ".csv":
                    return LoadCSV(filename);
                case ".json":
                    return LoadJSON(filename);
                case ".xml":
                    return LoadXML(filename);
                default:
                    return false;
            }
        }

        public void SortBy(string columnName)
        {
            switch (columnName.ToLower())
            {
                case "name":
                    this.Sort(Weapon.CompareByName);
                    break;

                case "type":
                    this.Sort(Weapon.CompareByType);
                    break;

                case "rarity":
                    this.Sort(Weapon.CompareByRarity);
                    break;

                case "baseattack":
                    this.Sort(Weapon.CompareByBaseAttack);
                    break;

                default:
                    throw new ArgumentException("Invalid sort column");
            }
        }

    }
}