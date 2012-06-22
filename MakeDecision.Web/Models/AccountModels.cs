using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MakeDecision.Web.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密码至少要{2}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "两次输入的新密码不一致。")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我？")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密码至少要{2}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码和密码不一致。")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "所在部门")]
        public int DepartmentId { get; set; }
    }

    public class UserModel
    {
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "所在部门")]
        public Department Department { get; set; }
    }
}