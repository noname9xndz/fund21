using AutoMapper;
using System.Collections.Generic;

namespace smartFunds.Service.Mapper
{
    public class InterchangeLocalityCustomResolver : IValueResolver<Service.Models.Interchange, Data.Models.Interchange, ICollection<Data.Models.InterchangeLocality>>
    {
        public ICollection<Data.Models.InterchangeLocality> Resolve(Service.Models.Interchange source, Data.Models.Interchange destination, ICollection<Data.Models.InterchangeLocality> destMember, ResolutionContext context)
        {
            int interchangeId = (int) context.Items["interchangeId"];
            var interchangeLocalities = new List<Data.Models.InterchangeLocality>();
            foreach (var item in source.Localities)
            {
                interchangeLocalities.Add(new Data.Models.InterchangeLocality { InterchangeId = interchangeId, LocalityId = item.Id });
            }
            return interchangeLocalities;
        }
    }
}
