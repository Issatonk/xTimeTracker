using AutoMapper;
using xTimeTracker.API.Models;

namespace xTimeTracker.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<ProjectCreateRequest, Core.Project>()
                .ForMember(proj => proj.Plan, option=> option.MapFrom(src=> new TimeSpan(src.Hours, src.Minutes, src.Seconds)));

            CreateMap<ProjectUpdateRequest, Core.Project>()
                .ForMember(proj => proj.Plan, option => option.MapFrom(src => new TimeSpan(src.Hours, src.Minutes, src.Seconds)));
        }
    }
}
