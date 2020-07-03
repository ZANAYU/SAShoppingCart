using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using ShoppingCart.Models;
using ShoppingCart.Database;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ShoppingCart.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private ShoppingCartContext _dbContext;
        private AuthHash _authHash;
        public AuthController(ILogger<AuthController> logger, ShoppingCartContext dbContext, AuthHash authHash)
        {
            _logger = logger;
            _dbContext = dbContext;
            _authHash = authHash;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
                return RedirectToAction("Index", "Product");
            else
                return View();
        }

        [HttpPost]
        public async Task<string> Login(string userName, string password, [FromBody]List<CartDetail> cartDetails)
        {
            password = _authHash.GetHash(password);
            bool isRegistered = _dbContext.Users.Where(user => user.UserId == userName && user.Password == password).Any();
            if (isRegistered == true)
            {
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name,userName)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                bool isMerged = MergeGuestCart(userName, cartDetails);
                if (isMerged)
                    return "/Cart/Index";
                else
                    return "/Product/Index";
            }
            else
                return "/Auth/Index";
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        [Authorize]
        public bool MergeGuestCart(string userId, List<CartDetail> cartDetails)
        {
            var userCart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            if (cartDetails != null && cartDetails.Count() != 0)
            {
                if (userCart != null)
                {
                    foreach (var guestCartDetail in cartDetails)
                    {
                        var product = _dbContext.Products.Where(p => p.ProductId == guestCartDetail.ProductId).FirstOrDefault();
                        var userCartDetail = userCart.CartDetails.Where(cd => cd.ProductId == guestCartDetail.ProductId).FirstOrDefault();
                        if (product != null && guestCartDetail.Qty >= 0)
                        {
                            if (userCartDetail != null)
                            {
                                userCartDetail.Qty += guestCartDetail.Qty;
                            }
                            else
                            {
                                userCart.CartDetails.Add(new CartDetail()
                                {
                                    CartId = userCart.CartId,
                                    ProductId = guestCartDetail.ProductId,
                                    Qty = guestCartDetail.Qty,
                                    Product = product
                                });
                            }
                        }
                    }
                    userCart.Total = userCart.CartDetails.Sum(cd => cd.Qty * cd.Product.Price);
                    _dbContext.Carts.Update(userCart);
                }
                else
                {
                    // create user cart
                    userCart = new Cart()
                    {
                        UserId = userId,
                        CartId = Guid.NewGuid().ToString(),
                        Total = 0,
                        CartDetails = new List<CartDetail>()
                    };
                    foreach (var guestCartDetail in cartDetails)
                    {
                        var product = _dbContext.Products.Where(p => p.ProductId == guestCartDetail.ProductId).FirstOrDefault();
                        var qty = guestCartDetail.Qty;
                        if (product != null && qty >= 0)
                        {
                            userCart.CartDetails.Add(
                                new CartDetail
                                {
                                    CartId = userCart.CartId,
                                    ProductId = product.ProductId,
                                    Qty = qty,
                                    Product = product
                                });
                        }
                    }
                    userCart.Total = userCart.CartDetails.Sum(cd => cd.Qty * cd.Product.Price);
                    _dbContext.Carts.Add(userCart);
                }
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
