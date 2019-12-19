using AutoMapper;
using smartFunds.Service.Models;

namespace smartFunds.Service.Mapper
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Data.Models.Setting, Setting>();
            CreateMap<Data.Models.HangFire.Job, Models.HangFire.Job>();            
            CreateMap<Data.Models.Interchange, Interchange>()
                .ForMember(x => x.CountryName, y => y.MapFrom(z => z.Country.Name))
                .ForMember(x => x.MainLocalityName, y => y.MapFrom(z => z.Locality.Name))
                .ForMember(x => x.Localities, opt => opt.ResolveUsing(new InterchangeCustomResolver()));
            CreateMap<Interchange, Data.Models.Interchange>()
                .ForMember(x => x.InterchangeLocalities, opt => opt.ResolveUsing(new InterchangeLocalityCustomResolver()));
            CreateMap<Event, Data.Models.Event>()
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore())
                .ForMember(x => x.DateLastUpdated, opt => opt.Ignore())
                .ForMember(x => x.LastUpdatedBy, opt => opt.Ignore());
            CreateMap<Data.Models.Event, Event>()
                .ForMember(x => x.CountryName, y => y.MapFrom(z => z.Country.Name))
                .ForMember(x => x.MainLocalityName, y => y.MapFrom(z => z.Locality.Name))
                .ForMember(x => x.EventName, opt => opt.Ignore())
                .ForMember(x => x.Sublocalities, opt => opt.ResolveUsing(new EventCustomResolver()));
            CreateMap<Event, Data.Models.Event>()
                .ForMember(x => x.EventSublocalities, opt => opt.ResolveUsing(new EventSublocalityCustomResolver()));
            CreateMap<Data.Models.Contactbase.Locality, Locality>()
                .ForMember(x => x.Sublocalities, opt => opt.ResolveUsing(new LocalityCustomResolver()));
            CreateMap<EventGuestModel, Data.Models.EventGuest>()
                .ForMember(x => x.DateLastUpdated, opt => opt.Ignore())
                .ForMember(x => x.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore())
                .ForMember(x => x.Event, opt => opt.Ignore());
            CreateMap<Data.Models.Test, Model.Common.TestModel>();
            CreateMap<Model.Common.TestModel, Data.Models.Test>();
        }
    }
}
