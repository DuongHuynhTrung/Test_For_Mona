
namespace Data.Models;

public class EmployeeModel : BaseEntityModel
{
    public string EmployeeCode { get; set; }
    public string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public PositionModel Position { get; set; }
}

public class EmployeeGetAllModel
{
    public string EmployeeCode { get; set; }
    public string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Age
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth > today.AddYears(-age)) age--;
            return age;
        }
    }
}

public class EmployeeCreateModel
{
    public required string Name { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string PositionName { get; set; }
}

public class EmployeeUpdateModel
{
    public required string EmployeeCode { get; set; }
    public string? Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Position { get; set; }
}