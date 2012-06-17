using System;
using System.Linq;
using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{
    [Authorize]
    public class KeyDataController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IKeyDataRepository keydataRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public KeyDataController() : this(new KeyDataRepository(), new CategoryRepository())
        {
        }

        public KeyDataController(IKeyDataRepository keydataRepository, ICategoryRepository categoryRepository)
        {
            this.keydataRepository = keydataRepository;
            this.categoryRepository = categoryRepository;
        }

        //
        // GET: /KeyData/

        public ViewResult Index()
        {
            return View(keydataRepository.AllIncluding(c => c.Category, c => c.Category.Unit, c => c.Category.Cycle));
        }

        //
        // GET: /KeyData/Details/5

        public ViewResult Details(int id)
        {
            return View(keydataRepository.Find(id));
        }

        //
        // GET: /KeyData/Create

        public ActionResult Create(int categoryId)
        {
            Category category = categoryRepository.AllIncluding(c => c.Cycle).Where(c => c.Id == categoryId).Single();
            var keyData = new KeyData {Category = category, CategoryId = categoryId};
            return View(keyData);
        }

        //
        // POST: /KeyData/Create

        [HttpPost]
        public ActionResult Create(KeyData keydata)
        {
            if (ModelState.IsValid)
            {
                keydata.Year = DateTime.Now.Year;
                keydata.CreateDate = DateTime.Now;
                keydataRepository.InsertOrUpdate(keydata);
                keydataRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
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
            if (ModelState.IsValid)
            {
                keydataRepository.InsertOrUpdate(keydata);
                keydataRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
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
            if (disposing)
            {
                keydataRepository.Dispose();
                categoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}