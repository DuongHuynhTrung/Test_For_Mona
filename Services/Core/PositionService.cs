using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Core;

public interface IPositionService
{
    Task<ResultModel> AddPosition(PositionCreateModel model);
    Task<ResultModel> GetAllPosition();
    Task<ResultModel> GetPositionById(int positionId);
    Task<ResultModel> UpdatePosition(PositionUpdateModel model);
    Task<ResultModel> DeletePositionById(int positionId);
}
public class PositionService : IPositionService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public PositionService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> AddPosition(PositionCreateModel model)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var checkExistPosition = _dbContext.Positions.Any(_ => _.PositionName == model.PositionName);
            if (checkExistPosition)
            {
                result.ErrorMessage = $"Position with name `{model.PositionName}` already exists";
                return result;
            }

            var newPosition = _mapper.Map<Position>(model);
            _dbContext.Positions.Add(newPosition);
            await _dbContext.SaveChangesAsync();

            result.Succeed = true;
            result.Data = newPosition;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetAllPosition()
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var positions = await _dbContext.Positions.ToListAsync();
            result.Data = positions;
            result.Succeed = true;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetPositionById(int positionId)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var position = await _dbContext.Positions.FindAsync(positionId);
            if (position == null)
            {
                result.ErrorMessage = "Position not found";
                return result;
            }

            result.Data = position;
            result.Succeed = true;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdatePosition(PositionUpdateModel model)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var checkExistPosition = _dbContext.Positions.Any(_ => _.PositionName == model.PositionName);
            if (checkExistPosition)
            {
                result.ErrorMessage = $"Position with name `{model.PositionName}` already exists";
                return result;
            }

            var position = await _dbContext.Positions.FindAsync(model.PositionId);
            if (position == null)
            {
                result.ErrorMessage = "Position not found";
                return result;
            }

            position.PositionName = model.PositionName;
            position.DateUpdated = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            result.Succeed = true;
            result.Data = position;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> DeletePositionById(int positionId)
    {
        var result = new ResultModel
        {
            Succeed = false
        };
        try
        {
            var position = await _dbContext.Positions.FindAsync(positionId);
            if (position == null)
            {
                result.ErrorMessage = "Position not found";
                return result;
            }

            var employeeWithPosition = await _dbContext.Employees
                .AnyAsync(_ => _.PositionId == positionId);

            if (employeeWithPosition)
            {
                result.ErrorMessage = "Position is assigned to one or more employees, cannot delete.";
                return result;
            }

            _dbContext.Positions.Remove(position);
            await _dbContext.SaveChangesAsync();

            result.Succeed = true;
            result.Data = "Position deleted successfully";
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

}
