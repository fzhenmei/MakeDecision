using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    /// <summary>
    /// 单位
    /// </summary>
    public class Unit
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "单位名称不能为空。")]
        [Display(Name = "单位")]
        public string UnitName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "单位标签不能为空")]
        [Display(Name = "单位标签")]
        public string UnitLabel { get; set; }

        [Required(ErrorMessage = "必须为单位提供小数位数")]
        [Range(0,6,ErrorMessage = "小数位数范围需要在0-6之间。")]
        public int Digits { get; set; }
    }
}