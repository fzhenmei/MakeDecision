using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    /// <summary>
    /// 单位
    /// </summary>
    public class Unit
    {
        public int Id { get; set; }

        [Display(Name = "单位")]
        public string UnitName { get; set; }
    }
}