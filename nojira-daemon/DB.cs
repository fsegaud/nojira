// <copyright file="DB.cs" company="AMPLITUDE Studios">Copyright AMPLITUDE Studios. All rights reserved.</copyright>

namespace Nojira.Daemon
{
    public static class DB
    {
        private static SQLite.SQLiteConnection connection;

        public static void Init()
        {
            if (System.IO.File.Exists(Config.DatabasePath))
            {
                System.IO.File.Copy(Config.DatabasePath, Config.DatabasePrevPath, true);
            }

            System.Console.WriteLine($"Connecting to database '{Config.DatabasePath}'...");

            DB.connection = new SQLite.SQLiteConnection(Config.DatabasePath);
            DB.connection.CreateTable<Log>();
        }

        public static void InsertLog(Log log)
        {
            DB.connection.Insert(log);
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLog()
        {
            return DB.connection.Query<Log>("SELECT * FROM Log;");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLog(string project)
        {
            return DB.connection.Query<Log>($"SELECT * FROM Log WHERE Project = '{DB.Escape(project)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLog(string project, string tag)
        {
            return DB.connection.Query<Log>($"SELECT * FROM Log WHERE Project = '{DB.Escape(project)}' AND Tag = '{DB.Escape(tag)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectProject()
        {
            return DB.connection.Query<Log>($"SELECT DISTINCT Project FROM Log;");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectProjectTag()
        {
            return DB.connection.Query<Log>($"SELECT DISTINCT Project, Tag FROM Log;");
        }

        private static string Escape(string str)
        {
            return str.Replace("'", "''");
        }

        [SQLite.Table("Log")]
        public class Log
        {
            public Log()
            {
            }

            public Log(System.DateTime timestamp, string type, string project, string tag, string message)
            {
                this.Timestamp = timestamp;
                this.Type = type;
                this.Project = project;
                this.Tag = tag;
                this.Message = message;
            }

            [SQLite.PrimaryKey, SQLite.AutoIncrement]
            public int Id
            {
                get;
                set;
            }

            public System.DateTime Timestamp
            {
                get;
                set;
            }

            [SQLite.MaxLength(16)]
            public string Type
            {
                get;
                set;
            }

            [SQLite.MaxLength(16)]
            public string Project
            {
                get;
                set;
            }

            [SQLite.MaxLength(64)]
            public string Tag
            {
                get;
                set;
            }

            [SQLite.MaxLength(256)]
            public string Message
            {
                get;
                set;
            }
        }
    }
}
