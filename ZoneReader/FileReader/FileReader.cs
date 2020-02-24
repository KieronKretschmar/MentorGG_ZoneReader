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


        private readonly Dictionary<ZoneType, string> zoneDirectories = new Dictionary<ZoneType, string>
        {
            { ZoneType.FireNade, "firenade_zones" },
            { ZoneType.Flash, "flash_zones" },
            { ZoneType.He, "he_zones" },
            { ZoneType.Position, "position_zones" },
            { ZoneType.Bomb, "bomb_zones" },
        };

        /// <summary>
        /// Holds data about all Zones
        /// </summary>
        private Dictionary<Tuple<ZoneType, Map>, ZoneCollection> ZoneCollection { get; set; }

        /// <summary>
        /// Holds data about all Lineups
        /// </summary>
        private Dictionary<Tuple<LineupType, Map>, ZoneCollection> LineupCollection { get; set; }

        public FileReader(ILogger<FileReader> logger, string resourcesPath)
        {
            this._logger = logger;
            this.resourcesPath = resourcesPath;
            var path = Path.GetFullPath(resourcesPath);

            // Create ZoneCollection
            ZoneCollection = new Dictionary<Tuple<ZoneType, Map>, ZoneCollection>();
            // .. iterate through all ZoneTypes
            foreach (var zoneType in zoneDirectories.Keys)
            {
                // Iterate through all files and add zones to Collection
                var zoneFiles = Directory.GetFiles(Path.Join(resourcesPath, zoneDirectories[zoneType]), zoneFilePattern);
                foreach (var zoneFile in zoneFiles)
                {
                    AddZoneFileToCollection(zoneType, zoneFile);
                }
            }

            // Create LineupCollection
            foreach (var lineupType in lineupDirectories)
            {

            }
        }

        public ZoneCollection GetZones(ZoneType zoneType, Map map)
        {
            var key = new Tuple<ZoneType, Map>(zoneType, map);
            if(ZoneCollection.ContainsKey(key))
            {
                return ZoneCollection[key];
            }
            return new ZoneCollection();
        }

        /// <summary>
        /// Adds the zones in the specified file to this.ZoneCollection
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mapEnum"></param>
        /// <returns></returns>
        private void AddZoneFileToCollection(ZoneType zoneType, string zoneFile)
        {
            // Determine key for dictionary
            var success = TryGetZoneMapFromFileName(zoneFile, out var mapEnum);
            if (!success)
            {
                _logger.LogInformation($"Skipping file {zoneFile} because map enum could not be determined.");
            }
            var dictKey = new Tuple<ZoneType, Map>(zoneType, mapEnum);

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
            if (!ZoneCollection.ContainsKey(dictKey))
            {
                ZoneCollection[dictKey] = new ZoneCollection();
            }

            // Set zones in collection
            ZoneCollection[dictKey].SetTeamZones(IsCtFile, jZones);
        }

        /// <summary>
        /// Tries to parse a fileName into a Map enum if the filename follows the designated pattern,
        /// e.g. "HE_de_cache_ct.GeoJSON" => Map.de_cache
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mapEnum"></param>
        /// <returns></returns>
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
