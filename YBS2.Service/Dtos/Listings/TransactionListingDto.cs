using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class TransactionListingDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public float TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Success { get; set; }
        public string Type { get; set; }
    }
}