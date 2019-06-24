using AutoMapper;
using Entities = ImageGallery.Domain.Entities;

namespace ImageGallery.API.Automapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Entities.Image, Model.Image>().ReverseMap();

            CreateMap<Model.ImageForCreation, Entities.Image>()
                    .ForMember(m => m.FileName, options => options.Ignore())
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.OwnerId, options => options.Ignore());

            CreateMap<Model.ImageForUpdate, Entities.Image>()
                .ForMember(m => m.FileName, options => options.Ignore())
                .ForMember(m => m.Id, options => options.Ignore())
                .ForMember(m => m.OwnerId, options => options.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}
