using AutoMapper;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.DTOs;

namespace Fuxikarte.Backend.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Product, ProductNavDTO>();
            CreateMap<NewProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
}
