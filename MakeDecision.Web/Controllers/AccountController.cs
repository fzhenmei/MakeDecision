using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MakeDecision.Web.Models;

namespace MakeDecision.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IDepartmentUserRepository departmentUserRepository;

        public AccountController() : this(new DepartmentRepository(), new DepartmentUserRepository())
        {
        }

        public AccountController(IDepartmentRepository departmentRepository,
                                 IDepartmentUserRepository departmentUserRepository)
        {
            this.departmentRepository = departmentRepository;
            this.departmentUserRepository = departmentUserRepository;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
            if (page == null || page <= 0)
            {
                ViewBag.CurrentPage = 1;
            }
            else
            {
                ViewBag.CurrentPage = page;
            }

            ViewBag.PageSize = 12;

            int recordCount;
            List<string> users =
                (from MembershipUser user in
                     Membership.Provider.GetAllUsers((int) ViewBag.CurrentPage - 1, (int) ViewBag.PageSize,
                                                     out recordCount)
                 select user.UserName).ToList();
            List<UserModel> userModels =
                departmentUserRepository.All
                    .Where(d => users.Contains(d.UserName))
                    .Select(d =>
                            new UserModel
                                {
                                    UserName = d.UserName,
                                    Department = d.Department
                                })
                    .ToList();

            ViewBag.TotalCount = recordCount > 0 ? recordCount - 1 : recordCount;/*减去Administrator*/

            return View(userModels);
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "用户名或密码不正确。");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [Authorize]
        public ActionResult Register()
        {
            PopulateDropDownList();
            return View();
        }

        //
        // POST: /Account/Register
        [Authorize]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(model.UserName, model.Password, "null@null.com", null, null, true, null,
                                          out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(model.UserName, "User");
                        var departmentUser = new DepartmentUser
                                                 {UserName = model.UserName, DepartmentId = model.DepartmentId};
                        departmentUserRepository.InsertOrUpdate(departmentUser);
                        departmentUserRepository.Save();

                        return RedirectToAction("Index", "Account");
                    }

                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            PopulateDropDownList();
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError("", "旧密码不正确或者新密码不符合规则。");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string username)
        {
            MembershipUser user = Membership.GetUser(username);
            if (user != null)
            {
                return View(new UserModel {UserName = user.UserName});
            }

            return Redirect("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DoDelete(string userName)
        {
            if (string.Compare(userName, "Administrator", true) == 0)
            {
                return RedirectToAction("Index");
            }

            Membership.DeleteUser(userName);
            return RedirectToAction("Index");
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已经存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "e-mail已经存在。";

                case MembershipCreateStatus.InvalidPassword:
                    return "密码不符合要求，请重新输入。";

                case MembershipCreateStatus.InvalidEmail:
                    return "e-mail格式不正确。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "系统故障：The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "注册被取消。The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "系统故障，发生未知错误。";
            }
        }

        private void PopulateDropDownList(object selectedDepartment = null)
        {
            IOrderedQueryable<Department> departments = departmentRepository.All.OrderBy(c => c.DepartmentName);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "DepartmentName", selectedDepartment);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                departmentRepository.Dispose();
                departmentUserRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}