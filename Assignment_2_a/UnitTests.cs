using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment_2_a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection WeaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            WeaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            // Expected Value: 48
            // TODO: call WeaponCollection.GetHighestBaseAttack() and confirm that it matches the expected value using asserts.
            WeaponCollection.Load(inputPath);
            Assert.That(WeaponCollection.GetHighestBaseAttack(), Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            // Expected Value: 23
            // TODO: call WeaponCollection.GetLowestBaseAttack() and confirm that it matches the expected value using asserts.
            WeaponCollection.Load(inputPath);
            Assert.That(WeaponCollection.GetLowestBaseAttack(), Is.EqualTo(23));
        }

        [TestCase(Weapon.WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(Weapon.WeaponType type, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfType(type) and confirm that the weapons list returns Count matches the expected value using asserts.
            WeaponCollection.Load(inputPath);
            Assert.That(WeaponCollection.GetAllWeaponsOfType(type).Count, Is.EqualTo(expectedValue));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars) and confirm that the weapons list returns Count matches the expected value using asserts.
            WeaponCollection.Load(inputPath);
            Assert.That(WeaponCollection.GetAllWeaponsOfRarity(stars).Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            // TODO: load returns true, expect WeaponCollection with count of 95 .
            Assert.That(WeaponCollection.Load(inputPath), Is.True);
            Assert.That(WeaponCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            // TODO: load returns false, expect an empty WeaponCollection
            Assert.That(WeaponCollection.Load("missing.csv!"), Is.False);
            Assert.That(WeaponCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty.
            WeaponCollection.Load(inputPath);

            Assert.That(WeaponCollection.Save(outputPath), Is.True);

            WeaponCollection loaded = new WeaponCollection();
            Assert.That(loaded.Load(outputPath), Is.True);
            Assert.That(loaded.Count, Is.GreaterThan(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.Save(outputPath));
            Assert.That(WeaponCollection.Load(outputPath));
            Assert.That(WeaponCollection.Count == 0);
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            //Weapon expected = null;
            // TODO: uncomment this once you added the Type1 and Type2
            //expected = new Weapon()
            //{
            //    Name = "Skyward Blade",
            //    Type = Weapon.Sword,
            //    Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
            //    Rarity = 5,
            //    BaseAttack = 46,
            //    SeconardStat = "Energy Recharge",55675675676767667667677676767676
            //    Passive = "Sky-Piercing Fang"
            //};

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            //Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            Assert.That(Weapon.TryParse(line, out Weapon weapon), Is.True);

            Assert.That(weapon.Name, Is.EqualTo("Skyward Blade"));
            Assert.That(weapon.Type, Is.EqualTo(Weapon.WeaponType.Sword));
            Assert.That(weapon.Rarity, Is.EqualTo(5));
            Assert.That(weapon.BaseAttack, Is.EqualTo(46));
            Assert.That(weapon.SecondaryStat, Is.EqualTo("Energy Recharge"));
            Assert.That(weapon.Passive, Is.EqualTo("Sky-Piercing Fang"));

            //Assert.That(expectedValue.Name, Is.EqualTo(actual.Name));
            //Assert.That(expected.Type, Is.EqualTo(actual.Type));
            //Assert.That(expected.BaseAttack, Is.EqualTo(actual.BaseAttack));
            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
            string invalidWeapon = "1,Bulbasaur,A,B,C,65,65";

            Assert.That(Weapon.TryParse(invalidWeapon, out Weapon weapon), Is.False);
            Assert.That(weapon, Is.Null);
        }
    }
}
