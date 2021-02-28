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
    public sealed class SiteModule : Nancy.NancyModule
    {
        public SiteModule()
        {
            this.Get(
                "/", 
                args =>
            {
                return new Nancy.Response()
                {
                    ContentType = "text/html",
                    Contents = stream =>
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Www.Index(Www.FormatArray(DB.SelectLog())));
                        stream.Write(bytes, 0, bytes.Length);
                    }
                };
            });

            this.Get(
                "/machine/{machine}",
                args =>
                {
                    return new Nancy.Response()
                    {
                        ContentType = "text/html",
                        Contents = stream =>
                        {
                            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Www.Index(Www.FormatArray(DB.SelectLogByMachine(args.machine))));
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    };
                });

            this.Get(
                "/project/{project}", 
                args =>
            {
                return new Nancy.Response()
                {
                    ContentType = "text/html",
                    Contents = stream =>
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Www.Index(Www.FormatArray(DB.SelectLogByProject(args.project))));
                        stream.Write(bytes, 0, bytes.Length);
                    }
                };
            });

            this.Get(
                "/project/{project}/{tag}", 
                args =>
            {
                return new Nancy.Response()
                {
                    ContentType = "text/html",
                    Contents = stream =>
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Www.Index(Www.FormatArray(DB.SelectLogBtProjectAndTag(args.project, args.tag))));
                        stream.Write(bytes, 0, bytes.Length);
                    }
                };
            });
        }
    }
}
