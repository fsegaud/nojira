// <copyright file="SiteModule.cs" company="AMPLITUDE Studios">Copyright AMPLITUDE Studios. All rights reserved.</copyright>

namespace Nojira.Daemon
{
    public sealed class SiteModule : Nancy.NancyModule
    {
        public SiteModule()
        {
            this.Get("/", _ =>
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

            this.Get("/{project}", _ =>
            {
                return new Nancy.Response()
                {
                    ContentType = "text/html",
                    Contents = stream =>
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Www.Index(Www.FormatArray(DB.SelectLog(_.project))));
                        stream.Write(bytes, 0, bytes.Length);
                    }
                };
            });

            this.Get("/{project}/{tag}", _ =>
            {
                return new Nancy.Response()
                {
                    ContentType = "text/html",
                    Contents = stream =>
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Www.Index(Www.FormatArray(DB.SelectLog(_.project, _.tag))));
                        stream.Write(bytes, 0, bytes.Length);
                    }
                };
            });
        }
    }
}
