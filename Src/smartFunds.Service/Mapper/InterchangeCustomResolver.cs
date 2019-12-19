using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using smartFunds.Service.Models;

namespace smartFunds.Service.Mapper
{
    public class InterchangeCustomResolver : IValueResolver<Data.Models.Interchange, Interchange, List<Locality>>
    {
        public List<Locality> Resolve(Data.Models.Interchange source, Interchange destination, List<Locality> destMember, ResolutionContext context)
        {
            var localities = new List<Locality>();
            if (source.InterchangeLocalities == null || !source.InterchangeLocalities.Any()) return localities;
            foreach (var item in source.InterchangeLocalities)
            {
                localities.Add(new Locality
                {
                    Id = item.LocalityId,
                    Name = item.Locality?.Name,
                    CountryCode = item.Locality?.CountryCode,
                    Sublocalities = item.Locality?.Sublocalities?.Select(x => new Sublocality
                    {
                        Id = x.Id,
                        Name = x.Name,
                        LocalityId = x.LocalityId
                    }).ToList()
                });
            }

            return localities;
        }
    }
}
