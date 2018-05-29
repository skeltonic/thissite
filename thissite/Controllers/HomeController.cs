using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using thissite.Models;
using Microsoft.EntityFrameworkCore;

namespace thissite.Controllers
{
    public class HomeController : Controller
    {

        ThisSiteDbContext _db;

        public HomeController(ThisSiteDbContext thisSiteDbContext)
        {
            _db = thisSiteDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CartSummary()
        {
            
            Guid cartId;
            Cart cart = null;
            if (Request.Cookies.ContainsKey("cartId"))
            {
                if (Guid.TryParse(Request.Cookies["cartId"], out cartId))
                {
                    cart = await _db.Carts
                        .Include(carts => carts.CartItems)
                        .ThenInclude(cartitems => cartitems.Product)
                        .FirstOrDefaultAsync(x => x.CookieIdentifier == cartId);
                }
            }
            if (cart == null)
            {
                cart = new Cart();
            }
            return Json(cart);
        }

        public async Task<IActionResult> Search(string id)
        {
            return Json(_db.Products.Where(x => x.Description.Contains(id) || x.Name.Contains(id)).ToList());
        }

    }
}
