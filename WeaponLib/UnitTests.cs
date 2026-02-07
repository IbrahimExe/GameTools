using NUnit.Framework;
using System;
using System.IO;

namespace WeaponLib
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection collection;
        private string inputPath;
        private string weaponsJson;
        private string weaponsCsv;
        private string weaponsXml;
        private string emptyJson;
        private string emptyCsv;
        private string emptyXml;

        const string INPUT_FILE = "data2.csv";

        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            collection = new WeaponCollection();
            inputPath = CombineToAppPath(INPUT_FILE);
            weaponsJson = CombineToAppPath("weapons.json");
            weaponsCsv = CombineToAppPath("weapons.csv");
            weaponsXml = CombineToAppPath("weapons.xml");
            emptyJson = CombineToAppPath("empty.json");
            emptyCsv = CombineToAppPath("empty.csv");
            emptyXml = CombineToAppPath("empty.xml");

            // ensure no leftover files
            DeleteIfExists(weaponsJson);
            DeleteIfExists(weaponsCsv);
            DeleteIfExists(weaponsXml);
            DeleteIfExists(emptyJson);
            DeleteIfExists(emptyCsv);
            DeleteIfExists(emptyXml);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteIfExists(weaponsJson);
            DeleteIfExists(weaponsCsv);
            DeleteIfExists(weaponsXml);
            DeleteIfExists(emptyJson);
            DeleteIfExists(emptyCsv);
            DeleteIfExists(emptyXml);
        }

        private void DeleteIfExists(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }



        // JSON:load/save tests
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.Count, Is.GreaterThan(0));

            Assert.That(collection.Save(weaponsJson), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(weaponsJson), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.SaveAsJSON(weaponsJson), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(weaponsJson), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.SaveAsJSON(weaponsJson), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.LoadJSON(weaponsJson), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.Save(weaponsJson), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.LoadJSON(weaponsJson), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }



        // CSV: load/save tests
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.Save(weaponsCsv), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(weaponsCsv), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.SaveAsCSV(weaponsCsv), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.LoadCSV(weaponsCsv), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }



        // XML: load/save tests
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.Save(weaponsXml), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(weaponsXml), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.SaveAsXML(weaponsXml), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.LoadXML(weaponsXml), Is.True);
            Assert.That(other.Count, Is.EqualTo(collection.Count));
        }



        // Empty save/load tests
        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            collection.Clear();
            Assert.That(collection.SaveAsJSON(emptyJson), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(emptyJson), Is.True);
            Assert.That(other.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            collection.Clear();
            Assert.That(collection.SaveAsCSV(emptyCsv), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(emptyCsv), Is.True);
            Assert.That(other.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            collection.Clear();
            Assert.That(collection.SaveAsXML(emptyXml), Is.True);

            var other = new WeaponCollection();
            Assert.That(other.Load(emptyXml), Is.True);
            Assert.That(other.Count, Is.EqualTo(0));
        }



        // Invalid format tests
        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.SaveAsJSON(weaponsJson), Is.True);

            var other = new WeaponCollection();
            // trying to load JSON with XML loader should fail
            Assert.That(other.LoadXML(weaponsJson), Is.False);
            Assert.That(other.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            Assert.That(collection.Load(inputPath), Is.True);
            Assert.That(collection.SaveAsXML(weaponsXml), Is.True);

            var other = new WeaponCollection();
            // trying to load XML with JSON loader = Fail
            Assert.That(other.LoadJSON(weaponsXml), Is.False);
            Assert.That(other.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            // Loading XML from CSV file = Fail
            var other = new WeaponCollection();
            Assert.That(other.LoadXML(inputPath), Is.False);
            Assert.That(other.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            // Loading JSON from CSV file = Fail
            var other = new WeaponCollection();
            Assert.That(other.LoadJSON(inputPath), Is.False);
            Assert.That(other.Count, Is.EqualTo(0));
        }
    }
}
