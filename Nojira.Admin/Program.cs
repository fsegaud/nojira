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

namespace Nojira.Admin
{
    using System.Linq;

    public class Program
    {
        private const string Help = "Nojira.Admin\n" +
                                    "\t-h --help\n" +
                                    "\t-l --list-users\n" +
                                    "\t-a --add-user username password\n" +
                                    "\t-d --delete-user username\n" +
                                    "\t-c --clear-logs";

        public static void Main(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                System.Console.WriteLine(Program.Help);
                return;
            }

            Nojira.Utils.Config.Load();

            System.Collections.Generic.Queue<AdminAction> actions = new System.Collections.Generic.Queue<AdminAction>();
            for (int argIndex = 0; argIndex < args.Length; ++argIndex)
            {
                switch (args[argIndex])
                {
                    case "-l":
                    case "--list-user":
                        {
                            actions.Enqueue(new ListUserAction());
                        }

                        break;

                    case "-a":
                    case "--add-user":
                        {
                            if (argIndex + 2 >= args.Length)
                            {
                                System.Console.Error.WriteLine("Bad Arguments.");
                                System.Console.WriteLine(Program.Help);
                                return;
                            }

                            string username = args[argIndex + 1];
                            string password = args[argIndex + 1];
                            actions.Enqueue(new AddUserAction(username, password));
                        }

                        break;

                    case "-d":
                    case "--delete-users":
                        {
                            if (argIndex + 1 >= args.Length)
                            {
                                System.Console.Error.WriteLine("Bad Arguments.");
                                System.Console.WriteLine(Program.Help);
                                return;
                            }

                            string username = args[argIndex + 1];
                            actions.Enqueue(new DeleteUserAction(username));
                        }

                        break;

                    case "-c":
                    case "--clear-logs":
                        {
                            actions.Enqueue(new ClearLogsAction());
                        }

                        break;
                }
            }

            if (actions.Count == 0)
            {
                System.Console.Error.WriteLine("Bad Arguments.");
                System.Console.WriteLine(Program.Help);
            }

            while (actions.Count > 0)
            {
                AdminAction action = actions.Dequeue();
                action.Execute();
                if (!string.IsNullOrEmpty(action.Status))
                {
                    System.Console.WriteLine(action.Status);
                }
            }
        }
    }
}
