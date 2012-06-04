using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    /// <summary>
    /// 周期
    /// </summary>
    public class Cycle
    {
        public int Id { get; set; }

        [Display(Name = "周期类型名称")]
        public string CycleName { get; set; }
    }
}