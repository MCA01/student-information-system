using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            StudentInformationContext db = new StudentInformationContext();

            var users = db.User.ToList();

            return View(users);
        }

        public ActionResult Add()
        {
            StudentInformationContext db = new StudentInformationContext();

            ViewBag.Faculty = new SelectList(db.Department.Where(x => x.facultyID == 0).ToList(), "id", "name");
            ViewBag.Department = null;

            return View();
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            StudentInformationContext db = new StudentInformationContext();


            if (ModelState.IsValid)
            {
                try
                {
                    user.isActive = true;
                    user.hireDate= DateTime.Now;
                    db.User.Add(user);
                    db.SaveChanges();
                    TempData["Result"] = true;
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            ViewBag.Faculty = new SelectList(db.Department.Where(x => x.facultyID == 0).ToList(), "id", "name");
            ViewBag.Department = null;

            var user = db.User.FirstOrDefault(x => x.id == id);

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    User existingUser = db.User.Find(user.id);

                    if (existingUser != null)
                    {
                        db.Entry(existingUser).CurrentValues.SetValues(user);

                        db.SaveChanges();
                        TempData["Result"] = true;
                    }
                    else
                    {
                        TempData["Result"] = false;
                    }
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var user = db.User.FirstOrDefault(x => x.id == id);

            if (user != null)
            {
                try
                {
                    db.User.Remove(user);
                    db.SaveChanges();
                    TempData["Result"] = true;
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetDepartments(int id)
        {
            using (StudentInformationContext db = new StudentInformationContext())
            {
                var departments = db.Department
                                    .Where(x => x.facultyID == id)
                                    .Select(d => new
                                    {
                                        id = d.id,
                                        name = d.name
                                    })
                                    .ToList();

                return Json(departments, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Authorization()
        {
            StudentInformationContext db = new StudentInformationContext();

            var auths = db.Authorizations.ToList();

            return View(auths);
        }

        public ActionResult AddAuthorization()
        {
            StudentInformationContext db = new StudentInformationContext();

            //User dropdown data
            var userList = db.User.Select(u => new
            {
                id = u.id,
                userDisplay = u.firstName + " " + u.lastName
            })
            .ToList();
            ViewBag.UserList = new SelectList(userList, "id", "userDisplay");
            ViewBag.AuthTypes = new SelectList(db.AuthorizationType.ToList(), "id", "type");

            return View();
        }

        [HttpPost]
        public ActionResult AddAuthorization(Authorizations auth)
        {
            StudentInformationContext db = new StudentInformationContext();


            if (ModelState.IsValid)
            {
                try
                {
                    db.Authorizations.Add(auth);
                    db.SaveChanges();
                    TempData["Result"] = true;
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }
            return RedirectToAction("Authorization");
        }

        public ActionResult RemoveAuthorization(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var auth = db.Authorizations.FirstOrDefault(x => x.id == id);

            if (auth != null)
            {
                try
                {
                    db.Authorizations.Remove(auth);
                    db.SaveChanges();
                    TempData["Result"] = true;
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }

            return RedirectToAction("Authorization");
        }

        public ActionResult AuthorizationType()
        {
            StudentInformationContext db = new StudentInformationContext();

            var authTypes = db.AuthorizationType.ToList();

            return View(authTypes);
        }

        public ActionResult AddAuthorizationType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthorizationType(AuthorizationType authType)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    db.AuthorizationType.Add(authType);
                    db.SaveChanges();
                    TempData["Result"] = true;
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }
            return RedirectToAction("AuthorizationType");
        }

        public ActionResult RemoveAuthorizationType(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var authType = db.AuthorizationType.FirstOrDefault(x => x.id == id);

            if (authType != null)
            {
                try
                {
                    db.AuthorizationType.Remove(authType);
                    db.SaveChanges();
                    TempData["Result"] = true;
                }
                catch (Exception)
                {
                    TempData["Result"] = false;
                }
            }
            else
            {
                TempData["Result"] = false;
            }

            return RedirectToAction("AuthorizationType");
        }
    }
}