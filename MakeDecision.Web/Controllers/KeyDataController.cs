using System;
using System.IO;
using System.Linq;
using System.Web;
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

        public ActionResult Index(int categoryId)
        {
            var category = categoryRepository.AllIncluding(c => c.Unit).SingleOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryId = categoryId;
            ViewBag.UnitLabel = category.Unit.UnitLabel;
            ViewBag.UnitName = category.Unit.UnitName;

            return
                View(
                    keydataRepository.AllIncluding(c => c.Category, c => c.Category.Unit, c => c.Category.Cycle).Where(
                        c => c.CategoryId == categoryId));
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
            ViewBag.CategoryId = categoryId;
            return View(GetCategory(categoryId));
        }

        private KeyData GetCategory(int categoryId)
        {
            Category category = categoryRepository.AllIncluding(c => c.Cycle).Where(c => c.Id == categoryId).Single();
            var keyData = new KeyData {Category = category, CategoryId = categoryId};
            return keyData;
        }

        //
        // POST: /KeyData/Create

        [HttpPost]
        public ActionResult Create(KeyData keydata, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                keydata.Year = DateTime.Now.Year;
                keydata.CreateDate = DateTime.Now;
                keydataRepository.InsertOrUpdate(keydata);
                GetFile(keydata, file);
                keydataRepository.Save();

                return RedirectToAction("Index", new { categoryId = keydata.CategoryId });
            }

            if (keydata.CategoryId <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(GetCategory(keydata.CategoryId));
        }

        private void GetFile(KeyData keydata, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var ext = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString("N") + ext;
                var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                keydata.FilePath = fileName;

                file.SaveAs(path);
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
        public ActionResult Edit(KeyData keydata, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                keydata.CreateDate = DateTime.Now;
                GetFile(keydata, file);
                keydataRepository.InsertOrUpdate(keydata);
                keydataRepository.Save();

                return RedirectToAction("Index", new { categoryId = keydata.CategoryId });
            }

            if (keydata.CategoryId <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(GetCategory(keydata.CategoryId));
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
            var keydata = keydataRepository.Find(id);
            if (keydata == null)
            {
                return RedirectToAction("Index", "Home");
            }

            keydataRepository.Delete(id);
            keydataRepository.Save();

            return RedirectToAction("Index", new { categoryId = keydata.CategoryId});
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