using System;
using System.IO;
using Newtonsoft.Json;

namespace ComfyUtils
{
    public class ConfigHelper<T> where T : class
    {
        public event Action OnConfigUpdated;
        private string ConfigPath { get; set; }
        public T Config { get; private set; }
        public ConfigHelper(string configPath)
        {
            ConfigPath = configPath;
            if (!File.Exists(ConfigPath))
            { File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Activator.CreateInstance(typeof(T)), Formatting.Indented)); }
            Config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));
			File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(ConfigPath), Path.GetFileName(ConfigPath))
            { NotifyFilter = NotifyFilters.LastWrite, EnableRaisingEvents = true };
            watcher.Changed += UpdateConfig;
        }
        private void UpdateConfig(object obj, FileSystemEventArgs args)
        {
            Config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));
            OnConfigUpdated.Invoke();
        }
		public void SaveConfig()
        => File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
    }
}
