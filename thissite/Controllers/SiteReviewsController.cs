﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace thissite.Controllers
{
    public class SiteReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}