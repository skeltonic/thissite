using System;
using System.Linq;
using thissite.Models;
using Microsoft.EntityFrameworkCore;

namespace thissite

{
    internal static class DbInitializer
    {
        internal static void Initialize(this ThisSiteDbContext db)
        {
 
            db.Database.Migrate();

            if (db.Products.Count() == 0)
            {
                db.Products.Add(new Product
                {
                    Description = "This hat sucks.",
                    Image = "/images/hat1.jpg",
                    Name = "This Site Sucks, the Hat",
                    Price = 25m
                });
                db.Products.Add(new Product
                {
                    Description = "This shirt sucks.",
                    Image = "/images/shirt1.jpg",
                    Name = "This Site Sucks, the Shirt",
                    Price = 20m
                });
                db.Products.Add(new Product
                {
                    Description = "<div style=\"align:center\">",
                    Image = "/images/shirtcss1.jpg",
                    Name = "CSS Sucks, The Shirt",
                    Price = 20m
                });
                db.Products.Add(new Product
                {
                    Description = "Like your dreams, this shirt is broken.",
                    Image = "/images/brokenshirt.jpg",
                    Name = "Broken Shirt, The Shirt",
                    Price = 20m
                });


                db.SaveChanges();
            }

        }
    }
}