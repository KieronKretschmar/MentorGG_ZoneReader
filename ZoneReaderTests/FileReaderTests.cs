using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZoneReader;
using ZoneReader.Enums;
using Microsoft.Extensions.Logging;

namespace ZoneReaderTests
{
    [TestClass]
    public class FileReaderTests
    {
        private readonly ServiceProvider sp;

        public FileReaderTests()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            sp = services.BuildServiceProvider();
        }

        [DataRow(ZoneType.FireNade)]
        [DataRow(ZoneType.Flash)]
        [DataRow(ZoneType.He)]
        [DataRow(ZoneType.Bomb)]
        [DataRow(ZoneType.Position)]
        [DataTestMethod]
        public void FileReaderFindsZoneFiles(ZoneType type)
        {
            var test = new FileReader(sp.GetRequiredService<ILogger<FileReader>>(), TestHelper.GetZoneResourcesDirectory());

            Dictionary<Map, ZoneCollection> zones = new Dictionary<Map, ZoneCollection>();

            foreach (Map map in Enum.GetValues(typeof(Map)))
            {
                var mapCollection = test.GetZones(type, map);
                zones[map] = mapCollection;
            }

            Assert.AreEqual(zones.Keys.Count, Enum.GetNames(typeof(Map)).Length);
            Assert.IsFalse(zones.Values.Contains(null));
        }

        [TestMethod]
        public void FileReaderFindsSmokeFiles()
        {
            var test = new FileReader(sp.GetRequiredService<ILogger<FileReader>>(), TestHelper.GetZoneResourcesDirectory());
            foreach (Map map in Enum.GetValues(typeof(Map)))
            {
                if (map == Map.de_train)
                {
                    //Skip de_train as there are no smoke defined for it
                    continue;
                }

                var zones = test.GetLineups(LineupType.Smoke, map);
                Assert.IsTrue(zones.IdLineUps.Any());
                Assert.IsTrue(zones.IdTargets.Keys.Any());
                Assert.IsFalse(zones.IdTargets.Values.Contains(null));
            }
        }

        [TestMethod]
        public void FileReaderCanDeserializeGeoJson()
        {
            var test = new FileReader(sp.GetRequiredService<ILogger<FileReader>>(), TestHelper.GetZoneResourcesDirectory());
            var mapCollection = test.GetZones(ZoneType.Bomb, Map.de_mirage);

            Assert.IsFalse(mapCollection.TeamZone.ContainsValue(null));
        }
    }
}
