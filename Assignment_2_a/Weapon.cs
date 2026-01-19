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

        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }

        public WeaponType Type { get; set; }

        public int Rarity { get; set; }

        public int BaseAttack { get; set; }

        public string Image { get; set; }

        public string SecondaryState { get; set; }

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
            // TODO: construct a comma seperated value string
                   // Name, Type, Image, Rarity, BaseAttack, SecondaryState, Passive
            return $"{Name}, {Type}, {Image}, {Rarity}, {BaseAttack}, {SecondaryState}, {Passive}";
        }

        static bool TryParse(string rawData, out Weapon weapon)
        {
            // Order: Name, Type, Image, Rarity, BaseAttack, SecondaryState, Passive
            string[] values = rawData.Split(',');

            weapon = new Weapon();
            if (int.TryParse(values[0], out int baseAttack)) // Wrong index
            {
                weapon.BaseAttack = baseAttack;
            }
            else
            {
                Console.WriteLine($"BaseAttack {values[0]} is invalid format!");
                return false;
            }

            return true;
        }
    }
}