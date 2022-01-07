using Newtonsoft.Json;
using System;
using System.IO;

namespace Diary.App.Models;

public class AppSettings
{
    private static Data? _data = null;

    public class Data
    {
        protected bool Equals(Data other)
        {
            return RedMineHost == other.RedMineHost && RedMinePort == other.RedMinePort &&
                   RedMineUserApiKey == other.RedMineUserApiKey;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Data)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RedMineHost, RedMinePort, RedMineUserApiKey);
        }

        #region RedMine 设置

        public string RedMineHost { get; set; } = "";
        public int RedMinePort { get; set; }
        public string RedMineUserApiKey { get; set; } = "";

        #endregion RedMine 设置
    }

    public static Data Load()
    {
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appName = AppDomain.CurrentDomain.FriendlyName;
        var jsonFilePath = $@"{appDataDir}/{appName}/settings.json";
        if (File.Exists(jsonFilePath))
        {
            var jsonText = File.ReadAllText(jsonFilePath);
            _data = JsonConvert.DeserializeObject<Data>(jsonText);
        }

        return _data ??= new Data() { RedMineHost = String.Empty, RedMinePort = 3000 };
    }

    public static void Store(Data newData)
    {
        if (_data == null || !_data.Equals(newData))
        {
            _data = newData;
            var text = JsonConvert.SerializeObject(_data, Formatting.None);
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = AppDomain.CurrentDomain.FriendlyName;
            var jsonFilePath = $@"{appDataDir}/{appName}/settings.json";
            File.WriteAllText(jsonFilePath, text);
        }
    }

    public static Data GetConfig()
    {
        if (_data == null)
            Load();
        return _data!;
    }
}