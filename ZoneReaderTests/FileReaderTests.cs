using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZoneReader;
using ZoneReader.Enums;

namespace ZoneReaderTests
{
    [TestClass]
    public class FileReaderTests
    {
        [DataRow(MapZoneType.FireNade)]
        [DataRow(MapZoneType.Flash)]
        [DataRow(MapZoneType.He)]
        [DataRow(MapZoneType.Bomb)]
        [DataRow(MapZoneType.Position)]
        [DataTestMethod]
        public void FileReaderFindsZoneFiles(MapZoneType type)
        {
            var test = new FileReader(TestHelper.GetZoneResourcesDirectory());

            Dictionary<ZoneMap, MapZoneCollection> zones = new Dictionary<ZoneMap, MapZoneCollection>();

            foreach (ZoneMap map in Enum.GetValues(typeof(ZoneMap)))
            {
                var mapCollection = test.GetZones(type, map);
                zones[map] = mapCollection;
            }

            Assert.AreEqual(zones.Keys.Count, Enum.GetNames(typeof(ZoneMap)).Length);
            Assert.IsFalse(zones.Values.Contains(null));
        }

        [TestMethod]
        public void FileReaderFindsSmokeFiles()
        {
            var test = new FileReader(TestHelper.GetZoneResourcesDirectory());
            foreach (ZoneMap map in Enum.GetValues(typeof(ZoneMap)))
            {
                if (map == ZoneMap.De_Train)
                {
                    //Skip de_train as there are no smoke defined for it
                    continue;
                }

                var zones = test.GetSmokeZones(map);
                Assert.IsTrue(zones.LineUps.Any());
                Assert.IsTrue(zones.IdTargets.Keys.Any());
                Assert.IsFalse(zones.IdTargets.Values.Contains(null));
            }
        }

        [TestMethod]
        public void FileReaderCanDeserializeGeoJson()
        {
            var test = new FileReader(TestHelper.GetZoneResourcesDirectory());
            var mapCollection = test.GetZones(MapZoneType.Bomb, ZoneMap.De_Mirage);

            Assert.IsFalse(mapCollection.TeamZone.ContainsValue(null));
        }
    }
}
