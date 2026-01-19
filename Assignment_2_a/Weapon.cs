using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2_a
{
    public class Weapon
    {
        public enum WeaponType
        {
            Sword,
            Polearm,
            Claymore,
            Catalyst,
            Bow,
            None
        }

        // CSV order:
        // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive
        public string Name { get; set; }

        public WeaponType Type { get; set; }

        public string Image { get; set; }

        public int Rarity { get; set; }

        public int BaseAttack { get; set; }

        public string SecondaryStat { get; set; }

        public string Passive { get; set; }

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }

        // TODO: add sort for each property:
        // CompareByType
        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }

        // CompareByRarity
        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }

        // CompareByBaseAttack
        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryParse(string rawData, out Weapon weapon)
        {
            weapon = null;

            if (string.IsNullOrWhiteSpace(rawData))
                return false;

            string[] values = rawData.Split(',');

            // Expect exactly 7 columns
            if (values.Length != 7)
                return false;

            // Parse WeaponType
            if (!Enum.TryParse(values[1], out WeaponType type))
                type = WeaponType.None;

            // Parse integers
            if (!int.TryParse(values[3], out int rarity))
                return false;

            if (!int.TryParse(values[4], out int baseAttack))
                return false;

            weapon = new Weapon
            {
                Name = values[0],
                Type = type,
                Image = values[2],
                Rarity = rarity,
                BaseAttack = baseAttack,
                SecondaryStat = values[5],
                Passive = values[6]
            };

            return true;
        }
    }
}