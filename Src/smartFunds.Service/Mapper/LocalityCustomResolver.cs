using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using smartFunds.Service.Models;

namespace smartFunds.Service.Mapper
{
    public class LocalityCustomResolver : IValueResolver<Data.Models.Contactbase.Locality, Locality, List<Sublocality>>
    {
        public List<Sublocality> Resolve(Data.Models.Contactbase.Locality source, Locality destination, List<Sublocality> destMember, ResolutionContext context)
        {
            var sublocalities = new List<Sublocality>();
            if (source.Sublocalities == null || !source.Sublocalities.Any()) return sublocalities;
            foreach (var item in source.Sublocalities)
            {
                sublocalities.Add(new Sublocality
                {
                    Id = item.Id,
                    Name = item.Name,
                    LocalityId = item.LocalityId
                });
            }

            return sublocalities;
        }
    }
}
