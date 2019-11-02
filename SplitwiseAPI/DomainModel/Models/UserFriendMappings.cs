using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class UserFriendMappings
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }

        public string FriendId { get; set; }
        [ForeignKey("FriendId")]
        public Users Friend { get; set; }

    }
}
