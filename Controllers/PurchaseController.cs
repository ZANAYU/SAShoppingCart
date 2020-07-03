using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using ShoppingCart.Models;
using ShoppingCart.Database;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly ILogger<PurchaseController> _logger;
        private ShoppingCartContext _dbContext;
        public PurchaseController(ILogger<PurchaseController> logger, ShoppingCartContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // TODO: display purchases
        // send purchases and activation code to view
        public IActionResult Index(string name)
        {
            var userId = HttpContext.User.Claims.First().Value;
            List<Order> orders = _dbContext.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.UtcDateTime).ToList();
            return View(orders);
        }
    }
}
