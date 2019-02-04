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
            if (!File.Exists(ConfigPath))
                File.Create(ConfigPath);
        }

        public static Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> dic;
            var serializer = new XmlSerializer(typeof(Dictionary<string, string>));

            using (var reader = new StreamReader(ConfigPath))
            {
                dic = (Dictionary<string, string>) serializer.Deserialize(reader);
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
            var serializer = new XmlSerializer(typeof(Dictionary<string, string>));

            try
            {
                File.WriteAllText(ConfigPath, "");
                using (var writer = new StreamWriter(ConfigPath, true))
                {
                    serializer.Serialize(writer, config);
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