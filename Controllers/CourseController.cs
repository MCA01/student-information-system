using StundetInformation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {

            StudentInformationContext db = new StudentInformationContext();

            var data = db.Course.ToList();

            LogEvents.LogToFile("Course-IndexPage", "Index fetched");
            return View(data);
        }

        public ActionResult Add()
        {
            StudentInformationContext db = new StudentInformationContext();

            //Instructor dropdown data
            var instructorList = db.User
            .Select(i => new
            {
                id = i.id,
                instructorDisplay = i.firstName + " " + i.lastName + " " + i.departmentID
            })
            .ToList();
            ViewBag.InstructorList = new SelectList(instructorList, "id", "instructorDisplay");

            //Semester dropdown data
            var semesterList = db.Semester.Select(i => new
            {
                id = i.id,
                semesterDisplay = i.name + " " + i.startDate + " " + i.endDate
            }).ToList();
            ViewBag.SemesterList = new SelectList(semesterList, "id", "semesterDisplay");

            //Faculty - Department dropdown data
            ViewBag.Faculty = new SelectList(db.Department.Where(x => x.facultyID == 0).ToList(), "id", "name");
            ViewBag.Department = null;

            LogEvents.LogToFile("Course-AddPage", "Add page fetched");
            return View();
        }


        [HttpPost]
        public ActionResult Add(Course course)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    string courseIdSTR = course.id.ToString();
                    
                    course.isActive = true;
                    db.Course.Add(course);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Course-Add", "Successfully added!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-Add", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-Add", "Cannot Add: Model state is not valid!");
            }
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Instructor dropdown data
            var instructorList = db.User
            .Select(i => new
            {
                id = i.id,
                instructorDisplay = i.firstName + " " + i.lastName + " " + i.departmentID
            })
            .ToList();
            ViewBag.InstructorList = new SelectList(instructorList, "id", "instructorDisplay");

            //Semester dropdown data
            var semesterList = db.Semester.Select(i => new
            {
                id = i.id,
                semesterDisplay = i.name + " " + i.startDate + " " + i.endDate
            }).ToList();
            ViewBag.SemesterList = new SelectList(semesterList, "id", "semesterDisplay");

            //Faculty - Department dropdown data
            ViewBag.Faculty = new SelectList(db.Department.Where(x => x.facultyID == 0).ToList(), "id", "name");
            ViewBag.Department = null;

            // Existing course
            var course = db.Course.FirstOrDefault(x => x.id == id);

            LogEvents.LogToFile("Course-EditPage", "Edit page fetched");
            return View(course);
        }

        [HttpPost]
        public ActionResult Edit(Course course)
        {
            StudentInformationContext db = new StudentInformationContext();

            if (ModelState.IsValid)
            {
                try
                {
                    Course existingCourse= db.Course.Find(course.id);

                    if (existingCourse != null)
                    {
                        db.Entry(existingCourse).CurrentValues.SetValues(course);

                        db.SaveChanges();
                        TempData["Result"] = true;
                        LogEvents.LogToFile("Course-Edit", "Successfully editted!");
                    }
                    else
                    {
                        TempData["Result"] = false;
                        LogEvents.LogToFile("Course-Edit", "Cannot Edit: There is no such course!");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-Edit", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-Edit", "Cannot Edit: Model state is not valid!");
            }
            return RedirectToAction("Index");
        }


        public ActionResult Remove(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var course = db.Course.FirstOrDefault(x => x.id == id);

            if (course != null)
            {
                try
                {
                    db.Course.Remove(course);
                    db.SaveChanges();
                    TempData["Result"] = true;
                    LogEvents.LogToFile("Course-Remove", "Successfully removed!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-Remove", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-Remove", "Cannot Remove: There is no such course!");
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

        public ActionResult InsIndex(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var insCourses = db.Course.Where(x => x.instructorId == id).ToList();

            LogEvents.LogToFile("Course-InsIndexPage", "InsIndex fetched");
            return View(insCourses);
        }

        public ActionResult InsCourseEdit(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            //Semester dropdown data
            var semesterList = db.Semester.Select(i => new
            {
                id = i.id,
                semesterDisplay = i.name + " " + i.startDate + " " + i.endDate
            }).ToList();
            ViewBag.SemesterList = new SelectList(semesterList, "id", "semesterDisplay");

            // Existing course
            var course = db.Course.FirstOrDefault(x => x.instructorId == id);

            LogEvents.LogToFile("Course-InsCoursePage", "InsCoursePage fetched");
            return View(course);
        }

        [HttpPost]
        public ActionResult InsCourseEdit(Course course)
        {
            StudentInformationContext db = new StudentInformationContext();
            Course existingCourse = db.Course.Find(course.id);

            if (ModelState.IsValid)
            {
                try
                {
                    

                    if (existingCourse != null)
                    {
                        course.instructorId = existingCourse.instructorId;
                        course.departmentID = existingCourse.departmentID;
                        course.isActive = true;
                        db.Entry(existingCourse).CurrentValues.SetValues(course);

                        db.SaveChanges();
                        TempData["Result"] = true;
                        LogEvents.LogToFile("Course-InsCourseEdit", "InsCourse successfully editted!");
                    }
                    else
                    {
                        TempData["Result"] = false;
                        LogEvents.LogToFile("Course-InsCourseEdit", "Cannot Edit: There is no such course!");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-InsCourseEdit", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-InsCourseEdit", "Cannot Edit: Model state is not valid!");
            }
            return RedirectToAction("InsIndex", new {id = existingCourse.instructorId});
        }

        public ActionResult InsCourseStudentList(int id)
        {
            StudentInformationContext db = new StudentInformationContext();
          
            var enrollsList = db.Enrolls.Where(x => x.courseID == id).ToList();

            LogEvents.LogToFile("Course-InsCourseStudentListPage", "InsCourseStudentListPage fetched");
            return View(enrollsList);
        }

        public ActionResult InsCourseGradeList(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            var gradesList = db.StudentGrade.Where(x => x.courseID == id).ToList();

            LogEvents.LogToFile("Course-InsCourseGradeListPage", "InsCourseGradeListPage fetched");
            return View(gradesList);
        }

        public ActionResult InsCourseStudentGrades(int courseID, int studentID)
        {
            StudentInformationContext db = new StudentInformationContext();

            var studentGradesList = db.StudentGrade.Where(x => x.courseID == courseID).Where(x => x.studentID == studentID).ToList();

            LogEvents.LogToFile("Course-InsCourseStudentGradesPage", "InsCourseStudentGradesPage fetched");
            return View(studentGradesList);
        }

        public ActionResult InsCourseStudentGradeAdd(int courseID, int studentID)
        {
            StudentInformationContext db = new StudentInformationContext();

            // Create the model
            var model = new StudentGrade();
            model.studentID = studentID;
            model.courseID = courseID;

            LogEvents.LogToFile("Course-InsCourseStudentListPage", "InsCourseStudentListPage fetched");
            return View(model);
        }

        [HttpPost]
        public ActionResult InsCourseStudentGradeAdd(StudentGrade grade)
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
                    LogEvents.LogToFile("Course-InsCourseStudentGradeAdd", "InsCourseStudentGrade successfully added!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-InsCourseStudentGradeAdd", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-InsCourseStudentGradeAdd", "Cannot Add: Model state is not valid!");
            }
            return RedirectToAction("InsCourseStudentGrades", new { courseID = grade.courseID, studentID = grade.studentID });
        }

        public ActionResult InsCourseStudentGradeEdit(int id)
        {
            StudentInformationContext db = new StudentInformationContext();

            // Existing grade
            var grade = db.StudentGrade.FirstOrDefault(x => x.id == id);

            LogEvents.LogToFile("Course-InsCourseStudentGradeEditPage", "InsCourseStudentGradeEditPage fetched");
            return View(grade);
        }

        [HttpPost]
        public ActionResult InsCourseStudentGradeEdit(StudentGrade grade)
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
                        LogEvents.LogToFile("Course-InsCourseStudentGradeEdit", "InsCourseStudentGrade successfully editted!");
                    }
                    else
                    {
                        TempData["Result"] = false;
                        LogEvents.LogToFile("Course-InsCourseStudentGradeEdit", "Cannot Edit: There is no such grade!");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-InsCourseStudentGradeEdit", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-InsCourseStudentGradeEdit", "Cannot Edit: Model state is not valid");

            }
            return RedirectToAction("InsCourseStudentGrades", new { courseID = grade.courseID, studentID = grade.studentID });
        }

        public ActionResult InsCourseStudentGradeRemove(int id)
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
                    LogEvents.LogToFile("Course-InsCourseStudentGradeRemove", "InsCourseStudentGrade successfully removed!");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = false;
                    LogEvents.LogToFile("Course-InsCourseStudentGradeRemove", ex.ToString());
                }
            }
            else
            {
                TempData["Result"] = false;
                LogEvents.LogToFile("Course-InsCourseStudentGradeRemove", "Cannot Remove: There is no such grade!");
            }

            return RedirectToAction("InsCourseStudentGrades", new { courseID = grade.courseID, studentID = grade.studentID });
        }

        public ActionResult StuIndex(int id)
        {
            ViewBag.StudentID = id;

            StudentInformationContext db = new StudentInformationContext();

            var enrolls = db.Enrolls.Where(x => x.studentID == id);
            List<StundetInformation.Models.Course> stuCourses = new List<Course>();
            foreach (Enrolls enroll in enrolls)
            {
                Course course = (Course)db.Course.FirstOrDefault(x => x.id == enroll.courseID);
                stuCourses.Add(course);
            }

            LogEvents.LogToFile("Course-StuIndexPage", "StuIndexPage fetched");
            return View(stuCourses);
        }

        public ActionResult StuCourseGradeList(int courseID, int studentID)
        {
            ViewBag.StudentID = studentID;
            StudentInformationContext db = new StudentInformationContext();

            var gradesList = db.StudentGrade.Where(x => x.courseID == courseID && x.studentID == studentID).ToList();

            LogEvents.LogToFile("Course-StuCourseGradeListPage", "StuCourseGradeListPage fetched");
            return View(gradesList);
        }
    }
}