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

    public class AdminModel : MasterModel
    {
        public AdminModel(Nancy.NancyContext context, bool isAdmin)
            : base(context)
        {
            this.context = context;
            this.IsAdmin = isAdmin;
        }

        public string Config => AdminModel.JsonToHtml(Nojira.Utils.Config.GetConfigFileContent());

        public Nojira.Utils.Database.User[] Users => Nojira.Utils.Database.GetAllUsers().ToArray();

        public override bool IsAdmin
        {
            get;
        }

        private static string JsonToHtml(string json)
        {
            string html = json.Replace("\n", "<br/>").Replace("    ", "&nbsp;&nbsp;&nbsp;&nbsp;");
            html = System.Text.RegularExpressions.Regex.Replace(html, "\"[A-Za-z0-9-_.:/]*\"", m => $"<span style=\"color:purple;\">{m}</span>");
            html = System.Text.RegularExpressions.Regex.Replace(html, "(?<=\\s)false|true(?=,)", m => $"<span style=\"color:blue;\">{m}</span>");
            html = System.Text.RegularExpressions.Regex.Replace(html, "(?<=\\s)\\d+(?=,)", m => $"<span style=\"color:red;\">{m}</span>");
            return html;
        }
    }
}
