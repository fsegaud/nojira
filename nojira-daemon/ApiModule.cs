// <copyright file="ApiModule.cs" company="AMPLITUDE Studios">Copyright AMPLITUDE Studios. All rights reserved.</copyright>

namespace Nojira.Daemon
{
    public sealed class ApiModule : Nancy.NancyModule
    {
        public ApiModule()
            : base("api")
        {
            this.Get("/log/{type}/{project}/{tag}/{message*}", _ =>
            {
                string formattedLog = $"[{System.DateTime.Now}] <{_.type}> {_.project}.{_.tag}: {_.message}";

                DB.Log log = new DB.Log(System.DateTime.Now, _.type, _.project, _.tag, _.message);
                DB.InsertLog(log);

                System.Console.WriteLine(formattedLog);
                return "OK";
            });
        }
    }
}
