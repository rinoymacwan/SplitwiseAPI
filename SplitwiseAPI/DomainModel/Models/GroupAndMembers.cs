using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class GroupAndMembers
    {
        public Groups Group { get; set; }
        public List<Users> Members { get; set; }
    }
}
