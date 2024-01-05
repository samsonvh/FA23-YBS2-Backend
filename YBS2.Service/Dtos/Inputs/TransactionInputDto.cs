using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class TransactionInputDto
    {
        public Guid? MemberId { get; set; }
        public Guid? MembershipPackageId { get; set; }
        public Guid? BookingId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float TotalAmount { get; set; }
        [Required]
        public string BankCode { get; set; }
        [Required]
        public string BankTranNo { get; set; }
        [Required]
        public string CardType { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public string VNPayCode { get; set; }
        [Required]
        public bool Success { get; set; }
    }
}