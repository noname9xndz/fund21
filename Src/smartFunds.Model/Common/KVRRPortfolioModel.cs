using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using smartFunds.Common;

namespace smartFunds.Model.Common
{
    public class KVRRPortfolioModel
    {
        public int KVRRId { get; set; }
        public KVRRModel KVRR { get; set; }
        public int PortfolioId { get; set; }
        public PortfolioModel Portfolio { get; set; }
    }
}
