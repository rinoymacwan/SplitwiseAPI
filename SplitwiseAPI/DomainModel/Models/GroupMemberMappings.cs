using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class GroupMemberMappings
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Groups Group { get; set; }

        public string MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Users User { get; set; }
    }
}
