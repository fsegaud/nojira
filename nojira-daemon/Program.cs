namespace Nojira.Daemon
{
    class Program
    {
        public const string Version = "v0.1-dev";

        static void Main(string[] args)
        {
            Config.Load();

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
            System.Console.WriteLine($"                            `8, ,8I   {Config.Title,28}");
            System.Console.WriteLine($"                             `Y8P\"    {Config.BaseUri,28}");
            System.Console.WriteLine("___________________________________________________________________");
            System.Console.WriteLine("");

            if (!Www.Load())
            {
                return;
            }

            DB.Init();

            Nancy.Hosting.Self.HostConfiguration cfg = new Nancy.Hosting.Self.HostConfiguration();
            cfg.UrlReservations.CreateAutomatically = true;
            cfg.MaximumConnectionCount = Config.MaxConnections;

            using (Nancy.Hosting.Self.NancyHost host = new Nancy.Hosting.Self.NancyHost(cfg, new System.Uri(Config.BaseUri)))
            {
                System.Console.WriteLine($"Listening on '{Config.BaseUri}'...");
                host.Start();

                System.Console.WriteLine("Done, done and done. Let's log! /o/");
                System.Console.WriteLine("-------------------------------------------------------------------");

                while (true)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }
    }
}
