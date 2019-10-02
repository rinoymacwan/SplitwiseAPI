using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Payers
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }
        [ForeignKey("ExpenseId")]
        public Expenses Expense { get; set; }

        public int PayerId { get; set; }
        [ForeignKey("PayerId")]
        public Users User { get; set; }

        public int AmountPaid { get; set; }
        public int PayerShare { get; set; }
    }
}
