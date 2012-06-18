using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{   
    public class DepartmentUserController : Controller
    {
		private readonly IDepartmentRepository departmentRepository;
		private readonly IDepartmentUserRepository departmentuserRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public DepartmentUserController() : this(new DepartmentRepository(), new DepartmentUserRepository())
        {
        }

        public DepartmentUserController(IDepartmentRepository departmentRepository, IDepartmentUserRepository departmentuserRepository)
        {
			this.departmentRepository = departmentRepository;
			this.departmentuserRepository = departmentuserRepository;
        }

        //
        // GET: /DepartmentUser/

        public ViewResult Index()
        {
            return View(departmentuserRepository.AllIncluding(departmentuser => departmentuser.Department));
        }

        //
        // GET: /DepartmentUser/Details/5

        public ViewResult Details(int id)
        {
            return View(departmentuserRepository.Find(id));
        }

        //
        // GET: /DepartmentUser/Create

        public ActionResult Create()
        {
			ViewBag.PossibleDepartment = departmentRepository.All;
            return View();
        } 

        //
        // POST: /DepartmentUser/Create

        [HttpPost]
        public ActionResult Create(DepartmentUser departmentuser)
        {
            if (ModelState.IsValid) {
                departmentuserRepository.InsertOrUpdate(departmentuser);
                departmentuserRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleDepartment = departmentRepository.All;
				return View();
			}
        }
        
        //
        // GET: /DepartmentUser/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossibleDepartment = departmentRepository.All;
             return View(departmentuserRepository.Find(id));
        }

        //
        // POST: /DepartmentUser/Edit/5

        [HttpPost]
        public ActionResult Edit(DepartmentUser departmentuser)
        {
            if (ModelState.IsValid) {
                departmentuserRepository.InsertOrUpdate(departmentuser);
                departmentuserRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleDepartment = departmentRepository.All;
				return View();
			}
        }

        //
        // GET: /DepartmentUser/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(departmentuserRepository.Find(id));
        }

        //
        // POST: /DepartmentUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            departmentuserRepository.Delete(id);
            departmentuserRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                departmentRepository.Dispose();
                departmentuserRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

