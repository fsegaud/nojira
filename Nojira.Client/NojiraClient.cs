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

namespace Nojira.Client
{
    public class NojiraClient
    {
        public static string Uri = "http://localhost:1410";
        public static string Project = "default";

        public static System.Exception LastException = null;

        public static bool LogInfo(string tag, string message)
        {
            return NojiraClient.Log("info", tag, message);
        }

        public static bool LogWarning(string tag, string message)
        {
            return NojiraClient.Log("warning", tag, message);
        }

        public static bool LogError(string tag, string message)
        {
            return NojiraClient.Log("error", tag, message);
        }

        private static bool Log(string type, string tag, string message)
        {
            if (NojiraClient.LastException != null)
            {
                return false;
            }

            string machine = System.Environment.MachineName;

            try
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create($"{NojiraClient.Uri.TrimEnd('/')}/api/log/{machine}/{type}/{NojiraClient.Project}/{tag}/{message}");
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();

                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (System.Exception e)
            {
                NojiraClient.LastException = e;
                return false;
            }
        }
    }
}
