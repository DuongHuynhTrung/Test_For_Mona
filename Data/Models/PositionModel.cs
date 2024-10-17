
namespace Data.Models;

public class PositionModel : BaseEntityModel
{
    public string PositionName { get; set; }
}

public class PositionCreateModel
{
    public required string PositionName { get; set; }
}

public class PositionUpdateModel
{
    public required int PositionId { get; set; }
    public required string PositionName { set; get; }
}
