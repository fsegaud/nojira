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
    public class Program
    {
        public const string Version = "v0.4-dev";

        private const string DefaultUsername = "nojira";
        private const string DefaultPassword = "nojira";

        public static void Main(string[] args)
        {
            Nojira.Utils.Config.Load();

            System.Console.WriteLine("                                 gg   gg                         ");
            System.Console.WriteLine("                                 \"\"   \"\"                         ");
            System.Console.WriteLine("    ,ggg,,ggg,     ,ggggg,       gg   gg    ,gggggg,    ,gggg,gg ");
            System.Console.WriteLine("   ,8\" \"8P\" \"8,   dP\"  \"Y8ggg    8I   88    dP\"\"\"\"8I   dP\"  \"Y8I ");
            System.Console.WriteLine("   I8   8I   8I  i8'    ,8I     ,8I   88   ,8'    8I  i8'    ,8I ");
            System.Console.WriteLine("  ,dP   8I   Yb,,d8,   ,d8'   _,d8I _,88,_,dP     Y8,,d8,   ,d8b,");
            System.Console.WriteLine("  8P'   8I   `Y8P\"Y8888P\"   888P\"8888P\"\"Y88P      `Y8P\"Y8888P\"`Y8");
            System.Console.WriteLine("                               ,d8I'                             ");
            System.Console.WriteLine("                             ,dP'8I                              ");
            System.Console.WriteLine("                            ,8\"  8I                             ");
            System.Console.WriteLine($"                            I8   8I   {Program.Version,28}");
            System.Console.WriteLine($"                            `8, ,8I   {Nojira.Utils.Config.Title,28}");
            System.Console.WriteLine($"                             `Y8P\"    {Nojira.Utils.Config.BaseUri,28}");
            System.Console.WriteLine("___________________________________________________________________");
            System.Console.WriteLine(string.Empty);

            System.Console.WriteLine($"Connecting to database '{Nojira.Utils.Config.DatabasePath}'...");
            Nojira.Utils.Database.Connect(Nojira.Utils.Config.DatabasePath, Nojira.Utils.Config.DatabasePrevPath);
            if (Nojira.Utils.Database.CountUser() == 0)
            {
                Nojira.Utils.Database.CreateUser(new Nojira.Utils.Database.User(Program.DefaultUsername, Program.DefaultPassword));

                System.Console.WriteLine("##################################################################");
                System.Console.WriteLine($"##     Created default user account ({Program.DefaultUsername}:{Program.DefaultPassword}).            ##");
                System.Console.WriteLine("##     Remember to create a proper one and remove this one.     ##");
                System.Console.WriteLine("##################################################################");
            }

            if (!Www.Load())
            {
                return;
            }

            Nancy.Hosting.Self.HostConfiguration cfg = new Nancy.Hosting.Self.HostConfiguration();
            cfg.UrlReservations.CreateAutomatically = true;
            cfg.MaximumConnectionCount = Nojira.Utils.Config.MaxConnections;

            using (Nancy.Hosting.Self.NancyHost host = new Nancy.Hosting.Self.NancyHost(cfg, new System.Uri(Nojira.Utils.Config.BaseUri)))
            {
                System.Console.WriteLine($"Listening on '{Nojira.Utils.Config.BaseUri}'...");
                host.Start();

                System.Console.WriteLine("Done, done and done. Let's log! /o/");
                System.Console.WriteLine("-------------------------------------------------------------------");

                // TODO: 'q' to quit.
                while (true)
                {
                    System.ConsoleKeyInfo key = System.Console.ReadKey();
                    if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                    {
                        System.Console.WriteLine();
                        break;
                    }
                }
            }

            Nojira.Utils.Database.Disconnect();
        }
    }
}
