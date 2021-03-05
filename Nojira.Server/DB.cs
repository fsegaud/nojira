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

namespace Nojira.Server
{
    using System.Linq;

    public static class DB
    {
        private const string DefaultUsername = "admin";
        private const string DefaultPassword = "admin";

        private static SQLite.SQLiteConnection connection;

        public static void Init(bool createDefaultUser)
        {
            if (System.IO.File.Exists(Config.DatabasePath))
            {
                System.IO.File.Copy(Config.DatabasePath, Config.DatabasePrevPath, true);
            }

            System.Console.WriteLine($"Connecting to database '{Config.DatabasePath}'...");

            DB.connection = new SQLite.SQLiteConnection(Config.DatabasePath);
            DB.connection.CreateTable<User>();
            DB.connection.CreateTable<Log>();

            if (createDefaultUser && DB.CountUser() == 0)
            {
                DB.TryInsertUser(new User(DB.DefaultUsername, DB.DefaultPassword));

                System.Console.WriteLine("##################################################################");
                System.Console.WriteLine($"##     Created default user account ({DB.DefaultUsername}:{DB.DefaultPassword}).              ##");
                System.Console.WriteLine("##     Remember to create a proper one and remove this one.     ##");
                System.Console.WriteLine("##################################################################");
            }
        }

        public static bool TryInsertUser(User user)
        {
            if (DB.SelectUser(user.UserName) != null)
            {
                return false;
            }

            return DB.connection.Insert(user) == 1;
        }

        public static void InsertLog(Log log)
        {
            DB.connection.Insert(log);
        }

        public static User SelectUser(string username)
        {
            return DB.connection.Query<User>($"SELECT * FROM User WHERE UserName = '{DB.Escape(username)}';").FirstOrDefault();
        }

        public static int CountUser()
        {
            return DB.connection.ExecuteScalar<int>($"SELECT COUNT(Id) FROM User;");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLog()
        {
            return DB.connection.Query<Log>("SELECT * FROM Log;");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLog(string query)
        {
            return DB.connection.Query<Log>(query);
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLogByMachine(string machineName)
        {
            return DB.connection.Query<Log>($"SELECT * FROM Log WHERE MachineName = '{DB.Escape(machineName)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLogByProject(string project)
        {
            return DB.connection.Query<Log>($"SELECT * FROM Log WHERE Project = '{DB.Escape(project)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectLogBtProjectAndTag(string project, string tag)
        {
            return DB.connection.Query<Log>($"SELECT * FROM Log WHERE Project = '{DB.Escape(project)}' AND Tag = '{DB.Escape(tag)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> SelectMachineName()
        {
            return DB.connection.Query<Log>($"SELECT DISTINCT MachineName FROM Log;");
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

        [SQLite.Table("User")]
        public class User
        {
            public User()
            {
            }

            public User(string userName, string password)
            {
                this.UserName = userName;

                byte[] salt;
                new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                System.Array.Copy(salt, 0, hashBytes, 0, 16);
                System.Array.Copy(hash, 0, hashBytes, 16, 20);

                this.PasswordHash = System.Convert.ToBase64String(hashBytes);
            }

            [SQLite.PrimaryKey, SQLite.AutoIncrement]
            public int Id
            {
                get;
                set;
            }

            [SQLite.MaxLength(64)]
            public string UserName
            {
                get;
                set;
            }

            [SQLite.MaxLength(48)]
            public string PasswordHash
            {
                get;
                set;
            }

            public bool CheckPassword(string password)
            {
                byte[] hashBytes = System.Convert.FromBase64String(this.PasswordHash);

                byte[] salt = new byte[16];
                System.Array.Copy(hashBytes, 0, salt, 0, 16);

                System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; ++i)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        [SQLite.Table("Log")]
        public class Log
        {
            public Log()
            {
            }

            public Log(System.DateTime timestamp, string machineName, string type, string project, string tag, string message)
            {
                this.Timestamp = timestamp;
                this.MachineName = machineName;
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

            [SQLite.MaxLength(32)]
            public string MachineName
            {
                get;
                set;
            }

            [SQLite.MaxLength(32)]
            public string Type
            {
                get;
                set;
            }

            [SQLite.MaxLength(32)]
            public string Project
            {
                get;
                set;
            }

            [SQLite.MaxLength(32)]
            public string Tag
            {
                get;
                set;
            }

            [SQLite.MaxLength(1024)]
            public string Message
            {
                get;
                set;
            }
        }
    }
}
