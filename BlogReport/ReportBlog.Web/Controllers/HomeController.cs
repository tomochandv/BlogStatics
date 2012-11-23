using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportBlog.Dac;

namespace ReportBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string email, string pwd)
        {
            object result = null;
            Member member = new Member();
            member.email = email;
            member.pwd = pwd;

            if (member.LoginMember(member).Rows.Count > 0)
            {
                result = 1;
                Dictionary<string,string> dic = new Dictionary<string,string>();
                dic.Add("admin","admin");
                TomochanClass.TomochanLogin.CreateLoginCookie(dic);
            }
            else
            {
                result = 0;
            }
            return Json(result);
        }

        [LoginCheck]
        public ActionResult Member()
        {
            return View();
        }

        public ActionResult MemberList()
        {
            return View(new Member().ListMember());
        }

        [HttpPost]
        public JsonResult MemberInsert(string txtEmail, string txtPassword)
        {
            object result = null;
            Member member = new Member();
            member.email = txtEmail;
            member.pwd = txtPassword;

            if (member.ViewMember(member).Rows.Count > 0)
            {
                result = 0;
            }
            else
            {

                result = member.InsertMember(member);
            }
           return Json(result);
        }

        public JsonResult MemberDelete(int idx)
        {
            Member member = new Member();
            member.idx = idx;

            return Json(member.DeleteMember(member));
        }

        [LoginCheck]
        public ActionResult Main()
        {
            return View();
        }
    }
}
