using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ManhattanLab
{
    class Program
    {
        static string data = ManhattanData.data;
        public static RootObject root = JsonConvert.DeserializeObject<RootObject>(data);
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int fullCount = Count(Neighborhoods);
            Console.WriteLine($"There are {fullCount} listed neighborhoods total:");
            Console.WriteLine(string.Join(", ", Neighborhoods));

            int fullCount2 = Count(NotEmptyNeighborhood);
            Console.WriteLine($"There are {fullCount2} listed neighborhoods total:");
            Console.WriteLine(string.Join(", ", NotEmptyNeighborhood));

            int fullCount3 = Count(NoDuplicates);
            Console.WriteLine($"There are {fullCount3} listed neighborhoods total:");
            Console.WriteLine(string.Join(", ", NoDuplicates));
            Coordinates();
        }

        public static int Count(IEnumerable<string> List)
        {
            int counter = 0;
            foreach (var n in List.Select((value, index) => new { value, index }))
            {
                counter = n.index;
            }
            return counter + 1;
        }

        public class ManhattanData
        {
            static string fileName = "data.json";
            public static string data = File.ReadAllText(fileName);

            public JObject manhattanJson = JObject.Parse(data);
        }

        public class RootObject
        {
            public string type { get; set; }

            public List<Feature> features { get; set; }
        }

        public class Feature
        {
            public string type { get; set; }

            public Geometry geometry { get; set; }

            public Properties properties { get; set; }
        }

        public class Geometry
        {
            public string type { get; set; }
            public double[] coordinates { get; set; }
        }

        public class Properties
        {
            public string zip { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string address { get; set; }
            public string borough { get; set; }
            public string neighborhood { get; set; }
            public string county { get; set; }
        }

        public static IEnumerable<string> Neighborhoods =
            from x in root.features
            select x.properties.neighborhood;

        public static IEnumerable<string> NotEmptyNeighborhood =
            from x in root.features
            where x.properties.neighborhood != ""
            select x.properties.neighborhood;

        public static IEnumerable<string> NoDuplicates =
            (from x in root.features
             select x.properties.neighborhood).Distinct();

        public static void Coordinates()
        {
            IEnumerable<double[]> coordinates = root.features
                .Select(x => x.geometry.coordinates);
            foreach(double[] coordinate in coordinates)
            {
                Console.WriteLine($"[{string.Join(",", coordinate)}] ");
            }
        }
    }
}