using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Core;


namespace Test_For_Mona.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;

    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpPost]
    public async Task<ActionResult> AddPosition([FromBody] PositionCreateModel model)
    {
        var result = await _positionService.AddPosition(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPosition()
    {
        var result = await _positionService.GetAllPosition();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{PositionId}")]
    public async Task<ActionResult> GetPositionById(int PositionId)
    {
        var result = await _positionService.GetPositionById(PositionId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut()]
    public async Task<ActionResult> UpdatePosition([FromBody] PositionUpdateModel model)
    {
        var result = await _positionService.UpdatePosition(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{PositionId}")]
    public async Task<ActionResult> DeletePositionById(int PositionId)
    {
        var result = await _positionService.DeletePositionById(PositionId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
