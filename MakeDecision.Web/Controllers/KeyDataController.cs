using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakeDecision.Web.Models;
using NLog;

namespace MakeDecision.Web.Controllers
{
    [Authorize]
    public class KeyDataController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IKeyDataRepository keydataRepository;
        private static Logger log = LogManager.GetCurrentClassLogger();

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

        public ActionResult Index(int categoryId, int? page)
        {
            Category category =
                categoryRepository.AllIncluding(c => c.Unit, c => c.Cycle).SingleOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (page == null || page <= 0)
            {
                ViewBag.CurrentPage = 1;
            }
            else
            {
                ViewBag.CurrentPage = page;
            }

            ViewBag.PageSize = 12;

            var keyDatas = keydataRepository.All
                    .Where(c => c.CategoryId == categoryId);

            ViewBag.TotalCount = keyDatas.Count();

            category.KeyDatas = keyDatas.OrderByDescending(c => c.Id)
                    .Skip(((int) ViewBag.CurrentPage - 1)*(int) ViewBag.PageSize).Take((int) ViewBag.PageSize).ToList();

            return View(category);
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
                keyData = new KeyData {Year = DateTime.Now.Year, Category = category, CategoryId = categoryId};
            }

            ViewBag.CategoryId = categoryId;
            ViewBag.CycleId = category.Cycle.Id;

            return keyData;
        }

        //
        // POST: /KeyData/Create

        [HttpPost]
        public ActionResult Create(KeyData keyData, HttpPostedFileBase file)
        {
            if (keyData.CycleValue == null)
            {
                keyData.CycleValue = keyData.Year.ToString(CultureInfo.InvariantCulture);
            }

            if (
                keydataRepository.All.Any(
                    k =>
                    k.CategoryId == keyData.CategoryId && k.Year == keyData.Year && k.CycleValue == keyData.CycleValue))
            {
                ModelState.AddModelError("ShouldBeOnlyOne",
                                         string.Format("无法保存数据：{0}年，周期为{1}的数据已经存在。", keyData.Year, keyData.CycleValue));
            }

            if (ModelState.IsValid)
            {
                keyData.CreateDate = DateTime.Now;
                GetFile(keyData, file);

                keydataRepository.InsertOrUpdate(keyData);
                keydataRepository.Save();

                return RedirectToAction("Index", new {categoryId = keyData.CategoryId});
            }

            if (keyData.CategoryId <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            Category category = categoryRepository.AllIncluding(c => c.Cycle).Single(c => c.Id == keyData.CategoryId);
            if (category == null)
            {
                return RedirectToAction("Index", "Home");
            }

            PopulateSelectList(category.CycleId);

            return View(InitKeyData(keyData.CategoryId));
        }

        private void GetFile(KeyData keydata, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var category = categoryRepository.Find(keydata.CategoryId);
                var filePath = new FilePathManager().GetFilePath(category.CategoryName);
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    log.Warn("File path is empty.");
                    filePath = Server.MapPath("~/Content/Uploads");
                }
                string path = Path.Combine(filePath, file.FileName);
                keydata.FilePath = path;

                file.SaveAs(path);
                log.Info("A file store in {0}", path);
            }
            else
            {
                log.Debug("no file.");
            }
        }

        //
        // GET: /KeyData/Edit/5

        public ActionResult Edit(int id)
        {
            KeyData keyData = keydataRepository.AllIncluding(k => k.Category).SingleOrDefault(k => k.Id == id);
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
        public ActionResult Edit(KeyData keyData, HttpPostedFileBase file)
        {
            ;
            if (keydataRepository.All.Any(
                k =>
                k.CategoryId == keyData.CategoryId && k.Year == keyData.Year && k.CycleValue == keyData.CycleValue &&
                k.Id != keyData.Id))
            {
                ModelState.AddModelError("ShouldBeOnlyOne",
                                         string.Format("无法保存数据：{0}年，周期为{1}的数据已经存在。", keyData.Year, keyData.CycleValue));
            }


            if (ModelState.IsValid)
            {
                if (keyData.CycleValue == null)
                {
                    keyData.CycleValue = keyData.Year.ToString();
                }

                keyData.CreateDate = DateTime.Now;
                GetFile(keyData, file);

                keydataRepository.InsertOrUpdate(keyData);
                keydataRepository.Save();

                return RedirectToAction("Index", new {categoryId = keyData.CategoryId});
            }

            if (keyData.CategoryId <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            KeyData initedKeyData = InitKeyData(keyData.CategoryId, keyData);
            PopulateSelectList(ViewBag.CycleId, keyData.CycleValue);

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
                            new[] {"上半年", "下半年"}, selectedValue);
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