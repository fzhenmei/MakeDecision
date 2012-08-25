using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICycleRepository cycleRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IUnitRepository unitRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public CategoryController()
            : this(new CategoryRepository(), new DepartmentRepository(), new CycleRepository(), new UnitRepository())
        {
        }

        public CategoryController(ICategoryRepository categoryRepository, IDepartmentRepository departmentRepository,
                                  ICycleRepository cycleRepository, IUnitRepository unitRepository)
        {
            this.categoryRepository = categoryRepository;
            this.departmentRepository = departmentRepository;
            this.cycleRepository = cycleRepository;
            this.unitRepository = unitRepository;
        }

        //
        // GET: /Category/

        public ViewResult Index(int departmentId)
        {
            ViewBag.DepartmentId = departmentId;
            return
                View(
                    categoryRepository.AllIncluding(c => c.Department, c => c.Cycle, c => c.Unit).Where(
                        c => c.DepartmentId == departmentId));
        }

        //
        // GET: /Category/Details/5

        public ViewResult Details(int id)
        {
            return View(categoryRepository.Find(id));
        }

        //
        // GET: /Category/Create

        public ActionResult Create(int departmentId)
        {
            Category category = LoadCategory(departmentId);
            return View(category);
        }

        private Category LoadCategory(int departmentId)
        {
            Department dept = departmentRepository.Find(departmentId);
            var category = new Category {DepartmentId = dept.Id, Department = dept};
            PopulateDropDownList();
            return category;
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.InsertOrUpdate(category);
                categoryRepository.Save();
                return RedirectToAction("Index", new RouteValueDictionary {{"departmentId", category.DepartmentId}});
            }

            Department dept = departmentRepository.Find(category.DepartmentId);
            category.DepartmentId = dept.Id;
            category.Department = dept;
            PopulateDropDownList();
            
            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id)
        {
            var category = categoryRepository.Find(id);
            PopulateDropDownList(category.CycleId, category.UnitId);
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.InsertOrUpdate(category);
                categoryRepository.Save();
                return RedirectToAction("Index", new {departmentId = category.DepartmentId});
            }
            
            PopulateDropDownList();
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id)
        {
            return View(categoryRepository.Find(id));
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, int departmentId)
        {
            categoryRepository.Delete(id);
            categoryRepository.Save();

            return RedirectToAction("Index", new {departmentId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                categoryRepository.Dispose();
                departmentRepository.Dispose();
                cycleRepository.Dispose();
                unitRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        private void PopulateDropDownList(object selectedCycle = null, object selectedUnit = null)
        {
            IOrderedQueryable<Cycle> cycles = cycleRepository.All.OrderBy(c => c.CycleName);
            ViewBag.CycleId = new SelectList(cycles, "Id", "CycleName", selectedCycle);

            var units = unitRepository.All.OrderBy(c => c.UnitName);
            ViewBag.UnitId = new SelectList(units, "Id", "UnitName", selectedUnit);
        }
    }
}