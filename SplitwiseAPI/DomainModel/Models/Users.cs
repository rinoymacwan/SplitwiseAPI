using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
