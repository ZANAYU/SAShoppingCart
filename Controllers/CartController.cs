using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.Models;
using ShoppingCart.Database;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private ShoppingCartContext _dbContext;
        public CartController(ILogger<CartController> logger, ShoppingCartContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public string GetCurrentUser()
        {
            return HttpContext.User.Claims.First().Value;
        }
        public IActionResult Index()
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                string userId = GetCurrentUser();
                Cart cart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
                ViewData["isLoggedIn"] = true;
                return View(cart);
            }
            else
            {
                ViewData["isLoggedIn"] = false;
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(string productId)
        {
            string userId = GetCurrentUser();
            var product = _dbContext.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (product != null)
            {
                var price = product.Price;
                var currentCart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
                var totalQty = 1;
                if (currentCart != null)
                {
                    currentCart.Total += price;
                    var currentCartDetail = _dbContext.CartDetails.Where(cd => cd.CartId == currentCart.CartId && cd.ProductId == productId).FirstOrDefault();
                    if (currentCartDetail != null)
                    {
                        currentCartDetail.Qty++;
                    }
                    else
                    {
                        currentCart.CartDetails.Add(new CartDetail
                        {
                            CartId = currentCart.CartId,
                            ProductId = productId,
                            Qty = 1
                        });
                    }
                    totalQty = currentCart.CartDetails.Sum(cd => cd.Qty);
                    _dbContext.Carts.Update(currentCart);
                }
                else
                {
                    string cartId = Guid.NewGuid().ToString();
                    _dbContext.Carts.Add(new Cart()
                    {
                        UserId = userId,
                        CartId = cartId,
                        Total = price,
                        CartDetails = new List<CartDetail>() {
                            new CartDetail
                            {
                                CartId = cartId,
                                ProductId = productId,
                                Qty = 1
                            }
                        }
                    });
                }
                _dbContext.SaveChanges();
                return Json(totalQty);
            }
            else
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        [HttpPost]
        public void UpdateQty(string productId, int changeTo)
        {
            if (changeTo >= 0)
            {
                var userId = GetCurrentUser();
                var cart = _dbContext.Carts.Where(c => c.UserId == userId).First();
                var cartDetail = _dbContext.CartDetails.Where(cd => cd.CartId == cart.CartId && cd.ProductId == productId).FirstOrDefault();
                if (cartDetail != null)
                {
                    cart.Total -= cartDetail.Product.Price * (cartDetail.Qty - changeTo);
                    if (changeTo > 0)
                        cartDetail.Qty = changeTo;
                    else
                        _dbContext.CartDetails.Remove(cartDetail);
                }
                _dbContext.SaveChanges();
            }
            return;
        }

        [Authorize]
        [HttpPost]
        public void ClearAll()
        {
            var userId = GetCurrentUser();
            var cart = _dbContext.Carts.Where(c => c.UserId == userId).First();
            cart.Total = 0;
            cart.CartDetails.Clear();
            _dbContext.SaveChanges();
        }

        [Authorize]
        public IActionResult Checkout()
        {
            double total = 0;
            var userId = GetCurrentUser();
            var userCart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            if (userCart != null)
                total = userCart.Total;
            if (total != 0)
                return RedirectToAction("Index", "Payment", new { total = total });
            else
                return RedirectToAction("Index", "Cart");
        }

        [Authorize]
        public IActionResult PlaceOrder()
        {
            CreateOrder();
            return RedirectToAction("Index", "Purchase");
        }

        [Authorize]
        public void CreateOrder()
        {
            var userId = GetCurrentUser();
            var cart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            if (cart != null)
            {
                // 1. create order
                Order order = new Order()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    UtcDateTime = DateTime.Now.ToUniversalTime(),
                    OrderDetails = new List<OrderDetail>(),
                };

                foreach (var cartDetail in cart.CartDetails)
                {
                    var activations = new List<Activation>();
                    for (int i = 0; i < cartDetail.Qty; i++)
                    {
                        activations.Add(new Activation()
                        {
                            ProductId = cartDetail.ProductId,
                            Code = Guid.NewGuid().ToString(),
                            OrderId = order.OrderId
                        });
                    }
                    order.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = cartDetail.ProductId,
                        Qty = cartDetail.Qty,
                        Activations = activations
                    });
                }
                _dbContext.Orders.Add(order);
                _dbContext.Carts.Remove(cart);
                _dbContext.SaveChanges();
            }
        }
    }
}
