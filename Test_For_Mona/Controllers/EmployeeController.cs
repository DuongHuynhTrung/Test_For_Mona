using Data.Common.PaginationModel;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace Test_For_Mona.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] EmployeeCreateModel model)
    {
        var result = await _employeeService.AddEmployee(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllEmployee([FromQuery] PagingParam paginationModel)
    {
        var result = await _employeeService.GetAllEmployees(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{EmployeeCode}")]
    public async Task<ActionResult> GetEmployeeByEmployeeCode(string EmployeeCode)
    {
        var result = await _employeeService.GetEmployeeByEmployeeCode(EmployeeCode);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut()]
    public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeUpdateModel model)
    {
        var result = await _employeeService.UpdateEmployee(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{EmployeeCode}")]
    public async Task<ActionResult> DeleteEmployeeByEmployeeCode(string EmployeeCode)
    {
        var result = await _employeeService.DeleteEmployeeByEmployeeCode(EmployeeCode);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
