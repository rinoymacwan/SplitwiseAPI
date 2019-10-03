using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;

namespace SplitwiseAPI.Models
{
    public class SplitwiseAPIContext : DbContext
    {
        public SplitwiseAPIContext (DbContextOptions<SplitwiseAPIContext> options)
            : base(options)
        {
        }

        public DbSet<SplitwiseAPI.DomainModel.Models.Users> Users { get; set; }

        public DbSet<SplitwiseAPI.DomainModel.Models.UserFriendMappings> UserFriendMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }

        public DbSet<SplitwiseAPI.DomainModel.Models.Groups> Groups { get; set; }

        public DbSet<SplitwiseAPI.DomainModel.Models.GroupMemberMappings> GroupMemberMappings { get; set; }

        public DbSet<SplitwiseAPI.DomainModel.Models.Settlements> Settlements { get; set; }

        public DbSet<SplitwiseAPI.DomainModel.Models.Categories> Categories { get; set; }
    }
}
