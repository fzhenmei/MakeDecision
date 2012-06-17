using System.Web.Mvc;

namespace MakeDecision.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "欢迎使用领导决策系统数据收集";

            return Redirect("/KeyData");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}