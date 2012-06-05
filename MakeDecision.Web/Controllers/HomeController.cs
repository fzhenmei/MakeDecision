using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakeDecision.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "欢迎使用领导决策系统数据收集";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
