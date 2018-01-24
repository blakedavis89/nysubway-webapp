using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Ipreo.Enums;
using System.Web.Caching;

namespace Ipreo.Classes
{
    public static class CacheHelper
    {
        /// <summary>
        /// Loads a list of subway names and a dictionary with their coordinates into cache.  
        /// </summary>
        public static void LoadSubwayInfoIntoCache()
        {
            string stopsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ReferenceFiles\stops.txt");

            try
            {
                if (File.Exists(stopsFilePath))
                {
                    using (StreamReader streamReader = new StreamReader(stopsFilePath))
                    {
                        int incrementor = 0;
                        Dictionary<string, Coordinates> subwayStops = new Dictionary<string, Coordinates>();
                        List<string> subwayNames = new List<string>();

                        while (!streamReader.EndOfStream)
                        {
                            string line = streamReader.ReadLine();

                            if (incrementor != 0)
                            {
                                string[] subwayInfo = line.Split(',');

                                //If the subway location is the parent station
                                if (subwayInfo[(int)SubwayIndex.LocationType] == "1")
                                {
                                    string stopName = subwayInfo[(int)SubwayIndex.StopName];
                                    double latitude;
                                    double longitude;
                                    bool parseLatitude = Double.TryParse(subwayInfo[(int)SubwayIndex.StopLatitude], out latitude);
                                    bool parseLongitude = Double.TryParse(subwayInfo[(int)SubwayIndex.StopLongitude], out longitude);

                                    if (!string.IsNullOrEmpty(stopName) && parseLatitude && parseLongitude)
                                    {
                                        if (!subwayNames.Contains(stopName))
                                        {
                                            subwayNames.Add(stopName);
                                            subwayStops.Add(stopName, new Coordinates { Latitude = latitude, Longitude = longitude });
                                        }
                                        else
                                        {
                                            bool matchingCoordinates = false;
                                            string[] existingNames = subwayNames.Where(s => s.Contains(stopName)).ToArray();

                                            foreach (string name in existingNames)
                                            {
                                                Coordinates existingCoordinates = subwayStops[name];

                                                if (existingCoordinates.Latitude == latitude && existingCoordinates.Longitude == longitude)
                                                {
                                                    matchingCoordinates = true;
                                                    break;
                                                }
                                            }

                                            if (!matchingCoordinates)
                                            {
                                                string stopId = subwayInfo[(int)SubwayIndex.StopId];
                                                subwayNames.Add(stopName + " - " + stopId);
                                                subwayStops.Add(stopName + " - " + stopId, new Coordinates { Latitude = latitude, Longitude = longitude });
                                            }
                                        }
                                    }
                                }
                            }

                            incrementor++;
                        }

                        if (subwayNames.Count > 0)
                        {
                            HttpRuntime.Cache.Add("SubwayNames", subwayNames, new CacheDependency(stopsFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                            HttpRuntime.Cache.Add("SubwayStops", subwayStops, new CacheDependency(stopsFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                        }
                    }
                }
                else
                {
                    throw new FileNotFoundException("Unable to locate the stops.txt file in the ReferenceFiles folder of the project.");
                }
            }
            catch (Exception ex)
            {
                //FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\ErrorLog.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite);

                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\ErrorLog.txt"), true))
                {
                    streamWriter.WriteLine(DateTime.Now.ToString() + " - " + ex.Message);
                }
            }
        }
    }
}