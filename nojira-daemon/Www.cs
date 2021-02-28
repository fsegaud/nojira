﻿// Copyright(c) 2021 François Ségaud
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
    public static class Www
    {
#if DEBUG
        private const string BasePath = "../../www/";
#else
        private const string BasePath = "www/";
#endif

        private static string index = null;

        public static bool Load()
        {
            System.Console.WriteLine("Loading Www resources...");

            string indexPath = System.IO.Path.Combine(Www.BasePath, "index.html");
            if (!System.IO.File.Exists(System.IO.Path.Combine(Www.BasePath, "index.html")))
            {
                System.Console.WriteLine($"Error: file '{indexPath}' not found!.");
                return false;
            }

            Www.index = System.IO.File.ReadAllText(indexPath);

            return true;
        }

        public static string Index(string content = null)
        {
            return Www.index
                .Replace("$Title", Config.Title)
                .Replace("$Version", Program.Version)
                .Replace("$Content", content ?? string.Empty);
        }

        public static string FormatArray(System.Collections.Generic.IEnumerable<DB.Log> logs)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("<table><tr><td><b>Projects</b></td><td>");
            sb.AppendLine($"<a href=\"/\">*</a> ");
            foreach (DB.Log log in DB.SelectProject())
            {
                sb.AppendLine($"<a href=\"/{log.Project}\">{log.Project}</a> ");
            }

            sb.AppendLine("</td></tr><tr><td><b>Tags</b></td><td>");

            foreach (DB.Log log in DB.SelectProjectTag())
            {
                sb.AppendLine($"<a href=\"/{log.Project}/{log.Tag}\">{log.Project}/{log.Tag}</a> ");
            }

            sb.AppendLine("</td></tr></table><br/>");

            sb.AppendLine("<table class=\"logs\">");
            sb.AppendLine($"<thead><tr><td>Timestamp</td><td>Machine</td><td>Type</td><td>Project</td><td>Tag</td><td>Message</td></tr></thead>");

            foreach (DB.Log log in logs)
            {
                string color = "#000000";
                switch (log.Type)
                {
                    case "info":
                        color = "#008000";
                        break;
                    case "warning":
                        color = "#ff8000";
                        break;
                    case "error":
                        color = "#ff0000";
                        break;
                }

                sb.AppendLine($"<tr><td>{log.Timestamp}</td><td>{log.MachineName}</td><td style=\"color:{color};\">{log.Type}</td><td>{log.Project}</td><td>{log.Tag}</td><td>{log.Message}</td></tr>");
            }

            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
