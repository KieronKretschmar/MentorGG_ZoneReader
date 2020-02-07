using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using ZoneReader.Enums;
using Newtonsoft.Json;

namespace ZoneReader
{
    public class FileReader : IZoneReader
    {
        private readonly string resourcesPath;
        private const string smokesDirectory = "smokes";
        private readonly Dictionary<MapZoneType, string> directories = new Dictionary<MapZoneType, string>
        {
            { MapZoneType.FireNade, "firenade_zones" },
            { MapZoneType.Flash, "flash_zones" },
            { MapZoneType.He, "he_zones" },
            { MapZoneType.Position, "position_zones" },
            { MapZoneType.Bomb, "bomb_zones" },
        };


        public FileReader(string resourcesPath)
        {
            this.resourcesPath = resourcesPath;
            var path = Path.GetFullPath(resourcesPath);
        }

        public SmokeZoneCollection GetSmokeZones(ZoneMap map)
        {
            var files = Directory.GetFiles(Path.Join(resourcesPath, smokesDirectory), "*" + map.ToString() + "*");
            var reader = new XMLReader();
            foreach (var file in files)
            {
                return new SmokeZoneCollection(reader.Deserialize(file));
            }

            //TODO OPTIONAL Add proper logging
            Console.WriteLine($"[SMOKE] No lineups found for map {map}");
            return SmokeZoneCollection.EmptyOnMap(map);
        }

        public MapZoneCollection GetZones(MapZoneType type, ZoneMap map)
        {
            var res = new MapZoneCollection();
            string mapPattern = "*" + map.ToString().ToLower() + "*";
            var zoneFiles = Directory.GetFiles(Path.Join(resourcesPath, directories[type]), mapPattern);

            foreach (string filePath in zoneFiles)
            {
                bool IsCt = false;
                if (Path.GetFileName(filePath).Contains("ct"))
                {
                     IsCt = true;
                }

                string geoJson = File.ReadAllText(filePath);
                JsonZoneCollection jZones = JsonConvert.DeserializeObject<JsonZoneCollection>(geoJson);

                res.SetTeamZones(IsCt, jZones);
            }

            return res;
        }
    }
}
