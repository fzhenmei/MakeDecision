using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDepartmentUserRepository departmentUserRepository;
        private readonly IKeyDataRepository keyDataRepository;

        public HomeController() : this(new DepartmentUserRepository(), new KeyDataRepository())
        {
        }

        public HomeController(IDepartmentUserRepository departmentUserRepository, IKeyDataRepository keyDataRepository)
        {
            this.departmentUserRepository = departmentUserRepository;
            this.keyDataRepository = keyDataRepository;
        }

        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("Admin");
            }

            var departmentUser =
                departmentUserRepository.AllIncluding(du => du.Department, du => du.Department.Categories).Where(
                    du => du.UserName == User.Identity.Name).ToList();
            
            var categories = new List<Category>();
            foreach (var d in departmentUser.Select(c => c.Department))
            {
                categories.AddRange(d.Categories);
            }

            foreach (var category in categories)
            {
                category.KeyDatas = GetKeyDatas(category.Id).ToList();
            }
            
            return View(categories.OrderBy(c => c.CategoryName));
        }

        public ActionResult About()
        {
            return View();
        }

        public IQueryable<KeyData> GetKeyDatas(int categoryId)
        {
            return keyDataRepository.All.Where(c => c.CategoryId == categoryId).OrderByDescending(c => c.Id).Take(2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                departmentUserRepository.Dispose();
                keyDataRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}