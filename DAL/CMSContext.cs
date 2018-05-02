using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ContainerManagementSystem.Models.User;
using ContainerManagementSystem.Models.ShippingChannel;
using ContainerManagementSystem.Models.ShipmentRouteDetail;
using ContainerManagementSystem.Models.ShipmentRoute;
using ContainerManagementSystem.Models.Shipment;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Models.Country;
using ContainerManagementSystem.Models.ShipmentHistory;

namespace ContainerManagementSystem.DAL
{
    public class CMSContext : DbContext
    {
        public CMSContext()
            : base("CMSContext")
        { 
        
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentRoute> ShipmentRoutes { get; set; }
        public DbSet<ShipmentRouteDetail> ShipmentRouteDetails { get; set; }
        public DbSet<ShippingChannel> ShippingChannels { get; set; }
        public DbSet<ShipmentHistory> ShipmentHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<CMSContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}