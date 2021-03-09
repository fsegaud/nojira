// Copyright(c) 2021 François Ségaud
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace Nojira.Utils
{
    public static class Config
    {
        private const string FilePath = "config.json";

        private static UserConfig userConfig = null;

        public static string Title => Config.userConfig != null ? Config.userConfig.Title : "Nojira Server";

        public static string Subtitle => Config.userConfig != null ? Config.userConfig.Subtitle : "Remote Logging";

        public static string BaseUri => Config.userConfig != null ? Config.userConfig.BaseUri : "http://localhost:80";

        public static int MaxConnections => Config.userConfig != null ? Config.userConfig.MaxConnections : 16;

        public static bool RequireAuth => Config.userConfig != null ? Config.userConfig.RequireAuth : true;

        public static string DatabasePath => Config.userConfig != null ? Config.userConfig.DatabasePath : "nojira.db";

        public static string DatabasePrevPath => Config.userConfig != null ? Config.userConfig.DatabasePrevPath : "nojira-prev.db";

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
                    Subtitle = Config.Subtitle,
                    BaseUri = Config.BaseUri,
                    MaxConnections = Config.MaxConnections,
                    RequireAuth = Config.RequireAuth,
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
            public string Subtitle;
            public string BaseUri;
            public int MaxConnections;
            public bool RequireAuth;
            public string DatabasePath;
            public string DatabasePrevPath;
        }
    }
}
