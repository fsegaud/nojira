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

    public class IndexModel : MasterModel
    {
        public IndexModel(Nancy.NancyContext context, System.Collections.Generic.IEnumerable<Nojira.Utils.Database.Log> logs)
            : base(context)
        {
            this.context = context;
            this.Logs = logs;
            this.Query = string.Empty;
        }

        public IndexModel(Nancy.NancyContext context, System.Collections.Generic.IEnumerable<Nojira.Utils.Database.Log> logs, string query, string error)
            : this(context, logs)
        {
            this.Query = query;
            this.Error = error;
        }
        
        public System.Collections.Generic.IEnumerable<string> ProjectShortcuts => Nojira.Utils.Database.GetProjects().Select(p => p.Project);

        public System.Collections.Generic.IEnumerable<string> TagShortcuts => Nojira.Utils.Database.GetTags().Select(p => $"{p.Project}/{p.Tag}");

        public System.Collections.Generic.IEnumerable<string> MachineShortcuts => Nojira.Utils.Database.GetMachineNames().Select(p => p.MachineName);

        public bool HasError => !string.IsNullOrEmpty(this.Error);

        public override bool IsAdmin
        {
            get
            {
                if (this.context.CurrentUser == null || string.IsNullOrEmpty(this.context.CurrentUser.Identity.Name))
                {
                    return false;
                }

                Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser(this.context.CurrentUser.Identity.Name);
                if (user == null)
                {
                    return false;
                }

                return user.Admin;
            }
        }

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