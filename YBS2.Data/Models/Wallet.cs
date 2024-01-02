using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Data.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        public int Point { get; set; }
    }
}