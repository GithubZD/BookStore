namespace CarsRent.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CarsRentDB : DbContext
    {
        public CarsRentDB()
            : base("name=CarsRentDB")
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CanaelOrder> CanaelOrders { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Categroy> Categroys { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<SeatNum> SeatNums { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Car>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categroy>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.Categroy)
                .HasForeignKey(e => e.CategroyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SeatNum>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.SeatNum)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.OrderDetails)
            //    .WithRequired(e => e.User)
            //    .WillCascadeOnDelete(false);
        }
    }
}
