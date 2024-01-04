using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.PageResponses
{
    public class VNPayBookingResponse
    {
        public string Code {get; set;}
        public float TotalAmount { get; set; }
        public string Name { get; set; }
        public string BankCode { get; set; }
        public string BankTranNo { get; set; }
        public string CardType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string VNPayCode { get; set; }
        public bool Success {get; set;}
    }
}