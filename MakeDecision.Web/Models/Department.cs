using System.Collections.Generic;

namespace MakeDecision.Web.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string DepartmentName { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}