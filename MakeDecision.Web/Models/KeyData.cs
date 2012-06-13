using System;
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


        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}