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

    public sealed class SiteModule : Nancy.NancyModule
    {
        private const char ConditionSeparator = ';';
        private const char KeySeparator = '=';
        private const char ValueSeparator = ',';
        private const string ConditionRegex = @"^\w+=[A-Za-z0-9-._]+(,[A-Za-z0-9-._]+)*$";
        private static readonly string[] AllowedKeys = { "project", "tag", "type", "machinename" };

        public SiteModule()
            : base("/")
        {
            this.Get("/", args => { return View["index", new IndexModel(Nojira.Utils.Database.GetAllLogs())]; });
            
            this.Get("/machine/{machine}", args => { return View["index", new IndexModel(Nojira.Utils.Database.GetLogsPerMachine(args.machine))]; });

            this.Get("/project/{project}", args => { return View["index", new IndexModel(Nojira.Utils.Database.GetLogsPerProject(args.project))]; });

            this.Get("/project/{project}/{tag}", args => { return View["index", new IndexModel(Nojira.Utils.Database.GetLogsPerProjectAndTag(args.project, args.tag))]; });

            this.Get("/q/{query*}", args => { return View["index", new IndexModel(this.CustomQuery(args.query, out string error), args.query, error)]; });
        }

        private System.Collections.Generic.IEnumerable<Nojira.Utils.Database.Log> CustomQuery(string userQuery, out string error)
        {
            string queryCondition = string.Empty;
            userQuery = userQuery.Replace(" ", string.Empty);
            string[] conditions = userQuery.Split(SiteModule.ConditionSeparator);
            for (int conditionIndex = 0; conditionIndex < conditions.Length; conditionIndex++)
            {
                string condition = conditions[conditionIndex];
                if (string.IsNullOrEmpty(condition))
                {
                    continue;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(condition, SiteModule.ConditionRegex))
                {
                    error = $"Syntax error on '{condition}'";
                    return Nojira.Utils.Database.GetAllLogs();
                }

                string[] tokens = condition.Split(SiteModule.KeySeparator);
                string key = tokens[0];
                if (!SiteModule.AllowedKeys.Contains(key.ToLower()))
                {
                    error = $"Syntax error on '{condition}', parameter '{key}' is not allowed.";
                    return Nojira.Utils.Database.GetAllLogs();
                }

                string[] values = tokens[1].Split(SiteModule.ValueSeparator);
                string formattedValues = string.Empty;
                for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
                {
                    formattedValues += $"{(string.IsNullOrEmpty(formattedValues) ? string.Empty : ", ")}'{values[valueIndex]}'";
                }

                queryCondition += (string.IsNullOrEmpty(queryCondition) ? "WHERE " : " AND ") + $"{key} IN ({formattedValues})";
            }

            string sqlQuery = $"SELECT * FROM Log {queryCondition};";

            error = null;
            return Nojira.Utils.Database.QueryLogs(sqlQuery);
        }
    }
}
