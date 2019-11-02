using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Settlements
    {
        public int Id { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Groups Group { get; set; }

        public string PayerId { get; set; }
        [ForeignKey("PayerId")]
        public Users Payer { get; set; }

        public string PayeeId { get; set; }
        [ForeignKey("PayeeId")]
        public Users Payee { get; set; }

        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
    }
}
