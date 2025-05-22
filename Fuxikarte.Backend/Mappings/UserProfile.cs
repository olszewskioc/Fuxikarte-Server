// Caminho: Fuxikarte.Backend/Mappings/MappingProfile.cs

using AutoMapper;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.DTOs;

namespace Fuxikarte.Backend.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserRegistrationDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            // Adicione outros mapeamentos conforme necessário
        }
    }
}
