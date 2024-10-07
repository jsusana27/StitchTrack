using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import models used for defining database entities

namespace CrochetBusinessAPI.Data
{
    //DbContext class to define and manage the database schema and relationships
    public class CrochetDbContext : DbContext
    {
        //Constructor that initializes the context with specific options
        public CrochetDbContext(DbContextOptions<CrochetDbContext> options) : base(options)
        {

        }

        public DbSet<Yarn> Yarns { get; set; }

        public DbSet<SafetyEye> SafetyEyes { get; set; }

        public DbSet<Stuffing> Stuffings { get; set; }

        public DbSet<FinishedProduct> FinishedProducts { get; set; }

        public DbSet<FinishedProductMaterial> FinishedProductMaterials { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerPurchase> CustomerPurchases { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        //Configure entity relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure the relationship between FinishedProduct and FinishedProductMaterial
            modelBuilder.Entity<FinishedProduct>()
                .HasMany(fp => fp.FinishedProductMaterials) //A FinishedProduct has many FinishedProductMaterials
                .WithOne(fpm => fpm.FinishedProduct) //Each FinishedProductMaterial is associated with one FinishedProduct
                .HasForeignKey(fpm => fpm.FinishedProductsID); //Define FinishedProductsID as the foreign key

            //Configure the relationship between FinishedProduct and CustomerPurchase
            modelBuilder.Entity<FinishedProduct>()
                .HasMany(fp => fp.CustomerPurchases) //A FinishedProduct has many CustomerPurchases
                .WithOne(cp => cp.FinishedProduct) //Each CustomerPurchase is associated with one FinishedProduct
                .HasForeignKey(cp => cp.FinishedProductsID); //Define FinishedProductsID as the foreign key

            //Configure the relationship between Customer and CustomerPurchase
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CustomerPurchases) //A Customer has many CustomerPurchases
                .WithOne(cp => cp.Customer) //Each CustomerPurchase is associated with one Customer
                .HasForeignKey(cp => cp.CustomerID); //Define CustomerID as the foreign key

            //Configure the relationship between Customer and Order
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders) //A Customer has many Orders
                .WithOne(o => o.Customer) //Each Order is associated with one Customer
                .HasForeignKey(o => o.CustomerID); //Define CustomerID as the foreign key

            //Configure the relationship between Order and OrderProduct
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderProducts) //An Order has many OrderProducts
                .WithOne(op => op.Order) //Each OrderProduct is associated with one Order
                .HasForeignKey(op => op.OrderID); //Define OrderID as the foreign key

            //Configure the relationship between OrderProduct and FinishedProduct
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.FinishedProduct) //Each OrderProduct is associated with one FinishedProduct
                .WithMany(fp => fp.OrderProducts) //A FinishedProduct can have many OrderProducts
                .HasForeignKey(op => op.FinishedProductsID); //Define FinishedProductsID as the foreign key
        }
    }
}