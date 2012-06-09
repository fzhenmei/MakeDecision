using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "部门")]
        public int DepartmentId { get; set; } 

        [Display(Name = "更新周期")]
        public int CycleId { get; set; }

        [Display(Name = "分类名称")]
        public string CategoryName { get; set; }

        public Department Department { get; set; }

        public Cycle Cycle { get; set; }
    }
}