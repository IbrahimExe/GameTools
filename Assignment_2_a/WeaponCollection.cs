using Assignment_2_a;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2_a
{
    public class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string filename)
        {
            Clear();

            if (!File.Exists(filename))
            {
                return false;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    // Skip header
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (Weapon.TryParse(line, out Weapon weapon))
                        {
                            Add(weapon);
                        }
                    }
                }
            }
            catch
            {
                Clear();
                return false;
            }

            return true;
        }

        public bool Save(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    // CSV Header
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                    foreach (Weapon weapon in this)
                    {
                        writer.WriteLine(weapon.ToString());
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public int GetHighestBaseAttack()
        {
            if (Count == 0)
                return 0;

            return this.Max(w => w.BaseAttack);
        }

        public int GetLowestBaseAttack()
        {
            if (Count == 0)
                return 0;

            return this.Min(w => w.BaseAttack);
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            return this.Where(w => w.Type == type).ToList();
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return this.Where(w => w.Rarity == stars).ToList();
        }

        public void SortBy(string columnName)
        {
            switch (columnName.ToLower())
            {
                case "name":
                    Sort(Weapon.CompareByName);
                    break;
                case "type":
                    Sort(Weapon.CompareByType);
                    break;
                case "rarity":
                    Sort(Weapon.CompareByRarity);
                    break;
                case "baseattack":
                    Sort(Weapon.CompareByBaseAttack);
                    break;
            }
        }
    }
}