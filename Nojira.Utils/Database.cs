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
    using System.Linq;

    public static class Database
    {
        private static SQLite.SQLiteConnection connection;

        public static void Connect(string dbPath, string dbBackupPath = null)
        {
            if (!string.IsNullOrEmpty(dbBackupPath) && System.IO.File.Exists(dbPath))
            {
                System.IO.File.Copy(dbPath, dbBackupPath, true);
            }

            Database.connection = new SQLite.SQLiteConnection(dbPath);
            Database.connection.CreateTable<User>();
            Database.connection.CreateTable<Log>();
        }

        public static void Disconnect()
        {
            Database.connection.Close();
        }

        public static bool CreateUser(User user)
        {
            if (Database.GetUser(user.UserName) != null)
            {
                return false;
            }

            return Database.connection.Insert(user) == 1;
        }

        public static System.Collections.Generic.IEnumerable<User> GetAllUsers()
        {
            return Database.connection.Query<User>($"SELECT * FROM User;");
        }

        public static User GetUser(string username)
        {
            return Database.connection.Query<User>($"SELECT * FROM User WHERE UserName = '{Database.Escape(username)}';").FirstOrDefault();
        }

        public static User GetUser(System.Guid guid)
        {
            return Database.connection.Query<User>($"SELECT * FROM User WHERE Guid = '{guid}';").FirstOrDefault();
        }

        public static int CountUser()
        {
            return Database.connection.ExecuteScalar<int>($"SELECT COUNT(Id) FROM User;");
        }

        public static void DeleteUser(string username)
        {
            Database.connection.ExecuteScalar<int>($"DELETE FROM User WHERE Username = '{Database.Escape(username)}';");
        }

        public static void DeleteUser(int id)
        {
            Database.connection.ExecuteScalar<int>($"DELETE FROM User WHERE Id = '{id}';");
        }

        public static void SetUserRole(int id, bool admin)
        {
            Database.connection.ExecuteScalar<int>($"UPDATE User Set Admin = '{(admin ? 1 : 0)}' WHERE Id = '{id}';");
        }

        public static void AddLog(Log log)
        {
            Database.connection.Insert(log);
        }

        public static System.Collections.Generic.IEnumerable<Log> QueryLogs(string query)
        {
            return Database.connection.Query<Log>(query);
        }

        public static System.Collections.Generic.IEnumerable<Log> GetAllLogs()
        {
            return Database.connection.Query<Log>("SELECT * FROM Log;");
        }

        public static System.Collections.Generic.IEnumerable<Log> GetLogsPerMachine(string machineName)
        {
            return Database.connection.Query<Log>($"SELECT * FROM Log WHERE MachineName = '{Database.Escape(machineName)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> GetLogsPerProject(string project)
        {
            return Database.connection.Query<Log>($"SELECT * FROM Log WHERE Project = '{Database.Escape(project)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> GetLogsPerProjectAndTag(string project, string tag)
        {
            return Database.connection.Query<Log>($"SELECT * FROM Log WHERE Project = '{Database.Escape(project)}' AND Tag = '{Database.Escape(tag)}';");
        }

        public static System.Collections.Generic.IEnumerable<Log> GetMachineNames()
        {
            return Database.connection.Query<Log>($"SELECT DISTINCT MachineName FROM Log;");
        }

        public static System.Collections.Generic.IEnumerable<Log> GetProjects()
        {
            return Database.connection.Query<Log>($"SELECT DISTINCT Project FROM Log;");
        }

        public static System.Collections.Generic.IEnumerable<Log> GetTags()
        {
            return Database.connection.Query<Log>($"SELECT DISTINCT Project, Tag FROM Log;");
        }

        public static void ClearAllLogs()
        {
            Database.connection.ExecuteScalar<int>($"DELETE FROM Log;");
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

            public User(string userName, string password, bool admin = false)
            {
                this.UserName = userName;
                this.Guid = System.Guid.NewGuid();
                this.Admin = admin;

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

            [SQLite.MaxLength(64)]
            public string PasswordHash
            {
                get;
                set;
            }

            public System.Guid Guid
            {
                get;
                set;
            }

            public bool Admin
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