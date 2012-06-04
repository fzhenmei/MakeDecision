using System;
using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    /// <summary>
    /// 生产技术部1线损率
    /// </summary>
    public class ProductionTech
    {
        public int Id { get; set; }

        /// <summary>
        /// 值的类型
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// 值的单位
        /// </summary>
        public int ValueUnit { get; set; }

        [Display(Name = "参照值")]
        public float Value { get; set; }

        public int Year { get; set; }

        [Display(Name = "周期")]
        public int Cycle { get; set; }

        public DateTime CreateDate { get; set; }
    }
}