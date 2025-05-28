using AutoMapper;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.DTOs;

namespace Fuxikarte.Backend.Mappings
{
    public class SaleProductProfile : Profile
    {
        public SaleProductProfile()
        {
            CreateMap<SaleProduct, SaleProductDTO>();
            CreateMap<SaleProduct, ProductsInSaleDTO>();
            CreateMap<SaleProduct, SalesForProductDTO>();
            CreateMap<NewSaleProductDTO, SaleProduct>();
            CreateMap<UpdateSaleProductDTO, SaleProduct>();
        }
    }
}
