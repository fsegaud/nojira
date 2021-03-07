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

    public class IndexModel
    {
        public IndexModel(System.Collections.Generic.IEnumerable<Nojira.Utils.Database.Log> logs)
        {
            this.Logs = logs;
            this.Query = string.Empty;
        }

        public IndexModel(System.Collections.Generic.IEnumerable<Nojira.Utils.Database.Log> logs, string query, string error)
            : this(logs)
        {
            this.Query = query;
            this.Error = error;
        }

        public string Title => Nojira.Utils.Config.Title;

        public string Version => Program.Version;

        public System.Collections.Generic.IEnumerable<string> ProjectShortcuts => Nojira.Utils.Database.GetProjects().Select(p => p.Project);

        public System.Collections.Generic.IEnumerable<string> TagShortcuts => Nojira.Utils.Database.GetTags().Select(p => $"{p.Project}/{p.Tag}");

        public System.Collections.Generic.IEnumerable<string> MachineShortcuts => Nojira.Utils.Database.GetMachineNames().Select(p => p.MachineName);

        public bool HasError => !string.IsNullOrEmpty(this.Error);

        public string Error
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        public System.Collections.Generic.IEnumerable<Nojira.Utils.Database.Log> Logs
        {
            get;
        }
    }
}
