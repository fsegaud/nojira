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
    public class DeleteUserAction : AdminAction
    {
        private readonly string username;

        public DeleteUserAction(string username)
        {
            this.username = username;
        }

        public override bool Execute()
        {
            Nojira.Utils.Database.Connect(Nojira.Utils.Config.DatabasePath, Nojira.Utils.Config.DatabasePrevPath);

            bool result = true;
            if (Nojira.Utils.Database.GetUser(this.username) != null)
            {
                Nojira.Utils.Database.DeleteUser(this.username);

                this.Status = $"Deleted user '{this.username}'.";
                result = true;
            }
            else
            {
                this.Status = $"User '{this.username}' does not exist exists.";
                result = false;
            }

            Nojira.Utils.Database.Disconnect();
            return result;
        }
    }
}
