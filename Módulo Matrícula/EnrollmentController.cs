using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers {
    public class EnrollmentController : Controller {
        private SchoolContext db = new SchoolContext();
        private int pageSize = 5;

        public EnrollmentController() { }

        // For testing.
        public EnrollmentController(SchoolContext context) {
            db = context;
        }

        [Authorize]
        // GET: Matricula
        public ActionResult Index(int? page, bool? dropFailed = false) {
            int currentUserID = GetCurrentUserID();
            StudentCourses studentCurrentCourses = new StudentCourses();

            studentCurrentCourses.CurrentPeriod = GetCurrentPeriod();
            studentCurrentCourses.CurrentEnrollments = GetCurrentEnrollments(currentUserID).ToPagedList(page ?? 1, pageSize);

            if (dropFailed == true) {
                ViewBag.ErrorMessage = "Course drop failed! You are not allowed to drop this class.";
            } else {
                ViewBag.ErrorMessage = null;
            }

            return View("Index", studentCurrentCourses);
        }

        [Authorize]
        public ActionResult Enrollment(int? availableCoursesPage, int? currentCoursesPage, bool? enrollFailed = false) {
            IEnumerable<Course> availableCourses, currentCourses;
            
            int currentUserID = GetCurrentUserID();
            StudentCourses studentCoursesData = new StudentCourses();

            availableCourses = GetAvailableCourses(currentUserID);
            currentCourses = GetCurrentCourses(currentUserID);

            studentCoursesData.AvailableCourses = availableCourses.ToPagedList(availableCoursesPage ?? 1, pageSize);
            studentCoursesData.EnrolledCourses = currentCourses.ToPagedList(currentCoursesPage ?? 1, pageSize);

            if (enrollFailed == true) {
                ViewBag.ErrorMessage = "Enrollment failed! You are not allowed to enroll this class.";
            } else {
                ViewBag.ErrorMessage = null;
            }

            return View("Enrollment", studentCoursesData);
        }

        [Authorize]
        public ActionResult EnrollCourse(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Find(id);

            if (course == null) {
                return HttpNotFound();
            }

            return PartialView("_ConfirmEnroll", course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EnrollCourseConfirmed(int id, bool? willFail = false) {
            try {
                if (willFail == false) {
                    Course courseToEnroll = db.Courses.Find(id);

                    if (courseToEnroll != null) {
                        Enrollment studentEnrollment = new Enrollment() { StudentID = GetCurrentUserID(), CourseID = courseToEnroll.CourseID, Period = GetCurrentPeriod() };
                        db.Enrollments.Add(studentEnrollment);
                        db.SaveChanges();
                    }
                } else {
                    return RedirectToAction("Enrollment", new { enrollFailed = true }); 
                }
            } catch (RetryLimitExceededException/* dex */) {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Enrollment", new { enrollFailed = true });
            }

            return RedirectToAction("Enrollment");
        }

        [Authorize]
        public ActionResult DropCourse(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Enrollment enrollment = db.Enrollments.Find(id);

            if (enrollment == null) {
                return HttpNotFound();
            }

            return PartialView("_ConfirmDrop", enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DropCourseConfirmed(int id, bool? willFail = false) {
            try {
                if (willFail == false) {
                    Enrollment enrollToDrop = db.Enrollments.Find(id);

                    if (enrollToDrop != null) {
                        db.Enrollments.Remove(enrollToDrop);
                        db.SaveChanges();
                    }
                } else {
                    return RedirectToAction("Index", new { dropFailed = true });
                }
            } catch (RetryLimitExceededException/* dex */) {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Index", new { dropFailed = true });
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AcademicHistory(int? page) {
            int studentID, pageNumber;
            IEnumerable<Enrollment> academicHistory;

            studentID = GetCurrentUserID();
            academicHistory = GetAcademicHistory(studentID);

            pageNumber = (page ?? 1);
            return View("AcademicHistory", academicHistory.ToPagedList(pageNumber, pageSize));
        }

        #region Helpers

        private int GetCurrentUserID() {
            return int.Parse(User.Identity.Name);
        }

        public IEnumerable<Course> GetCurrentCourses(int studentID) {
            string currentPeriod;

            currentPeriod = GetCurrentPeriod();

            var currentCourses = from c in db.Courses
                                 join e in db.Enrollments on c.CourseID equals e.CourseID
                                 where e.StudentID == studentID
                                 && e.Period == currentPeriod
                                 orderby c.CourseID
                                 select c;

            return currentCourses;
        }

        public IEnumerable<Enrollment> GetCurrentEnrollments(int studentID) {
            string currentPeriod;

            currentPeriod = GetCurrentPeriod();

            var currentCourses = from e in db.Enrollments
                                 where e.StudentID == studentID
                                 && e.Period == currentPeriod
                                 orderby e.EnrollmentID
                                 select e;

            return currentCourses;
        }

        public IEnumerable<Enrollment> GetAcademicHistory(int studentID) {
            var academicHistory = from e in db.Enrollments
                                  orderby e.EnrollmentID
                                  where e.StudentID == studentID
                                  select e;

            return academicHistory;
        }

        public IEnumerable<Course> GetAvailableCourses(int studentID) {
            IEnumerable<Course> currentCourses = GetCurrentCourses(studentID);
            
            var availableCourses = from c in db.Courses
                                   where !(currentCourses).Contains(c)
                                   orderby c.CourseID
                                   select c;

            return availableCourses;
        }

        public string GetCurrentPeriod() {
            string currentPeriod = "";
            int currentMonth, currentYear;
            DateTime currentDate;

            currentDate = System.DateTime.Now;
            currentMonth = currentDate.Month;
            currentYear = currentDate.Year;

            if (currentMonth < 8) {
                currentPeriod = "I";
            } else {
                currentPeriod = "II";
            }

            currentPeriod += "-" + currentYear.ToString();

            return currentPeriod;
        }

        #endregion
    }
}