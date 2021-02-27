// <copyright file="Config.cs" company="AMPLITUDE Studios">Copyright AMPLITUDE Studios. All rights reserved.</copyright>

namespace Nojira.Daemon
{
    public static class Config
    {
#if DEBUG
        private const string FilePath = "../../config.json";
#else
        private const string FilePath = "config.json";
#endif

        private static UserConfig userConfig = null;

        public static string Title => Config.userConfig != null ? Config.userConfig.Title : "CompanyName";

        public static string BaseUri => Config.userConfig != null ? Config.userConfig.BaseUri : "http://localhost:1410";

        public static int MaxConnections => Config.userConfig != null ? Config.userConfig.MaxConnections : 16;

        public static string DatabasePath => Config.userConfig != null ? Config.userConfig.DatabasePath : "logs.db";

        public static string DatabasePrevPath => Config.userConfig != null ? Config.userConfig.DatabasePrevPath: "logs-prev.db";

        public static void Load()
        {
            if (System.IO.File.Exists(Config.FilePath))
            {
                string json = System.IO.File.ReadAllText(Config.FilePath);
                Config.userConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<UserConfig>(json);
                Config.userConfig.BaseUri = Config.userConfig.BaseUri.TrimEnd('/');
            }
            else
            {
                UserConfig cfg = new UserConfig()
                {
                    Title = Config.Title,
                    BaseUri = Config.BaseUri,
                    MaxConnections = Config.MaxConnections,
                    DatabasePath = Config.DatabasePath,
                    DatabasePrevPath = Config.DatabasePrevPath
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(cfg, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(Config.FilePath, json);
            }
        }

        private class UserConfig
        {
            public string Title;
            public string BaseUri;
            public int MaxConnections;
            public string DatabasePath;
            public string DatabasePrevPath;
        }
    }
}