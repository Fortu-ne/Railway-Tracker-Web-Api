using AutoMapper;
using Railway.Data;
using Railway.Dtos;

namespace Railway.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Train, TrainDto>();
            CreateMap<TrainDto, Train>();
            CreateMap<Station, StationDto>();
            CreateMap<StationDto, Station>();
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<ScheduleDto, Schedule>();
        }
    }
}
