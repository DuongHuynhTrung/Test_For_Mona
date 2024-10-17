using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Employee : BaseEntity
    {
        public string? EmployeeCode { get; set; }
        public required string Name { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position? Position { get; set; }
    }
}
