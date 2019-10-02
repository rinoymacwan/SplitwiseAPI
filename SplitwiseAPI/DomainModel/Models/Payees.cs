using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Payees
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }
        [ForeignKey("ExpenseId")]
        public Expenses Expense { get; set; }

        public int PayeeId { get; set; }
        [ForeignKey("PayeeId")]
        public Users User { get; set; }

        public int PayeeShare { get; set; }
    }
}
