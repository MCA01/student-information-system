using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class SemesterController : Controller
    {
        // GET: Semester
        public ActionResult Index()
        {
            StudentInformationContext db = new StudentInformationContext();

            var data = db.Semester.ToList();

            return View(data);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Semester semester)
        {

            StudentInformationContext db = new StudentInformationContext();


            if (ModelState.IsValid)
            {
                try
                {
                    db.Semester.Add(semester);
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

            // Existing semester
            var semester = db.Semester.FirstOrDefault(x => x.id == id);

            return View(semester);
        }

        [HttpPost]
        public ActionResult Edit(Semester semester)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    Enrolls existingSemester = db.Enrolls.Find(semester.id);

                    if (existingSemester != null)
                    {
                        db.Entry(existingSemester).CurrentValues.SetValues(semester);

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

            var semester = db.Semester.FirstOrDefault(x => x.id == id);

            if (semester != null)
            {
                try
                {
                    db.Semester.Remove(semester);
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

    }
}