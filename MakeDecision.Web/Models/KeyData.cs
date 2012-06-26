using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    /// <summary>
    /// 关键数据
    /// </summary>
    public class KeyData
    {
        public int Id { get; set; }

        [Display(Name = "数据分类")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 数据分类
        /// </summary>
        public virtual Category Category { get; set; }

        [Display(Name = "参照值")]
        public decimal Value { get; set; }

        /// <summary>
        /// 更新年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 本数据更新周期
        /// </summary>
        [Display(Name = "更新周期")]
        public int CycleValue { get; set; }

        [Display(Name = "文件路径")]
        public string FilePath { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        public override string ToString()
        {
            if (Category == null)
            {
                return Value.ToString("0.00");
            }

            if (Category.Unit == null)
            {
                return Value.ToString("0.00");
            }

            switch (Category.Unit.Digits)
            {
                case 0:
                    return decimal.Ceiling(Value).ToString();
                case 1:
                    return Value.ToString("0.0");
                case 2:
                    return Value.ToString("0.00");
                case 3:
                    return Value.ToString("0.000");
                case 4: 
                    return Value.ToString("0.0000");
                case 5:
                    return Value.ToString("0.00000");
                case 6:
                    return Value.ToString("0.000000");
                default:
                    return Value.ToString("0.00");
            }
        }
    }
}