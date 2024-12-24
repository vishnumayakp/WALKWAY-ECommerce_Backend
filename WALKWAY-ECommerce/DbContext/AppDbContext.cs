using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WALKWAY_ECommerce.Models;
using WALKWAY_ECommerce.Models.Address_Model;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Order_Model;
using WALKWAY_ECommerce.Models.Product_Model;
using WALKWAY_ECommerce.Models.User_Model;
using WALKWAY_ECommerce.Models.WishList_Model;
namespace WALKWAY_ECommerce.DbContexts
{
    public class AppDbContext:DbContext
    {
       private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderMain> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasDefaultValue("user");

            modelBuilder.Entity<User>()
                .Property(b=>b.IsBlocked)
                .HasDefaultValue(false);


            modelBuilder.Entity<Category>()
                .HasMany(p=>p.Products)
                .WithOne(c=>c.Category)
                .HasForeignKey(c=>c.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(ci => ci.CategoryId);

            modelBuilder.Entity<WishList>()
               .HasOne(p => p.Product)
               .WithMany()
               .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<WishList>()
                .HasOne(u => u.User)
                .WithMany(w => w.WishLists)
                .HasForeignKey(u => u.UserId);


            modelBuilder.Entity<User>()
                .HasOne(u=>u.Cart)
                .WithOne(c=>c.User)
                .HasForeignKey<Cart>(c=>c.UserId);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci=>ci.Product)
                .WithMany(p=>p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            modelBuilder.Entity<Address>()
                .HasOne(u => u.User)
                .WithMany(ad => ad.Addresses)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<OrderMain>()
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<OrderMain>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<OrderMain>()
                .HasOne(a => a.Address)
                .WithMany(o => o.Orders)
                .HasForeignKey(f => f.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(f => f.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(f => f.productId);

            modelBuilder.Entity<OrderItem>()
                .Property(pr => pr.TotalPrice)
                .HasPrecision(30, 2);

            modelBuilder.Entity<OrderMain>()
                .Property(o => o.OrderStatus)
                .HasDefaultValue("pending");

        }


    }
}
