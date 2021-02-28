namespace Nojira.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Nojira.Client.RemoteLog.Uri = "http://localhost:1410";
            Nojira.Client.RemoteLog.Project = "nojira";

            Nojira.Client.RemoteLog.LogInfo("test", "test of an info message.");
            Nojira.Client.RemoteLog.LogWarning("test", "test of an warning message.");
            Nojira.Client.RemoteLog.LogError("test", "test of an error message.");

            Nojira.Client.RemoteLog.LogInfo("client", "Sending HTTP request...");
            Nojira.Client.RemoteLog.LogWarning("client", "HTTP request times out (try 1/3).");
            Nojira.Client.RemoteLog.LogError("client", "HTTP request times out (try 3/3).");
        }
    }
}
