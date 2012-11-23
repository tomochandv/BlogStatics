﻿using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ReportBlog.Web
{
    public class LoginCheck : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (!TomochanClass.TomochanLogin.LoginYn())
            {
                context.Result = new RedirectResult("/Home/Index");
            }
        }
    }
}