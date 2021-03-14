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
    using Nancy.Extensions;

    public class AccountModule : Nancy.NancyModule
    {
        public AccountModule()
            : base("/account")
        {
            this.Get(
                "/{id}/{guid}",
                args =>
                {
                    Nojira.Utils.Database.UserToken userToken = Nojira.Utils.Database.GetUserToken(args.id);
                    if (userToken == null)
                    {
                        return $"No token found for user#{args.id}.";
                    }

                    if (userToken.Expire < System.DateTime.Now)
                    {
                        return $"Token for user#{args.id} expired.";
                    }

                    if (userToken.Guid != args.guid)
                    {
                        return $"Token for user#{args.id} does not match guid.";
                    }

                    Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser((int)args.id);
                    if (user == null)
                    {
                        return $"User#{args.id} not found.";
                    }

                    dynamic model = new System.Dynamic.ExpandoObject();
                    model.Title = Nojira.Utils.Config.Title;
                    model.Subtitle = Nojira.Utils.Config.Subtitle;

                    return this.View["reset-password", model];
                });

            this.Post(
                "/{id}/{guid}",
                args =>
                {
                    Nojira.Utils.Database.UserToken userToken = Nojira.Utils.Database.GetUserToken(args.id);
                    if (userToken == null)
                    {
                        return $"No token found for user#{args.id}.";
                    }

                    if (userToken.Expire < System.DateTime.Now)
                    {
                        return $"Token for user#{args.id} expired.";
                    }

                    if (userToken.Guid != args.guid)
                    {
                        return $"Token for user#{args.id} does not match guid.";
                    }

                    Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser((int)args.id);
                    if (user == null)
                    {
                        return $"User#{args.id} not found.";
                    }

                    if (string.IsNullOrEmpty(this.Request.Form.Password) || 
                        string.CompareOrdinal(this.Request.Form.Password, this.Request.Form.Confirm) != 0)
                    {
                        dynamic model = new System.Dynamic.ExpandoObject();
                        model.Title = Nojira.Utils.Config.Title;
                        model.Subtitle = Nojira.Utils.Config.Subtitle;
                        model.Error = true;

                        return this.View["reset-password", model];
                    }

                    Nojira.Utils.Database.DeleteUserToken(args.id);
                    user.SetPassword(this.Request.Form.Password);
                    Nojira.Utils.Database.UpdateUser(user);

                    return this.Context.GetRedirect("~/login");
                });
        }
    }
}
