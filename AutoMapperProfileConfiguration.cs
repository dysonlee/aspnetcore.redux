using AutoMapper;

namespace Web
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            Configure();
        }

        private void Configure()
        {
            // Example
            // CreateMap<SourceModel, TargetModel>()
            //     .ForMember(c => c.Property1, opt => opt.Ignore())
            //     .AfterMap((sourceModel, targetModel, ctx) =>
            //     {
            //     });
        }
    }
}
