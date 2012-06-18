using System.ComponentModel.DataAnnotations;

namespace MakeDecision.Web.Models
{
    public class DepartmentUser
    {
        public int Id { get; set; }
        
        public string UserName{ get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}