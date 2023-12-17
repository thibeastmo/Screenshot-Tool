using Newtonsoft.Json;
using System.IO;

namespace Galaxy_Life_Tool
{
    public class StoredData
    {
        private StoredData()
        {
            //prevents creating a new one
        }
        public static Data data;
        private const string DataFileLocation = ".\\StoredData.json";
        public static void Save()
        {
            if (data == null)
            {
                data = new Data();
            }
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(DataFileLocation, json);
        }
        public static void Read()
        {
            var json = string.Empty;
            if (!File.Exists(DataFileLocation))
            {
                Save();
            }
            json = File.ReadAllText(DataFileLocation);
            data = JsonConvert.DeserializeObject<Data>(json);
        }
        public static void Initialize()
        {
            Read();
            if (!Directory.Exists(data.SaveFolder))
            {
                Directory.CreateDirectory(data.SaveFolder);
            }
        }
        public class Data
        {
            public string SaveFolder { get; set; } = ".\\Saves";
            public int Hook { get; set; } = -1;
            public int ApplicationType { get; set; } = 0;
        }
        public enum ApplicationType
        {
            Steam = 11,
            Flash = 13
        }
    }
}
