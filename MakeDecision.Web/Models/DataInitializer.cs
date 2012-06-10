using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MakeDecision.Web.Models
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<MakeDecisionWebContext>
    {
        protected override void Seed(MakeDecisionWebContext context)
        {
            context.Database.CompatibleWithModel(false);

            var cycles = new List<Cycle>
                             {
                                 new Cycle {CycleName = "年"},
                                 new Cycle {CycleName = "月"},
                                 new Cycle {CycleName = "周"},
                                 new Cycle {CycleName = "天"}
                             };
            cycles.ForEach(c => context.Cycles.Add(c));
            context.SaveChanges();

            var departments = new List<Department>
                                  {
                                      new Department {DepartmentName = "生产技术部"}
                                  };
            departments.ForEach(c => context.Departments.Add(c));
            context.SaveChanges();

            var categories = new List<Category>
                                 {
                                     new Category
                                         {
                                             CategoryName = "线损率",
                                             Department = departments.First(),
                                             Cycle = cycles.Where(c => c.CycleName == "月").Single()
                                         },
                                     new Category
                                         {
                                             CategoryName = "综合电压合格率",
                                             Department = departments.First(),
                                             Cycle = cycles.Where(c => c.CycleName == "月").Single()
                                         },
                                     new Category
                                         {
                                             CategoryName = "220kV及以上架空线路跳闸次数（率）",
                                             Department = departments.First(),
                                             Cycle = cycles.Where(c => c.CycleName == "月").Single()
                                         },
                                     new Category
                                         {
                                             CategoryName = "110kV架空线路跳闸次数（率）",
                                             Department = departments.First(),
                                             Cycle = cycles.Where(c => c.CycleName == "月").Single()
                                         },
                                     new Category
                                         {
                                             CategoryName = "35kV线路跳闸次数（率）",
                                             Department = departments.First(),
                                             Cycle = cycles.Where(c => c.CycleName == "月").Single()
                                         }
                                 };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var keyDatas = new List<KeyData>
                               {
                                   new KeyData
                                       {
                                           Value = 1,
                                           Year = 2012,
                                           CreateDate = DateTime.Now,
                                           Category = categories.First(),
                                       }
                               };
            keyDatas.ForEach(c => context.KeyDatas.Add(c));
            context.SaveChanges();
        }
    }
}