using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department

        public ActionResult Index()
        {
            StudentInformationContext db = new StudentInformationContext();

            var data = db.Department.ToList();

            LogEvents.LogToFile("Department-IndexPage", "Index fetched");
            return View(data);
        }

        public ActionResult DepartmentIndex(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            // Getting Departments where faculty id matchs with the argument faculty id
            var departments = db.Department.Where(x => x.facultyID == id).ToList();

            LogEvents.LogToFile("Department-DepartmentIndexPage", "DepartmentIndex fetched");
            return View(departments);
        }

        public ActionResult AddFaculty()
        {
            LogEvents.LogToFile("Department-AddFacultyPage", "AddFacultyPage fetched");
            return View();
        }

        [HttpPost]
        public ActionResult AddFaculty(Department department)
        {

            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    department.facultyID = 0;
                    department.isActive = true;
                    db.Department.Add(department);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Department-AddFaculty", "Faculty successfully added!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Department-AddFaculty", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Department-AddFaculty", "Cannot Add: Model state is not valid!");
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddDepartment(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            LogEvents.LogToFile("Department-AddDepartmentPage", "AddDepartmentPage fetched");
            return View();
        }

        [HttpPost]
        public ActionResult AddDepartment(Department department)
        {

            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    department.id = 0;
                    department.isActive = true;
                    db.Department.Add(department);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Department-AddDepartment", "Department successfully added!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Department-AddDepartment", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Department-AddDepartment", "Cannot Add: Model state is not valid!");
            }
            return RedirectToAction("DepartmentIndex", new { id = department.facultyID });
        }

        public ActionResult EditFaculty(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            // Existing department
            var department = db.Department.FirstOrDefault(x => x.id == id);

            LogEvents.LogToFile("Department-EditFacultyPage", "EditFacultyPage fetched");
            return View(department);
        }

        [HttpPost]
        public ActionResult EditFaculty(Department faculty)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    Department existingFaculty = db.Department.Find(faculty.id);

                    if (existingFaculty != null)
                    {
                        existingFaculty.name = faculty.name;
                        db.Entry(existingFaculty);
                        db.SaveChanges();
                        TempData["Result"] = true;
                        LogEvents.LogToFile("Department-EditFaculty", "Faculty successfully editted!");
                    }
                    else
                    {
                        TempData["Result"] = false;
                        LogEvents.LogToFile("Department-EditFaculty", "Faculty successfully editted!");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Department-EditFaculty", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Department-EditFaculty", "Cannot Edit: Model state is not valid!");
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditDepartment(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Faculty dropdown data
            var facultyList = db.Department.Where(x => x.facultyID == 0)
            .Select(f => new
            {
                id = f.facultyID,
                facultyDisplay = f.name
            })
            .ToList();
            ViewBag.FacultyList = new SelectList(facultyList, "id", "facultyDisplay", id);

            // Existing department
            var department = db.Department.FirstOrDefault(x => x.id == id);

            LogEvents.LogToFile("Department-EditDepartmentPage", "EditDepartmentPage fetched");
            return View(department);
        }

        [HttpPost]
        public ActionResult EditDepartment(Department department)
        {
            StudentInformationContext db = new StudentInformationContext();

            Department existingDepartment = db.Department.Find(department.id);

            if (ModelState.IsValid)
            {
                try
                {

                    if (existingDepartment != null)
                    {
                        existingDepartment.name = department.name;
                        db.Entry(existingDepartment);
                        db.SaveChanges();
                        TempData["Result"] = true;
                        LogEvents.LogToFile("Department-EditDepartment", "Department successfully editted!");
                    }
                    else
                    {
                        TempData["Result"] = false;
                        LogEvents.LogToFile("Department-EditDepartment", "Cannot Edit: There is no such department!");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Department-EditDepartment", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Department-EditDepartment", "Cannot Edit: Model state is not valid!");
            }
            return RedirectToAction("DepartmentIndex", new { id = existingDepartment.facultyID });
        }


        public ActionResult RemoveFaculty(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var faculty = db.Department.FirstOrDefault(x => x.id == id);

            if (faculty != null)
            {
                try
                {
                    db.Department.Remove(faculty);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Department-RemoveFaculty", "Faculty successfully removed!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Department-RemoveFaculty", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Department-RemoveFaculty", "Cannot Remove: There is no such faculty!");
            }

            return RedirectToAction("Index");

        }


        public ActionResult RemoveDepartment(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var department = db.Department.FirstOrDefault(x => x.id == id);

            if (department != null)
            {
                try
                {
                    db.Department.Remove(department);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Department-RemoveDepartment", "Departmetn successfully removed!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Department-RemoveDepartment", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Department-RemoveDepartment", "Cannot Remove: There is no such department!");
            }

            return RedirectToAction("DepartmentIndex", new { id = department.facultyID });

        }
    }
}