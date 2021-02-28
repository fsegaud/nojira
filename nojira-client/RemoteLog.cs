namespace Nojira.Client
{
    public class RemoteLog
    {
        public static string Uri = "http://localhost:1410";
        public static string Project = "default";

        public static System.Exception LastException= null;

        public static bool LogInfo(string tag, string message)
        {
            return RemoteLog.Log("info", tag, message);
        }

        public static bool LogWarning(string tag, string message)
        {
            return RemoteLog.Log("warning", tag, message);
        }

        public static bool LogError(string tag, string message)
        {
            return RemoteLog.Log("error", tag, message);
        }

        private static bool Log(string type, string tag, string message)
        {
            if (RemoteLog.LastException != null)
            {
                return false;
            }

            try
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create($"{Uri.TrimEnd('/')}/api/log/{type}/{Project}/{tag}/{message}");
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();

                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (System.Exception e)
            {
                RemoteLog.LastException = e;
                return false;
            }
        }
    }
}
