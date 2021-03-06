﻿// Copyright(c) 2021 François Ségaud
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
    using Nancy;
    using Nancy.Security;

    public class AdminModule : Nancy.NancyModule
    {
        public AdminModule()
            : base("/admin")
        {
            this.RequiresAuthentication();

            this.Get(
                "/",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    return this.View["admin", new AdminModel(this.Context, this.IsAdmin())];
                });

            this.Post(
                "/",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    string username = this.Request.Form.Username;
                    string password = this.Request.Form.Password;

                    if (!string.IsNullOrEmpty(username))
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            Nojira.Utils.Database.CreateUser(new Nojira.Utils.Database.User(username, password));
                            return Response.AsRedirect("/admin");
                        }
                        else
                        {
                            int id = Nojira.Utils.Database.CreateUser(new Nojira.Utils.Database.User(username));
                            return Response.AsRedirect($"/admin/user/{id}/reset");
                        }
                    }

                    return Response.AsRedirect("/admin");
                });

            this.Get(
                "/clear-logs",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    Nojira.Utils.Database.ClearAllLogs();
                    return Response.AsRedirect("/admin");
                });

            this.Get(
                "/purge-tokens",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    Nojira.Utils.Database.DeleteAllUserTokens();
                    return Response.AsRedirect("/admin");
                });

            this.Get(
                "/shutdown",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    Program.ExitRequest = true;
                    return "Shutting down...";
                });

            this.Get(
                "/user/{id}/delete",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    Nojira.Utils.Database.DeleteUser(int.Parse(args.id));
                    return Response.AsRedirect("/admin");
                });

            this.Get(
                "/user/{id}/promote",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    Nojira.Utils.Database.SetUserRole(int.Parse(args.id), true);
                    return Response.AsRedirect("/admin");
                });

            this.Get(
                "/user/{id}/demote",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    Nojira.Utils.Database.SetUserRole(int.Parse(args.id), false);
                    return Response.AsRedirect("/admin");
                });

            this.Get(
                "/user/{id}/reset",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    string guid = System.Guid.NewGuid().ToString();
                    System.DateTime expire = System.DateTime.Now.AddDays(1);
                    Nojira.Utils.Database.CreateUserToken(new Nojira.Utils.Database.UserToken(args.id, guid, expire));

                    return Response.AsRedirect($"/admin/url/account/{args.id}/{guid}");
                });

            this.Get(
                "/url/{url*}",
                args =>
                {
                    if (!this.IsAdmin())
                    {
                        return Response.AsRedirect("/");
                    }

                    return this.View["url", new UrlModel(this.Context, this.IsAdmin(), Nojira.Utils.Config.BaseUri.TrimEnd('/') + "/" + args.Url)]; 
                });
        }

        public bool IsAdmin()
        {
            if (this.Context.CurrentUser == null || string.IsNullOrEmpty(this.Context.CurrentUser.Identity.Name))
            {
                return false;
            }

            Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser(this.Context.CurrentUser.Identity.Name);
            if (user == null)
            {
                return false;
            }

            return user.Admin;
        }
    }
}
