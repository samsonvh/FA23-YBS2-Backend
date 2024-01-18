using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class TransactionPageRequest : DefaultPageRequest
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        // public int? MinPassenger { get; set; }
        // public int? MaxPassenger { get; set; }
        public EnumTransactionType? Type { get; set; }
    }
}