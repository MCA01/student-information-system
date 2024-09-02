using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StundetInformation.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            StudentInformationContext db = new StudentInformationContext();

            string userType = "";

            // Check the User table
            var user = db.User.SingleOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                if (db.Authorizations.Where(x => x.AuthorizationType.type == "Admin" && x.userID == user.id).SingleOrDefault() != null)
                {
                    userType = "admin";
                    Session["Admin"] = user;
                }
                else if (db.Authorizations.Where(x => x.AuthorizationType.type == "Instructor" && x.userID == user.id).SingleOrDefault() != null)
                {
                    userType = "instructor";
                    Session["Instructor"] = user;
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var student = db.Student.SingleOrDefault(u => u.email == email && u.password == password);

                if (student != null)
                {
                    userType = "student";
                    // Student authenticated
                    Session["Student"] = student; // To distinguish between user and student
                                                  // Redirect to the appropriate page
                    return RedirectToAction("Index", "Home");
                }
            }

            if (userType == "none")
            {
                // No matching user or student found
                return RedirectToAction("LoginPage", "Login");
            }

            // Redirect to the home page after successful login
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LoginPage", "Login");
        }

        public ActionResult NoneAuthorized()
        {
            TempData["Result"] = false;
            return RedirectToAction("LoginPage", "Login");
        }
    }
}
