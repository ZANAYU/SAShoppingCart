using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(double total)
        {
            ViewData["Total"] = total;
            return View();
        }
        [HttpPost]
        public IActionResult Payment()
        {
            return RedirectToAction("PlaceOrder", "Cart");
        }
    }
}