namespace MakeDecision.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        public int DeptmentId { get; set; } //?

        public int CycleTypeId { get; set; } //?

        public string CategoryName { get; set; }

        public virtual Department Department { get; set; }

        public virtual Cycle Cycle { get; set; }
    }
}