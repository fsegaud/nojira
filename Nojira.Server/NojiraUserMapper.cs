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
    public class NojiraUserMapper : Nancy.Authentication.Forms.IUserMapper
    {
        public static System.Guid? ValidateUser(string username, string password)
        {
            Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser(username);

            if (user == null || !user.CheckPassword(password))
            {
                return null;
            }

            return user.Guid;
        }

        public System.Security.Claims.ClaimsPrincipal GetUserFromIdentifier(System.Guid identifier, Nancy.NancyContext context)
        {
            Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser(identifier);
            if (user == null)
            {
                return null;
            }

            return new System.Security.Claims.ClaimsPrincipal(new System.Security.Principal.GenericIdentity(user.UserName));
        }
    }
}
