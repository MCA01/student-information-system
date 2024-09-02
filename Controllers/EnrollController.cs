using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class EnrollController : Controller
    {
        // GET: Enroll
        public ActionResult Index()
        {
            StudentInformationContext db = new StudentInformationContext();

            var data = db.Enrolls.ToList();

            LogEvents.LogToFile("Enroll-IndexPage", "Index fetched");
            return View(data);
        }

        public ActionResult Add()
        {

            StudentInformationContext db = new StudentInformationContext();

            //Course dropdown data
            var courseList = db.Course
            .Select(c => new
            {
                id = c.id,
                courseDisplay = c.courseName + " " + c.courseCode + " " + c.Department.name
            })
            .ToList();
            ViewBag.CourseList = new SelectList(courseList, "id", "courseDisplay");

            LogEvents.LogToFile("Enroll-AddPage", "AddPage fetched");
            return View();
        }

        [HttpPost]
        public ActionResult Add(Enrolls enrollment)
        {

            StudentInformationContext db = new StudentInformationContext();


            if (ModelState.IsValid)
            {
                try
                {
                    enrollment.status = "Enrolled";
                    db.Enrolls.Add(enrollment);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Department-IndexPage", "Index fetched");
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

            //Course dropdown data
            var courseList = db.Course
            .Select(c => new
            {
                id = c.id,
                courseDisplay = c.courseName + " " + c.courseCode + " " + c.Department.name
            })
            .ToList();
            ViewBag.CourseList = new SelectList(courseList, "id", "courseDisplay");

            // Existing enrollment
            var enrollment = db.Enrolls.FirstOrDefault(x => x.id == id);

            return View(enrollment);
        }

        [HttpPost]
        public ActionResult Edit(Enrolls enrollment)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    Enrolls existingEnrollment = db.Enrolls.Find(enrollment.id);

                    if (existingEnrollment != null)
                    {
                        db.Entry(existingEnrollment).CurrentValues.SetValues(enrollment);

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

            var enrollment = db.Enrolls.FirstOrDefault(x => x.id == id);

            if (enrollment != null)
            {
                try
                {
                    db.Enrolls.Remove(enrollment);
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