using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "部门")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "更新周期")]
        public int CycleId { get; set; }

        [Required]
        [Display(Name = "单位")]
        public int UnitId { get; set; }

        [Required]
        [Display(Name = "分类名称")]
        public string CategoryName { get; set; }

        public virtual Department Department { get; set; }

        public virtual Cycle Cycle { get; set; }

        public virtual Unit Unit { get; set; }

        public ICollection<KeyData> KeyDatas { get; set; }
    }
}