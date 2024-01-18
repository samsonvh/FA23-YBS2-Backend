using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid? MembershipRegistrationId { get; set; }
        [ForeignKey("MembershipRegistrationId")]
        public MembershipRegistration? MembershipRegistration { get; set; }
        public Guid? BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public float TotalAmount { get; set; }
        public float? Point { get; set; }
        public string? BankCode { get; set; }
        public string? BankTranNo { get; set; }
        public string? CardType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? VNPayCode { get; set; }
        public bool Success { get; set; }
        public EnumTransactionType Type { get; set; }
    }
}