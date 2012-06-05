using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name = "部门名称")]
        public string DepartmentName { get; set; }

        [Display(Name = "部门数据分类")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}