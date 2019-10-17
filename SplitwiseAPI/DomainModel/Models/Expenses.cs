using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Expenses
    {
        public int Id { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Groups Group { get; set; }

        public int AddedById { get; set; }
        [ForeignKey("AddedById")]
        public Users User { get; set; }

        public DateTime DateTime { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
            
        public string Currency{ get; set; }
        public int Total { get; set; }
        public string SplitBy { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }
}
