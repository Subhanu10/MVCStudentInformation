using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dataAccessLayer;
using MVCStudentInformation.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCStudentInformation.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository repo;
        public StudentController(IStudentRepository std)
        {
            repo = std;
        }

        // GET: StudentController
        public ActionResult StudentInformation()
        {
            try
            {


                var student = repo.GetAllStudents();
                return View("GetAllStudent", student);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var student = repo.GetStudentById(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            try
            {

                var stateList = repo.GetStates();
                ViewBag.States = stateList.Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString()
                }).ToList();



                ViewBag.GenderList = repo.GetGender();

                return View("Create", new StudentInformation());
            }
            catch (Exception ex)


            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(StudentInformation details)
        {
            try
            {

                var stateList = repo.GetStates();
                ViewBag.States = stateList.Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString()
                }).ToList();

                ViewBag.GenderList = repo.GetGender();

                if (ModelState.IsValid)
                {


                    if (repo.CheckDuplicate(details.Id, details.Email, details.MobileNumber))
                    {
                        ModelState.AddModelError("", "Email or MobileNumber already exists!");

                        return View("Create", details);
                    }



                    repo.AddStudent(details);
                    TempData["Success"] = "Student added successfully!";

                    return RedirectToAction(nameof(StudentInformation));




                }
                return View("AddStudent", details);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var student = repo.GetStudentById(id);
                if (student == null)
                {
                    return NotFound();
                }
                var stateList = repo.GetStates();
                ViewBag.States = stateList.Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString(),

                }).ToList();

                ViewBag.GenderList = repo.GetGender();


                return View("EditStudent", student);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStudent(StudentInformation model)
        {
            try
            {
                var stateList = repo.GetStates();
                ViewBag.States = stateList.Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString(),

                }).ToList();


                ViewBag.GenderList = repo.GetGender();

                if (ModelState.IsValid)
                {
                    if (repo.CheckDuplicate(model.Id, model.Email, model.MobileNumber))
                    {
                        ModelState.AddModelError("", "Email or MobileNumber already exists!");

                        return View("EditStudent", model);
                    }

                    repo.UpdateStudent(model);
                    TempData["Success"] = "Student updated successfully!";
                    return RedirectToAction(nameof(StudentInformation));
                }
                return View("EditStudent", model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var student = repo.GetStudentById(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View("DeleteStudent", student);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                repo.DeleteStudent(id);
                return RedirectToAction(nameof(StudentInformation));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        //____________________________________________________________________________________________________________________________________________

        // GET: StudentController
        public ActionResult AllStudent()
        {
            try
            {


                var student = repo.GetAllStudents();
                return View("index", student);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // GET: StudentController/Create
        public ActionResult CreateOrEdit(int id = 0)
        {
            try
            {

                var stateList = repo.GetStates();
                ViewBag.States = stateList.Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString()
                }).ToList();
                ViewBag.GenderList = repo.GetGender();


                if (id == 0)
                {
                    return PartialView("_AddOrEdit", new StudentInformation());
                }
                else
                {
                    var student = repo.GetStudentById(id);
                    return PartialView("_AddOrEdit", student);
                }


            }
            catch (Exception ex)


            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(StudentInformation details)
        {
            try
            {

                var stateList = repo.GetStates();
                ViewBag.States = stateList.Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString()
                }).ToList();

                ViewBag.GenderList = repo.GetGender();

                if (!ModelState.IsValid)
                {
                    return PartialView("_AddOrEdit", details);
                }
                if (repo.CheckDuplicate(details.Id, details.Email, details.MobileNumber))
                {
                    ModelState.AddModelError("", "Email or MobileNumber already exists!");

                    return PartialView("_AddOrEdit", details);
                }
                if (details.Id == 0)
                {
                    repo.AddStudent(details);

                    return Json(new { success = true, message = "Student added successfully!" });
                }
                repo.UpdateStudent(details);
                return Json(new { success = true, message = "Student updated successfully!" });
            }

            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(ex.Message));
            }
        }
    }
}
        
                    