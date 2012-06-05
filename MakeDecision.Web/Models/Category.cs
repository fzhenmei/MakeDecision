using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        //public int DeptmentId { get; set; } //?

        //public int CycleTypeId { get; set; } //?

        [Display(Name = "分类名称")]
        public string CategoryName { get; set; }

        [Display(Name = "部门")]
        public virtual Department Department { get; set; }

        [Display(Name = "更新周期")]
        public virtual Cycle Cycle { get; set; }
    }
}