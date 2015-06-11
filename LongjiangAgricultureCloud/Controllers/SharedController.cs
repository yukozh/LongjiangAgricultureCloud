﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace LongjiangAgricultureCloud.Controllers
{
    public class SharedController : BaseController
    {
        // GET: Shared
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("Message")]
        public ActionResult Message(string msg, string sid)
        {
            if (Session[sid].ToString() != sid)
                return RedirectToAction("NoAccess", "Shared", null);
            ViewBag.Msg = msg;
            return View();
        }

        [Route("Login")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(string Username, string Password, string returnUrl)
        {
            var pwd = Helpers.Security.SHA1(Password);
            var user = (from u in DB.Users
                        where u.Username == Username
                        && u.Password == pwd
                        && u.Role >= Models.UserRole.大区经理
                        select u).SingleOrDefault();
            if (user == null)
            {
                return Msg("用户名或密码错误！");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(Username, false);
                return Redirect(returnUrl ?? "/");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Shared");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}