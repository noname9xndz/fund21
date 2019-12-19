using AutoMapper;
using System.Collections.Generic;

namespace smartFunds.Service.Mapper
{
    public class EventSublocalityCustomResolver : IValueResolver<Service.Models.Event, Data.Models.Event, ICollection<Data.Models.EventSublocality>>
    {
        public ICollection<Data.Models.EventSublocality> Resolve(Service.Models.Event source, Data.Models.Event destination, ICollection<Data.Models.EventSublocality> destMember, ResolutionContext context)
        {
            int eventId = (int) context.Items["eventId"];
            var eventSublocalities = new List<Data.Models.EventSublocality>();
            foreach (var item in source.Sublocalities)
            {
                eventSublocalities.Add(new Data.Models.EventSublocality { EventId = eventId, SublocalityId = item.Id });
            }
            return eventSublocalities;
        }
    }
}
