﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.DomainModel.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string MadeById { get; set; }
        [ForeignKey("MadeById")]
        public Users User { get; set; }
    }
}
