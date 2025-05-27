using AutoMapper;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.DTOs;

namespace Fuxikarte.Backend.Mappings
{
    public class LocalProfile : Profile
    {
        public LocalProfile()
        {
            CreateMap<Local, LocalDTO>();
            CreateMap<Local, LocalNavDTO>();
            CreateMap<NewLocalDTO, Local>();
            CreateMap<UpdateLocalDTO, Local>();
        }
    }
}
