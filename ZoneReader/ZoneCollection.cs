using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZoneReader
{
    /// <summary>
    /// Collection of zones for a particular map.
    /// </summary>
    public class ZoneCollection : IEnumerable<KeyValuePair<bool, List<Zone>>>
    {
        public Dictionary<bool, List<Zone>> TeamZone { get; }

        public ZoneCollection()
        {
            TeamZone = new Dictionary<bool, List<Zone>>
            {
                {false, new List<Zone>() },
                {true, new List<Zone>() },
            };
        }

        public void SetTeamZones(bool team, List<Zone> zones)
        {
            TeamZone[team] = zones;
        }

        public List<Zone> this[bool team] => TeamZone[team];

        public IEnumerator<KeyValuePair<bool, List<Zone>>> GetEnumerator()
        {
            return TeamZone.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Zone> Values()
        {
            return TeamZone.Values.SelectMany(x=> x).ToList();
        }

        public void AddCollection(ZoneCollection other) 
        {
            foreach (var key in TeamZone.Keys)
            {
                TeamZone[key].AddRange(other.TeamZone[key]);
            }
        }

        /// <summary>
        /// Returns a dictionary with all zones.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Zone> ToZoneDict()
        {
            return TeamZone.Values.SelectMany(x => x).ToDictionary(x => x.Id, x => x);
        }
    }
}