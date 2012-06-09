using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MakeDecision.Web.Models
{
    public class MakeDecisionWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MakeDecision.Web.Models.MakeDecisionWebContext>());

        public DbSet<MakeDecision.Web.Models.Cycle> Cycles { get; set; }

        public DbSet<MakeDecision.Web.Models.Category> Categories { get; set; }

        public DbSet<MakeDecision.Web.Models.Department> Departments { get; set; }

        public DbSet<MakeDecision.Web.Models.KeyData> KeyDatas { get; set; }
    }
}