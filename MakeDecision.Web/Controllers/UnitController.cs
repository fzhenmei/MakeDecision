using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{   
    public class UnitController : Controller
    {
		private readonly IUnitRepository unitRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public UnitController() : this(new UnitRepository())
        {
        }

        public UnitController(IUnitRepository unitRepository)
        {
			this.unitRepository = unitRepository;
        }

        //
        // GET: /Unit/

        public ViewResult Index()
        {
            return View(unitRepository.All);
        }

        //
        // GET: /Unit/Details/5

        public ViewResult Details(int id)
        {
            return View(unitRepository.Find(id));
        }

        //
        // GET: /Unit/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Unit/Create

        [HttpPost]
        public ActionResult Create(Unit unit)
        {
            if (ModelState.IsValid) {
                unitRepository.InsertOrUpdate(unit);
                unitRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Unit/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(unitRepository.Find(id));
        }

        //
        // POST: /Unit/Edit/5

        [HttpPost]
        public ActionResult Edit(Unit unit)
        {
            if (ModelState.IsValid) {
                unitRepository.InsertOrUpdate(unit);
                unitRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Unit/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(unitRepository.Find(id));
        }

        //
        // POST: /Unit/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            unitRepository.Delete(id);
            unitRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                unitRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

