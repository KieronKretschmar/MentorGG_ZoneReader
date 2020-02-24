using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using ZoneReader.Enums;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ZoneReader
{
    public class FileReader : IZoneReader
    {
        private readonly ILogger<FileReader> _logger;
        private readonly string resourcesPath;
        private const string smokesDirectory = "smokes";
        private const string zoneFilePattern = "*.GeoJSON";
        private readonly Regex mapFromZoneFileNameRegex = new Regex(@"((?:de|cs)_[a-z]*)(?:.GeoJSON)");

        private readonly Dictionary<MapZoneType, string> directories = new Dictionary<MapZoneType, string>
        {
            { MapZoneType.FireNade, "firenade_zones" },
            { MapZoneType.Flash, "flash_zones" },
            { MapZoneType.He, "he_zones" },
            { MapZoneType.Position, "position_zones" },
            { MapZoneType.Bomb, "bomb_zones" },
        };

        /// <summary>
        /// Holds data about all Zones
        /// </summary>
        private Dictionary<Tuple<MapZoneType, Map>, MapZoneCollection> MapZoneCollection { get; set; }

        public FileReader(ILogger<FileReader> logger, string resourcesPath)
        {
            this._logger = logger;
            this.resourcesPath = resourcesPath;
            var path = Path.GetFullPath(resourcesPath);

            // Create MapZoneCollection
            MapZoneCollection = new Dictionary<Tuple<MapZoneType, Map>, MapZoneCollection>();
            // .. iterate through all MapZoneTypes
            foreach (var mapZoneType in directories.Keys)
            {
                // Iterate through all files and add zones to Collection
                var zoneFiles = Directory.GetFiles(Path.Join(resourcesPath, directories[mapZoneType]), zoneFilePattern);
                foreach (var zoneFile in zoneFiles)
                {
                    AddZoneFileToCollection(mapZoneType, zoneFile);
                }
            }

        }

        public MapZoneCollection GetZones(MapZoneType zoneType, Map map)
        {
            var key = new Tuple<MapZoneType, Map>(zoneType, map);
            if(MapZoneCollection.ContainsKey(key))
            {
                return MapZoneCollection[key];
            }
            return new MapZoneCollection();
        }

        private void AddZoneFileToCollection(MapZoneType mapZoneType, string zoneFile)
        {
            // Determine key for dictionary
            var success = TryGetZoneMapFromFileName(zoneFile, out var mapEnum);
            if (!success)
            {
                _logger.LogInformation($"Skipping file {zoneFile} because map enum could not be determined.");
            }
            var dictKey = new Tuple<MapZoneType, Map>(mapZoneType, mapEnum);

            // Determine whether it's a CT or T zone file
            bool IsCtFile = false;
            if (Path.GetFileName(zoneFile).Contains("ct"))
            {
                IsCtFile = true;
            }

            // Read zones of this file
            string geoJson = File.ReadAllText(zoneFile);
            JsonZoneCollection jZones = JsonConvert.DeserializeObject<JsonZoneCollection>(geoJson);

            // Create entry in dictionary if it doesn't already exist, 
            // at this point it does exist if zones for the other team have already been loaded
            if (!MapZoneCollection.ContainsKey(dictKey))
            {
                MapZoneCollection[dictKey] = new MapZoneCollection();
            }

            // Set zones in collection
            MapZoneCollection[dictKey].SetTeamZones(IsCtFile, jZones);
        }
        public bool TryGetZoneMapFromFileName(string fileName, out Map mapEnum)
        {
            // Create default value, only to be returned upon failure
            mapEnum = Map.de_cache;

            var mapMatch = mapFromZoneFileNameRegex.Match(fileName);
            if (!mapMatch.Success)
            {
                _logger.LogInformation($"Map name could not be determined for file {fileName}.");
                return false;
            }

            var parseSuccess = Enum.TryParse<Map>(mapMatch.Value, true, out Map parsedMapEnum);
            if (!parseSuccess)
            {
                _logger.LogInformation($"MapEnum could not be determined from regex match {mapMatch.Value} for file {fileName}.");
                return false;
            }

            mapEnum = parsedMapEnum;
            return true;
        }

        public SmokeZoneCollection GetSmokeZones(Map map)
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
    }
}
