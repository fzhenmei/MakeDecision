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

        /// <summary>
        /// 数据分类
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// 值的单位
        /// </summary>
        public int ValueUnit { get; set; }

        [Display(Name = "参照值")]
        public float Value { get; set; }

        /// <summary>
        /// 更新年份
        /// </summary>
        public int Year { get; set; }

        [Display(Name = "周期")]
        public int Cycle { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}