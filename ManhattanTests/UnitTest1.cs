using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManhattanLab;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ManhattanTests
{
    public class UnitTest1
    {
        static string data = ManhattanData.data;
        public static RootObject root = JsonConvert.DeserializeObject<RootObject>(data);

        [Fact]
        public void Cam_read_our_file()
        {
            Assert.NotEmpty(data);
            Assert.Equal("FeatureCollection", root.type);
        }

        [Fact]
        public void Num_of_neighborhoods()
        {
            Assert.Equal(147, root.features.Count());
        }
        [Fact]
        public void Num_of_neighborhood_not_dup()
        {
            IEnumerable<string> notNull =
                from x in root.features
                where x.properties.neighborhood.Length > 0
                select x.properties.neighborhood;

            Assert.Equal(143, notNull.Count());
        }
        [Fact]
        public void Output39RemoveDuplicateNeighborhoods()
        {
            IEnumerable<string> nonDuplicates = root.features
                .Select(x => x.properties.neighborhood)
                .Where(neighborhood => !neighborhood.Equals(""))
                .Distinct();

            Assert.Equal(39, nonDuplicates.Count());
        }
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
}
