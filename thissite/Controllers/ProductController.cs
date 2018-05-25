using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using thissite.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace thissite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ThisSiteDbContext _context;

        public ProductController(ThisSiteDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Product p = _context.Products.Find(id.Value);
                return View(p);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Details(int id, int quantity = 1)
        {
            Guid cartId;
            Cart cart = null;
            if (Request.Cookies.ContainsKey("cartId"))
            {
                if (Guid.TryParse(Request.Cookies["cartId"], out cartId))
                {
                        cart = _context.Carts
                        .Include(Carts => Carts.CartItems)
                        .ThenInclude(CartItems => CartItems.Product)
                        .FirstOrDefault(x => x.CookieIdentifier == cartId);
                }
            }

            if (cart == null)
            {
                cart = new Cart();
                cartId = Guid.NewGuid();
                cart.CookieIdentifier = cartId;

                _context.Carts.Add(cart);
                Response.Cookies.Append("cartId", cartId.ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.UtcNow.AddYears(100) });

            }
            CartItem item = cart.CartItems.FirstOrDefault(x => x.Product.ID == id);
            if (item == null)
            {
                item = new CartItem();
                item.Product = _context.Products.Find(id);
                cart.CartItems.Add(item);
            }

            item.Quantity += quantity;
            cart.LastModified = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
    }
}

