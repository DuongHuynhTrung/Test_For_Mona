using AutoMapper;
using Data.Entities;
using Data.Models;

namespace Services.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, EmployeeModel>();
            CreateMap<Employee, EmployeeGetAllModel>();
            CreateMap<EmployeeCreateModel, Employee>();
            CreateMap<Position, PositionModel>();
            CreateMap<PositionCreateModel, Position>();
        }
    }
}
