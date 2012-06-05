using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{   
    public class DepartmentController : Controller
    {
		private readonly IDepartmentRepository departmentRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public DepartmentController() : this(new DepartmentRepository())
        {
        }

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
			this.departmentRepository = departmentRepository;
        }

        //
        // GET: /Department/

        public ViewResult Index()
        {
            return View(departmentRepository.AllIncluding(department => department.Categories));
        }

        //
        // GET: /Department/Details/5

        public ViewResult Details(int id)
        {
            return View(departmentRepository.Find(id));
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid) {
                departmentRepository.InsertOrUpdate(department);
                departmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Department/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(departmentRepository.Find(id));
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid) {
                departmentRepository.InsertOrUpdate(department);
                departmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Department/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(departmentRepository.Find(id));
        }

        //
        // POST: /Department/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            departmentRepository.Delete(id);
            departmentRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                departmentRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

