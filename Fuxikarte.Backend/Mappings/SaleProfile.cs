using AutoMapper;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.DTOs;

namespace Fuxikarte.Backend.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleDTO>();
            CreateMap<Sale, SaleNavDTO>();
            CreateMap<NewSaleDTO, Sale>();
            CreateMap<UpdateSaleDTO, Sale>();
        }
    }
}
