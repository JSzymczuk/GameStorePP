using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace GameStore.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderPosition> OrderPositions { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<OrderStatusChange> OrderStatusChanges { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Pegi> Pegi { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Requirements> Requirements { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}