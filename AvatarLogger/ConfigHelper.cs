using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ComfyUtils
{
    public class ConfigHelper<T> where T : class
    {
        public event Action OnConfigUpdated;
        private string ConfigPath { get; set; }
        private bool SaveOnUpdate { get; set; }
        private T InternalConfig;
        public T Config
        {
            get => InternalConfig;
            set
            {
                InternalConfig = value;
                if (SaveOnUpdate) { SaveConfig(); }
            }
        }
        public ConfigHelper(string configPath, bool saveOnUpdate = false)
        {
            ConfigPath = configPath;
            SaveOnUpdate = saveOnUpdate;
            if (!File.Exists(ConfigPath))
            { File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Activator.CreateInstance(typeof(T)), Formatting.Indented)); }
            InternalConfig = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(InternalConfig, Formatting.Indented));
            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(ConfigPath), Path.GetFileName(ConfigPath))
            { NotifyFilter = NotifyFilters.LastWrite, EnableRaisingEvents = true };
            watcher.Changed += UpdateConfig;
        }
        private void UpdateConfig(object obj, FileSystemEventArgs args)
        {
            T UpdatedConfig = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));
            foreach (PropertyInfo property in UpdatedConfig.GetType().GetProperties())
            {
                PropertyInfo property0 = InternalConfig.GetType().GetProperty(property.Name);
                if (property0 == null) { continue; }
                if (property.GetValue(UpdatedConfig) != property0.GetValue(InternalConfig))
                { InternalConfig = UpdatedConfig; OnConfigUpdated?.Invoke(); break; }
            }
        }
        public void SaveConfig() => File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(InternalConfig, Formatting.Indented));
    }
}
