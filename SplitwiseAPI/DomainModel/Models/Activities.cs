using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Activities
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }

        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}
