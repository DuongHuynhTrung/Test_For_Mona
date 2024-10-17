
using AutoMapper;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Microsoft.EntityFrameworkCore;

namespace Services.Core;

public interface IEmployeeService
{
    Task<ResultModel> AddEmployee(EmployeeCreateModel model);
    Task<ResultModel> GetAllEmployees(PagingParam paginationModel);
    Task<ResultModel> GetEmployeeByEmployeeCode(string employeeCode);
    Task<ResultModel> UpdateEmployee(EmployeeUpdateModel model);
    Task<ResultModel> DeleteEmployeeByEmployeeCode(string employeeCode);
}
public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public EmployeeService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> AddEmployee(EmployeeCreateModel model)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var position = await _dbContext.Positions.FirstOrDefaultAsync(_ => _.PositionName == model.PositionName);
            if (position == null)
            {
                result.ErrorMessage = "Position does not exist.";
                return result;
            }

            //Check Employee Greater than 18 years old
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
            DateOnly dob = new(model.DateOfBirth.Year, model.DateOfBirth.Month, model.DateOfBirth.Day);

            int ageDifferenceInYears = currentDate.Year - dob.Year;

            if (dob > currentDate.AddYears(-ageDifferenceInYears))
            {
                ageDifferenceInYears--;
            }
            if (ageDifferenceInYears < 18)
            {
                result.ErrorMessage = "Employee must be at least 18 years old.";
                result.Succeed = false;
                return result;
            }

            var employee = _mapper.Map<EmployeeCreateModel, Employee>(model);
            employee.PositionId = position.Id;
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            //[NV]_[YYYY]_[MM]_[DD]_[Số thứ tự] ví dụ NV_2023_12_23_1   
            employee.EmployeeCode = GenerateEmployeeCode(employee);
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();

            result.Succeed = true;
            result.Data = employee;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetAllEmployees(PagingParam paginationModel)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Employees
                .Include(_ => _.Position).AsQueryable();
            var employees = data.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<EmployeeGetAllModel>(employees);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count())
            {
                Data = viewModels
            };
            result.Data = paging;
            result.Succeed = true;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetEmployeeByEmployeeCode(string employeeCode)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var employee = await _dbContext.Employees
                .Include(_ => _.Position)
                .FirstOrDefaultAsync(_ => _.EmployeeCode == employeeCode);

            if (employee == null)
            {
                result.ErrorMessage = "Employee not found.";
                return result;
            }

            var employeeModel = _mapper.Map<EmployeeModel>(employee);

            result.Succeed = true;
            result.Data = employeeModel;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateEmployee(EmployeeUpdateModel model)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var employee = await _dbContext.Employees
                .Include(_ => _.Position)
                .FirstOrDefaultAsync(_ => _.EmployeeCode == model.EmployeeCode);
            if (employee == null)
            {
                result.ErrorMessage = "Employee not found.";
                return result;
            }

            if (model.Name != null)
            {
                employee.Name = model.Name;
            }

            if (model.DateOfBirth != null)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
                DateOnly dob = new(model.DateOfBirth.Value.Year, model.DateOfBirth.Value.Month, model.DateOfBirth.Value.Day);

                int ageDifferenceInYears = currentDate.Year - dob.Year;

                if (dob > currentDate.AddYears(-ageDifferenceInYears))
                {
                    ageDifferenceInYears--;
                }
                if (ageDifferenceInYears < 18)
                {
                    result.ErrorMessage = "Employee must be at least 18 years old.";
                    result.Succeed = false;
                    return result;
                }
                employee.DateOfBirth = (DateOnly)model.DateOfBirth;
            }

            if (model.Position != null)
            {
                var position = await _dbContext.Positions.FirstOrDefaultAsync(_ => _.PositionName == model.Position);
                if (position == null)
                {
                    result.ErrorMessage = "Position does not exist.";
                    return result;
                }
                employee.PositionId = position.Id;
            }
            employee.DateUpdated = DateTime.Now;

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();

            result.Succeed = true;
            result.Data = employee;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> DeleteEmployeeByEmployeeCode(string employeeCode)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(_ => _.EmployeeCode == employeeCode);
            if (employee == null)
            {
                result.ErrorMessage = "Employee not found.";
                return result;
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            result.Succeed = true;
            result.Data = "Employee deleted successfully.";
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    private string GenerateEmployeeCode(Employee employee)
    {
        var datePart = DateTime.Now.ToString("yyyy_MM_dd");
        return $"NV_{datePart}_{employee.Id}";
    }
}
