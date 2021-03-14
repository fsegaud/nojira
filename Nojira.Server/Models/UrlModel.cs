// <copyright file="UrlModel.cs" company="AMPLITUDE Studios">Copyright AMPLITUDE Studios. All rights reserved.</copyright>

namespace Nojira.Server
{
    public class UrlModel : MasterModel
    {
        public UrlModel(Nancy.NancyContext context, bool isAdmin, string url)
            : base(context)
        {
            this.IsAdmin = isAdmin;
            this.Url = url;
        }

        public override bool IsAdmin
        {
            get;
        }

        public string Url
        {
            get;
        }
    }
}
