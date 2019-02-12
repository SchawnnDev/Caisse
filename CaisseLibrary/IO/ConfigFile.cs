using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
            var dic=new Dictionary<string, string>();

            if (!File.Exists(ConfigPath) || new FileInfo(ConfigPath).Length == 0) return dic;

            var rootElement = XElement.Parse(File.ReadAllText(ConfigPath));

            foreach (var element in rootElement.Elements())
                dic.Add(element.Name.LocalName, element.Value);

            return dic;
        }

        public static void SetValues(Dictionary<string, string> values)
        {

            var config = GetConfig();

            foreach (var item in values)
            {

                if (config.ContainsKey(item.Key))
                    config[item.Key] = item.Value;
                else
                    config.Add(item.Key, item.Value);

            }

            SaveConfig(config);


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
            
            try
            {
                File.WriteAllText(ConfigPath, "");
                using (var writer = new StreamWriter(ConfigPath, true))
                {
                    var element = new XElement("root", config.Select(kv => new XElement(kv.Key, kv.Value)));
                    element.Save(writer);
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