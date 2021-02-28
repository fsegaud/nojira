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

namespace Nojira.Daemon
{
    public class Program
    {
        public const string Version = "v0.1-dev";

        public static void Main(string[] args)
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
            System.Console.WriteLine(string.Empty);

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
