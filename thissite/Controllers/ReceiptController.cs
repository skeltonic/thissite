using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using thissite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Braintree;

namespace thissite.Controllers
{
    public class ReceiptController : Controller
    {
        private ThisSiteDbContext _thisSiteDbContext;
        private EmailService _emailService;
        private SignInManager<ThisSiteUser> _signInManager;
        private BraintreeGateway _brainTreeGateway;


        public ReceiptController(ThisSiteDbContext thisSiteDbContext,
            EmailService emailService,
            SignInManager<ThisSiteUser> signInManager,
            BraintreeGateway braintreeGateway)
        {
            this._thisSiteDbContext = thisSiteDbContext;
            this._emailService = emailService;
            this._signInManager = signInManager;
            this._brainTreeGateway = braintreeGateway;
        }

        public ActionResult Index(string id)
        {
            
            return View(_thisSiteDbContext.Orders.Single(x => x.TrackingNumber == id));
        }
    }

        
    
}