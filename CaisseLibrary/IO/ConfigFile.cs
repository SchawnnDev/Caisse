using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CaisseLibrary.IO
{
    public class ConfigFile
    {
        private static string ConfigPath { get; set; }

        public static void Init()
        {
            var configFolderPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Caisse");

            if (!Directory.Exists(configFolderPath))
                Directory.CreateDirectory(configFolderPath);

            ConfigPath = Path.Combine(configFolderPath, "config.xml");
        }

        public static Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> dic;

            if (!File.Exists(ConfigPath) || new FileInfo(ConfigPath).Length == 0) return new List<KeyValuePair<string, string>>().ToDictionary(t=>t.Key,t=>t.Value);

            var serializer = new XmlSerializer(typeof(List<KeyValuePair<string, string>>));

            using (var reader = new StreamReader(ConfigPath))
            {
                dic = ((List<KeyValuePair<string, string>>) serializer.Deserialize(reader)).ToDictionary(t => t.Key, t => t.Value);
                reader.Close();
            }

            return dic;
        }

        public static void SetValue(string key, string value)
        {
            var config = GetConfig();

            if (config.ContainsKey(key))
                config[key] = value;
            else
                config.Add(key, value);

            SaveConfig(config);
        }

        public static void SaveConfig(Dictionary<string, string> config)
        {
            if (!File.Exists(ConfigPath))
                File.Create(ConfigPath);

            var serializer = new XmlSerializer(typeof(List<KeyValuePair<string, string>>));

            try
            {
                File.WriteAllText(ConfigPath, "");
                using (var writer = new StreamWriter(ConfigPath, true))
                {
                    serializer.Serialize(writer, config.Select(t => new KeyValuePair<string,string>(t.Key,t.Value)).ToList());
                    writer.Close();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}