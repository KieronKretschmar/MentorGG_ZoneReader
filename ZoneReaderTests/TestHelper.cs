using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.IO;
using System.Linq;

namespace ZoneReaderTests
{
    public static class TestHelper
    {
        public static readonly string TestDataFolderName = "TestData";

        public static string GetZippedTestDemoFilePath()
        {
            return GetTestFilePath("TestDemo_Valve1.dem.bz2");
        }

        public static string GetUnzippedTestDemoFilePath()
        {
            return GetTestFilePath("TestDemo_Valve1.dem");
        }

        public static string GetTestFilePath(string fileName)
        {
            var path = Path.Combine(GetTestDataFolderPath(), fileName);
            if (path.EndsWith(".dem") && !File.Exists(path))
            {
                throw new FileNotFoundException(".dem not found. You need to unzip it in order to run tests, since the unzipped file is too large for the repo.");
            }
            return path;
        }

        public static string GetEquipmentCsvDirectory()
        {
            return Path.Join(GetTestDataFolderPath(), "/EquipmentData");
        }

        public static string GetZoneResourcesDirectory()
        {
            return Path.Join(GetTestDataFolderPath(), "/Zones/resources");
        }

        public static string GetTestDataFolderPath()
        {
            string startupPath = ApplicationEnvironment.ApplicationBasePath;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, TestDataFolderName);
        }
    }
}
