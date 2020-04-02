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
        private const string zoneFilePattern = "*.GeoJSON";
        private const string lineupFilePattern = "*.xml";
        private readonly Regex mapFromZoneFileNameRegex = new Regex(@"((?:de|cs)_[a-z,2]*)(?:_ct|_t){0,1}(?:.GeoJSON)", RegexOptions.IgnoreCase);
        private readonly Regex mapFromLineupFileNameRegex = new Regex(@"((?:de|cs)_[a-z,2]*)(?:.xml)", RegexOptions.IgnoreCase);

        private readonly Dictionary<LineupType, string> lineupDirectories = new Dictionary<LineupType, string>
        {
            {LineupType.Smoke, "smoke_lineups" }
        };

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
        private Dictionary<Tuple<ZoneType, Map>, ZoneCollection> ZoneCollections { get; set; }

        /// <summary>
        /// Holds data about all Lineups
        /// </summary>
        private Dictionary<Tuple<LineupType, Map>, LineupCollection> LineupCollections { get; set; }

        public FileReader(ILogger<FileReader> logger, string resourcesPath)
        {
            this._logger = logger;

            // Create ZoneCollection
            ZoneCollections = new Dictionary<Tuple<ZoneType, Map>, ZoneCollection>();
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
            LineupCollections = new Dictionary<Tuple<LineupType, Map>, LineupCollection>();
            foreach (var lineupType in lineupDirectories.Keys)
            {
                // Iterate through all files (one per map) and add lineups to Collection
                var lineupFiles = Directory.GetFiles(Path.Join(resourcesPath, lineupDirectories[lineupType]), lineupFilePattern);
                foreach (var lineupFile in lineupFiles)
                {
                    AddLineupFileToCollection(lineupType, lineupFile);
                }
            }
        }

        /// <summary>
        /// Gets all lineups of the given type and on the given map, or an empty collection if no data is available.
        /// </summary>
        /// <param name="lineupType"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public LineupCollection GetLineups(LineupType lineupType, string map)
        {
            if (!Enum.IsDefined(typeof(Map), map))
            {
                return LineupCollection.GetEmptyCollection();
            }

            var mapEnum = Enum.Parse<Map>(map);
            var key = new Tuple<LineupType, Map>(lineupType, mapEnum);
            if (LineupCollections.ContainsKey(key))
            {
                return LineupCollections[key];
            }
            // Return empty collection if no data is found
            return LineupCollection.GetEmptyCollection();
        }

        /// <summary>
        /// Gets all lineups of the given type and on the given map, or an empty collection if no data is available.
        /// </summary>
        /// <param name="lineupType"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public LineupCollection GetLineups(LineupType lineupType, Map map)
        {
            var key = new Tuple<LineupType, Map>(lineupType, map);
            if (LineupCollections.ContainsKey(key))
            {
                return LineupCollections[key];
            }
            // Return empty collection if no data is found
            return LineupCollection.GetEmptyCollection();
        }

        /// <summary>
        /// Gets all zones of the given type, or an empty collection if no data is available.
        /// </summary>
        /// <param name="zoneType"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public ZoneCollection GetZones(ZoneType zoneType)
        {
            var res = new ZoneCollection();
            foreach (var zoneCollection in ZoneCollections.Where(x => x.Key.Item1 == zoneType).Select(x=>x.Value))
            {
                res.AddCollection(zoneCollection);
            }
            return res;
        }

        /// <summary>
        /// Gets all zones of the given type and on the given map, or an empty collection if no data is available.
        /// </summary>
        /// <param name="zoneType"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public ZoneCollection GetZones(ZoneType zoneType, string map)
        {
            if (!Enum.IsDefined(typeof(Map), map))
            {
                return new ZoneCollection();
            }

            var mapEnum = Enum.Parse<Map>(map);
            var key = new Tuple<ZoneType, Map>(zoneType, mapEnum);
            if (ZoneCollections.ContainsKey(key))
            {
                return ZoneCollections[key];
            }
            return new ZoneCollection();
        }

        /// <summary>
        /// Gets all zones of the given type and on the given map, or an empty collection if no data is available.
        /// </summary>
        /// <param name="zoneType"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public ZoneCollection GetZones(ZoneType zoneType, Map map)
        {
            var key = new Tuple<ZoneType, Map>(zoneType, map);
            if(ZoneCollections.ContainsKey(key))
            {
                return ZoneCollections[key];
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
            var success = TryGetMapFromFilePath(zoneFile, out var mapEnum);
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
            if (!ZoneCollections.ContainsKey(dictKey))
            {
                ZoneCollections[dictKey] = new ZoneCollection();
            }

            // Set zones in collection
            ZoneCollections[dictKey].SetTeamZones(IsCtFile, jZones);
        }

        /// <summary>
        /// Tries to parse a fileName into a Map enum if the filename contains the map's name,
        /// e.g. "HE_de_cache_ct.GeoJSON" => Map.de_cache
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mapEnum"></param>
        /// <returns></returns>
        public bool TryGetMapFromFilePath(string filePath, out Map mapEnum)
        {
            // Create default value, only to be returned upon failure
            mapEnum = Map.de_cache;

            var fileName = Path.GetFileName(filePath);

            // zones are .GeoJson, lineups are currently .xml. Select regex accordingly
            var regex = filePath.EndsWith(".GeoJSON") ? mapFromZoneFileNameRegex : mapFromLineupFileNameRegex;

            var mapMatch = regex.Match(fileName);
            if (!mapMatch.Success)
            {
                _logger.LogInformation($"Map name could not be determined for file {filePath}.");
                return false;
            }

            var mapName = mapMatch.Groups[1].Value;
            var parseSuccess = Enum.TryParse<Map>(mapName, true, out Map parsedMapEnum);
            if (!parseSuccess)
            {
                _logger.LogInformation($"MapEnum could not be determined from regex match {mapMatch.Value} for file {filePath}.");
                return false;
            }

            mapEnum = parsedMapEnum;
            return true;
        }

        /// <summary>
        /// Adds the lineups in the specified file to this.LineupCollection
        /// </summary>
        /// <param name="lineupType"></param>
        /// <param name="lineupFile"></param>
        private void AddLineupFileToCollection(LineupType lineupType, string lineupFile)
        {
            // Determine key for dictionary
            var success = TryGetMapFromFilePath(lineupFile, out var mapEnum);
            if (!success)
            {
                _logger.LogInformation($"Skipping lineup file {lineupFile} because map enum could not be determined.");
            }
            var dictKey = new Tuple<LineupType, Map>(lineupType, mapEnum);

            // Read zones of this file in legacy xml format
            var reader = new XMLReader();
            var collectionLegacyFormat = reader.Deserialize(lineupFile);

            var collection = collectionLegacyFormat.ToLineupCollection();

            LineupCollections[dictKey] = collection;
        }
    }
}
