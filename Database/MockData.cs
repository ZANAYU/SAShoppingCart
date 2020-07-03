using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Database;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ShoppingCart.Database
{
    public class MockData
    {
        public MockData(ShoppingCartContext dbcontext, AuthHash authHash)
        {
            // Generate hard-coded 4 users sharing 123 as password
            User user = new User();
            user.UserId = "John";
            user.Password = "202cb962ac59075b964b07152d234b70";

            user.Password = authHash.GetHash(user.Password);
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = "Mary";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = "Chris";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = "Martini";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            // Generate hard-coded 6 products
            Product product = new Product();
            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Charts";
            product.Description = "Brings a powerful charting capabilities to your .Net applications.";
            product.Price = 99;
            product.Image = "NetCharts.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Paypal";
            product.Description = "Integrate your .Net apps with Paypal the easy way!";
            product.Price = 69;
            product.Image = "NetPaypal.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net ML";
            product.Description = "Supercharged .Net machine learning libraries.";
            product.Price = 299;
            product.Image = "NetML.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Analytics";
            product.Description = "Perform data mining and analytics easily in .Net.";
            product.Price = 299;
            product.Image = "NetAnalytics.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Logger";
            product.Description = "Logs and aggregates events easily in your .Net apps.";
            product.Price = 49;
            product.Image = "NetLogger.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Numerics";
            product.Description = "Powerful numerical methods for your .Net simulations.";
            product.Price = 199;
            product.Image = "NetNumerics.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Directory";
            product.Description = "Build highly secure applications with single sign-on and multi-factor authentication.";
            product.Price = 139;
            product.Image = "NetDirectory.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Cognition";
            product.Description = "Comprehensive AI services and APIs to help you deploy cognitive services.";
            product.Price = 699;
            product.Image = "NetCognition.png";
            dbcontext.Add(product);
            dbcontext.SaveChanges();
        }
    }
}
