namespace Nojira.Client
{
    public class RemoteLog
    {
        private static string uri = "http://localhost:1410";
        private static string project = "default";

        public static System.Exception LastException= null;

        public static void Init(string uri, string project)
        {
            RemoteLog.uri = uri.TrimEnd('/');
            RemoteLog.project = project;
        }

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
                System.Net.WebRequest request = System.Net.WebRequest.Create($"{uri}/api/log/{type}/{project}/{tag}/{message}");
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
