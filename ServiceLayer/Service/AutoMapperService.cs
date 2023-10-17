using AutoMapper;
using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using Services_Layer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.Service
{
    public class AutoMapperService : IAutoMapperService
    {
        private readonly IMapper mapper;

        public AutoMapperService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductMappingProfile());
            });

            mapper = config.CreateMapper();
        }
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sourceList)
        {
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sourceList);
        }

        public class ProductMappingProfile : Profile
        {
            public ProductMappingProfile()
            {
                CreateMap<Product, ProductRequest>();
            }
        }
    }
}
