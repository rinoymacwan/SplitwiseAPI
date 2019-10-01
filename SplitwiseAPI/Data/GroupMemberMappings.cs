using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Data
{
    public class GroupMemberMappings
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Groups Group { get; set; }

        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Users User { get; set; }
    }
}
