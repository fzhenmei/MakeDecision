using System.Web.Mvc;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CycleController : Controller
    {
        private readonly ICycleRepository cycleRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public CycleController() : this(new CycleRepository())
        {
        }

        public CycleController(ICycleRepository cycleRepository)
        {
            this.cycleRepository = cycleRepository;
        }

        //
        // GET: /Cycle/

        public ViewResult Index()
        {
            return View(cycleRepository.All);
        }

        //
        // GET: /Cycle/Details/5

        public ViewResult Details(int id)
        {
            return View(cycleRepository.Find(id));
        }

        //
        // GET: /Cycle/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cycle/Create

        [HttpPost]
        public ActionResult Create(Cycle cycle)
        {
            if (ModelState.IsValid)
            {
                cycleRepository.InsertOrUpdate(cycle);
                cycleRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Cycle/Edit/5

        public ActionResult Edit(int id)
        {
            return View(cycleRepository.Find(id));
        }

        //
        // POST: /Cycle/Edit/5

        [HttpPost]
        public ActionResult Edit(Cycle cycle)
        {
            if (ModelState.IsValid)
            {
                cycleRepository.InsertOrUpdate(cycle);
                cycleRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Cycle/Delete/5

        public ActionResult Delete(int id)
        {
            return View(cycleRepository.Find(id));
        }

        //
        // POST: /Cycle/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            cycleRepository.Delete(id);
            cycleRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cycleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}