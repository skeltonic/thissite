using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace thissite.Models
{
    public class ThisSiteDbContext : IdentityDbContext<ThisSiteUser>
    {
        public ThisSiteDbContext() : base()
        {

        }

        public ThisSiteDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Orderitems { get; set; }

    }

    public class ThisSiteUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class Cart
    {
        public Cart()
        {
            this.CartItems = new HashSet<CartItem>();
        }

        public int ID { get; set; }
        public Guid CookieIdentifier { get; set; }
        public DateTime LastModified { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

    }

    public class CartItem
    {
        public int ID { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }


    }

    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int ID { get; set; }
        public string TrackingNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string Region { get; set; }
        public string Locale { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }

    public class OrderItem
    {
        public int ID { get; set; }
        public Order Order { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}


