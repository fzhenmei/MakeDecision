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
            IQueryable<DepartmentUser> departmentUser =
                departmentUserRepository.AllIncluding(du => du.Department, du => du.Department.Categories).Where(
                    du => du.UserName == User.Identity.Name);
            //var keyDatas = keyDataRepository.All.Where(k => departmentUser.Select(c => c.Department.Categories.Select(ca => ca.Id)).con);
            return Redirect("/KeyData");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}