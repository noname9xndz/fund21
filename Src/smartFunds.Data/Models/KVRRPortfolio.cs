using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class KVRRPortfolio
    {
        public int KVRRId { get; set; }
        public KVRR KVRR { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
