using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class StudentGradeController : Controller
    {
        // GET: StudentGrade
        public ActionResult Index()
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


            //Student dropdown data
            var studentList = db.Student.Select(s => new
            {
                id = s.id,
                studentDisplay = s.firstName + " " + s.lastName
            })
            .ToList();
            ViewBag.StudentList = new SelectList(studentList, "id", "studentDisplay");

            return View();
        }

        public ActionResult ListByCourse(int courseID)
        {
            StudentInformationContext db = new StudentInformationContext();

            var studentsGradesList = db.StudentGrade.Where(x => x.courseID == courseID).ToList();

            ViewBag.CourseID = courseID;

            return View(studentsGradesList);
        }

        public ActionResult ListByStudent(int studentID)
        {
            StudentInformationContext db = new StudentInformationContext();

            var coursesGradesList = db.StudentGrade.Where(x => x.studentID == studentID).ToList();

            ViewBag.StudentID = studentID;

            return View(coursesGradesList);
        }

        public ActionResult AddGradeToCourse(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Student dropdown data
            var studentList = db.Student.Select(s => new
            {
                id = s.id,
                studentDisplay = s.firstName + " " + s.lastName
            })
            .ToList();

            // Create the model
            var model = new StudentGrade();
            model.courseID = id;
            ViewBag.StudentList = new SelectList(studentList, "id", "studentDisplay");

            return View(model);
        }


        [HttpPost]
        public ActionResult AddGradeToCourse(StudentGrade grade)
        {

            StudentInformationContext db = new StudentInformationContext();


            if (ModelState.IsValid)
            {
                try
                {
                    grade.date = DateTime.Today;
                    db.StudentGrade.Add(grade);
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
            return RedirectToAction("ListByCourse", new {grade.courseID});
        }

        public ActionResult AddGradeToStudent(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Course dropdown data
            var courseList = db.Course.Select(c => new
            {
                id = c.id,
                courseDisplay = c.courseCode + " " + c.courseName
            })
            .ToList();


            // Create the model
            var model = new StudentGrade();
            model.studentID = id;
            ViewBag.CourseList = new SelectList(courseList, "id", "courseDisplay");

            return View(model);
        }

        [HttpPost]
        public ActionResult AddGradeToStudent(StudentGrade grade)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    grade.date = DateTime.Today;
                    db.StudentGrade.Add(grade);
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
            return RedirectToAction("ListByStudent", new { grade.studentID });
        }

        public ActionResult EditGradeFromCourse(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Student dropdown data
            var studentList = db.Student.Select(s => new
            {
                id = s.id,
                studentDisplay = s.firstName + " " + s.lastName
            })
            .ToList();
            ViewBag.StudentList = new SelectList(studentList, "id", "studentDisplay");

            // Existing grade
            var grade = db.StudentGrade.FirstOrDefault(x => x.id == id);

            return View(grade);
        }

        [HttpPost]
        public ActionResult EditGradeFromCourse(StudentGrade grade)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    StudentGrade existingGrade = db.StudentGrade.Find(grade.id);

                    if (existingGrade != null)
                    {
                        grade.date = DateTime.Today;
                        db.Entry(existingGrade).CurrentValues.SetValues(grade);
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
            return RedirectToAction("ListByCourse", new {courseID = grade.courseID});
        }

        public ActionResult EditGradeFromStudent(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Course dropdown data
            var courseList = db.Course.Select(c => new
            {
                id = c.id,
                courseDisplay = c.courseCode + " " + c.courseName
            })
            .ToList();
            ViewBag.CourseList = new SelectList(courseList, "id", "courseDisplay");

            // Existing grade
            var grade = db.StudentGrade.FirstOrDefault(x => x.id == id);

            return View(grade);
        }

        [HttpPost]
        public ActionResult EditGradeFromStudent(StudentGrade grade)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    StudentGrade existingGrade = db.StudentGrade.Find(grade.id);

                    if (existingGrade != null)
                    {
                        grade.date = DateTime.Today;
                        db.Entry(existingGrade).CurrentValues.SetValues(grade);
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
            return RedirectToAction("ListByStudent", new { studentID = grade.studentID });
        }

        public ActionResult RemoveGradeFromCourse(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var grade = db.StudentGrade.FirstOrDefault(x => x.id == id);

            if (grade != null)
            {
                try
                {
                    db.StudentGrade.Remove(grade);
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

            return RedirectToAction("ListByCourse", new { courseID = grade.courseID });
        }

        public ActionResult RemoveGradeFromStudent(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var grade = db.StudentGrade.FirstOrDefault(x => x.id == id);

            if (grade != null)
            {
                try
                {
                    db.StudentGrade.Remove(grade);
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

            return RedirectToAction("ListByStudent", new { studentID = grade.studentID });
        }
    }
}