using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.PageResponses
{
    public class VNPayResponseModel
    {
        public Guid MembershipPackageId { get; set; }
        public string VnpayResponseCode { get; set; }
        public DateTime VnpayPaymentDate { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionStatus { get; set; }
    }
}