using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{   
    public class KeyDataController : Controller
    {
		private readonly IKeyDataRepository keydataRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public KeyDataController() : this(new KeyDataRepository())
        {
        }

        public KeyDataController(IKeyDataRepository keydataRepository)
        {
			this.keydataRepository = keydataRepository;
        }

        //
        // GET: /KeyData/

        public ViewResult Index()
        {
            return View(keydataRepository.All);
        }

        //
        // GET: /KeyData/Details/5

        public ViewResult Details(int id)
        {
            return View(keydataRepository.Find(id));
        }

        //
        // GET: /KeyData/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /KeyData/Create

        [HttpPost]
        public ActionResult Create(KeyData keydata)
        {
            if (ModelState.IsValid) {
                keydataRepository.InsertOrUpdate(keydata);
                keydataRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /KeyData/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(keydataRepository.Find(id));
        }

        //
        // POST: /KeyData/Edit/5

        [HttpPost]
        public ActionResult Edit(KeyData keydata)
        {
            if (ModelState.IsValid) {
                keydataRepository.InsertOrUpdate(keydata);
                keydataRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /KeyData/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(keydataRepository.Find(id));
        }

        //
        // POST: /KeyData/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            keydataRepository.Delete(id);
            keydataRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                keydataRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

