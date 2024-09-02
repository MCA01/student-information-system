using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            StudentInformationContext db = new StudentInformationContext();
            var data = db.Student.ToList();
            
            return View(data);
        }

        public ActionResult Add()
        {
            StudentInformationContext db = new StudentInformationContext();

            ViewBag.Faculty = new SelectList(db.Department.Where(x => x.facultyID == 0).ToList(), "id", "name");
            ViewBag.Department = null;

            return View();
        }

        [HttpPost]
        public ActionResult Add(Student student)
        {
            StudentInformationContext db = new StudentInformationContext();


            if (ModelState.IsValid)
            {
                try
                {
                    student.isActive = true;
                    student.registrationStatus = "Active";
                    student.registrationDate = DateTime.Now;
                    db.Student.Add(student);
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

        public ActionResult Remove(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var student = db.Student.FirstOrDefault(x => x.id == id);

            if (student != null)
            {
                try
                {
                    db.Student.Remove(student);
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            ViewBag.Faculty = new SelectList(db.Department.Where(x => x.facultyID == 0).ToList(), "id", "name");
            ViewBag.Department = null;

            var student = db.Student.FirstOrDefault(x => x.id == id);

            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    Student existingStudent = db.Student.Find(student.id);

                    if (existingStudent != null)
                    {
                        student.isActive = true;
                        db.Entry(existingStudent).CurrentValues.SetValues(student);
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
    }
}
