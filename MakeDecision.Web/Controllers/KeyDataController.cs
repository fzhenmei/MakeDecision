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
            Category category =
                categoryRepository.AllIncluding(c => c.Unit, c => c.Cycle).SingleOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryId = categoryId;
            ViewBag.UnitLabel = category.Unit.UnitLabel;
            ViewBag.UnitName = category.Unit.UnitName;
            ViewBag.CycleId = category.Cycle.Id;
            ViewBag.CycleName = category.Cycle.CycleName;

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
            KeyData keyData = InitKeyData(categoryId);
            PopulateSelectList(ViewBag.CycleId);

            return View(keyData);
        }

        private KeyData InitKeyData(int categoryId, KeyData keyData = null)
        {
            Category category = categoryRepository.AllIncluding(c => c.Cycle).Single(c => c.Id == categoryId);
            if (keyData == null)
            {
                keyData = new KeyData { Year = DateTime.Now.Year, Category = category, CategoryId = categoryId };
            }

            ViewBag.CategoryId = categoryId;
            ViewBag.CycleId = category.Cycle.Id;

            return keyData;
        }

        //
        // POST: /KeyData/Create

        [HttpPost]
        public ActionResult Create(KeyData keydata, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                keydata.CreateDate = DateTime.Now;
                keydataRepository.InsertOrUpdate(keydata);
                GetFile(keydata, file);
                keydataRepository.Save();

                return RedirectToAction("Index", new {categoryId = keydata.CategoryId});
            }

            if (keydata.CategoryId <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(InitKeyData(keydata.CategoryId));
        }

        private void GetFile(KeyData keydata, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string ext = Path.GetExtension(file.FileName);
                string fileName = Guid.NewGuid().ToString("N") + ext;
                string path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                keydata.FilePath = fileName;

                file.SaveAs(path);
            }
        }

        //
        // GET: /KeyData/Edit/5

        public ActionResult Edit(int id)
        {
            var keyData = keydataRepository.AllIncluding(k => k.Category).SingleOrDefault(k => k.Id == id);
            if (keyData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            InitKeyData(keyData.CategoryId, keyData);
            PopulateSelectList(keyData.Category.CycleId, keyData.CycleValue);

            return View(keyData);
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

                return RedirectToAction("Index", new {categoryId = keydata.CategoryId});
            }

            if (keydata.CategoryId <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var initedKeyData = InitKeyData(keydata.CategoryId, keydata);
            PopulateSelectList(ViewBag.CycleId, keydata.CycleValue);

            return View(initedKeyData);
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
            KeyData keydata = keydataRepository.Find(id);
            if (keydata == null)
            {
                return RedirectToAction("Index", "Home");
            }

            keydataRepository.Delete(id);
            keydataRepository.Save();

            return RedirectToAction("Index", new {categoryId = keydata.CategoryId});
        }

        private void PopulateSelectList(int cycleId, object selectedValue = null)
        {
            switch (cycleId)
            {
                case 2:
                    ViewData["CycleValue"] =
                        new SelectList(
                            new[] {"1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"},
                            selectedValue);
                    break;
                case 5:
                    ViewData["CycleValue"] =
                        new SelectList(
                            new[] {"1季度", "2季度", "3季度", "4季度"},
                            selectedValue);
                    break;
                case 6:
                    ViewData["CycleValue"] =
                        new SelectList(
                            new [] {"上半年", "下半年"}, selectedValue);
                    break;
            }
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