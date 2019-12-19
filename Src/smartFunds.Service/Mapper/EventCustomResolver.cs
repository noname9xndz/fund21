using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using smartFunds.Service.Models;

namespace smartFunds.Service.Mapper
{
    public class EventCustomResolver : IValueResolver<Data.Models.Event, Event, List<Sublocality>>
    {
        public List<Sublocality> Resolve(Data.Models.Event source, Event destination, List<Sublocality> destMember, ResolutionContext context)
        {
            var sublocalities = new List<Sublocality>();
            if (source.EventSublocalities == null || !source.EventSublocalities.Any()) return sublocalities;
            foreach (var item in source.EventSublocalities)
            {
                sublocalities.Add(new Sublocality
                {
                    Id = item.SublocalityId,
                    Name = item.Sublocality.Name,
                    LocalityId = item.Sublocality.LocalityId
                });
            }

            return sublocalities;
        }
    }
}
