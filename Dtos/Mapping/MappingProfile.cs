using AutoMapper;
using WebApplication1.Models;

namespace WebApplication1.Dtos.Mapping;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Category, CategoryDTO>().ReverseMap();
    CreateMap<Product, ProductDTO>().ReverseMap();
  }
}