using System.Linq;

namespace Nojira.Admin
{
    public class Program
    {
        private const string Help = "Nojira.Admin\n" +
                                    "\t-h --help\n" +
                                    "\t-l --list-user" +
                                    "\t-a --add-user username password\n" +
                                    "\t-d --delete-user username\n" +
                                    "\t-c --clear-logs";

        public static void Main(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                System.Console.WriteLine(Program.Help);
                return;
            }

            Nojira.Utils.Config.Load();

            System.Collections.Generic.Queue<AdminAction> actions = new System.Collections.Generic.Queue<AdminAction>();
            for (int argIndex = 0; argIndex < args.Length; ++argIndex)
            {
                switch (args[argIndex])
                {
                    case "-l":
                    case "--list-user":
                        {
                            actions.Enqueue(new ListUserAction());
                        }

                        break;

                    case "-a":
                    case "--add-user":
                        {
                            if (argIndex + 2 >= args.Length)
                            {
                                System.Console.Error.WriteLine("Bad Arguments.");
                                System.Console.WriteLine(Program.Help);
                                return;
                            }

                            string username = args[argIndex + 1];
                            string password = args[argIndex + 1];
                            actions.Enqueue(new AddUserAction(username, password));
                        }

                        break;

                    case "-d":
                    case "--delete-user":
                        {
                            if (argIndex + 1 >= args.Length)
                            {
                                System.Console.Error.WriteLine("Bad Arguments.");
                                System.Console.WriteLine(Program.Help);
                                return;
                            }

                            string username = args[argIndex + 1];
                            actions.Enqueue(new DeleteUserAction(username));
                        }

                        break;

                    case "-c":
                    case "--clear-logs":
                        {
                            actions.Enqueue(new ClearLogsAction());
                        }

                        break;
                }
            }

            if (actions.Count == 0)
            {
                System.Console.Error.WriteLine("Bad Arguments.");
                System.Console.WriteLine(Program.Help);
            }


            while (actions.Count > 0)
            {
                AdminAction action = actions.Dequeue();
                action.Execute();
                if (!string.IsNullOrEmpty(action.Status))
                {
                    System.Console.WriteLine(action.Status);
                }
            }
        }
    }

    public abstract class AdminAction
    {
        public string Status
        {
            get;
            protected set;
        }

        public abstract bool Execute();
    }

    public class ListUserAction : AdminAction
    {
        public override bool Execute()
        {
            Nojira.Utils.Database.Connect(Nojira.Utils.Config.DatabasePath, Nojira.Utils.Config.DatabasePrevPath);

            foreach (Nojira.Utils.Database.User user in Nojira.Utils.Database.GetAllUsers())
            {
                this.Status += $"{user.UserName}\n";
            }

            if (this.Status != null)
            {
                this.Status = this.Status.TrimEnd('\n');
            }
            else
            {
                this.Status = "No user found.";
            }

            Nojira.Utils.Database.Disconnect();

            return true;
        }
    }

    public class AddUserAction : AdminAction
    {
        private readonly string username;
        private readonly string password;

        public AddUserAction(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override bool Execute()
        {
            Nojira.Utils.Database.Connect(Nojira.Utils.Config.DatabasePath, Nojira.Utils.Config.DatabasePrevPath);

            bool result = true;
            if (Nojira.Utils.Database.GetUser(this.username) == null)
            {
                if (Nojira.Utils.Database.CreateUser(new Nojira.Utils.Database.User(this.username, this.password)))
                {
                    this.Status = $"Added user '{this.username}'.";
                    result = true;
                }
                else
                {
                    this.Status = $"Failed to add user '{this.username}'.";
                    result = false;
                }
            }
            else
            {
                this.Status = $"User '{this.username}' already exists.";
                result = false;
            }

            Nojira.Utils.Database.Disconnect();
            return result;
        }
    }

    public class DeleteUserAction : AdminAction
    {
        private readonly string username;

        public DeleteUserAction(string username)
        {
            this.username = username;
        }

        public override bool Execute()
        {
            Nojira.Utils.Database.Connect(Nojira.Utils.Config.DatabasePath, Nojira.Utils.Config.DatabasePrevPath);

            bool result = true;
            if (Nojira.Utils.Database.GetUser(this.username) != null)
            {
                Nojira.Utils.Database.DeleteUser(this.username);

                this.Status = $"Deleted user '{this.username}'.";
                result = true;
            }
            else
            {
                this.Status = $"User '{this.username}' does not exist exists.";
                result = false;
            }

            Nojira.Utils.Database.Disconnect();
            return result;
        }
    }

    public class ClearLogsAction : AdminAction
    {
        public override bool Execute()
        {
            Nojira.Utils.Database.Connect(Nojira.Utils.Config.DatabasePath, Nojira.Utils.Config.DatabasePrevPath);
            Nojira.Utils.Database.ClearAllLogs();
            Nojira.Utils.Database.Disconnect();

            this.Status = $"Logs cleared.";
            return true;
        }
    }
}