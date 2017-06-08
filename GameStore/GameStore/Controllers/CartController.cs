using GameStore.Helpers;
using GameStore.Models;
using System;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class CartController : Controller
    {
        private const string sessionCart = "Cart";
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var cart = Session.IsSet(sessionCart) ?
                Session.Get<Cart>(sessionCart) : new Cart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddProduct(int? productId, int quantity)
        {
            Product product = db.Products.Find(productId);
            if (quantity > 0 && product != null)
            {
                Cart cart = Session.Get<Cart>(sessionCart);
                if (cart == null)
                {
                    cart = new Cart();
                    Session.Set(sessionCart, cart);
                }
                cart.AddAmount(product, quantity);
                return Json(new
                {
                    left = product.Quantity - quantity,
                    cartTotal = cart.TotalProducts
                });
            }
            return Content(string.Empty);
        }

        [HttpPost]
        public ActionResult ChangeProductQuantity(int? productId, int quantity)
        {
            Product product = db.Products.Find(productId);
            if (product != null)
            {
                Cart cart = Session.Get<Cart>(sessionCart);
                if (cart == null)
                {
                    cart = new Cart();
                    Session.Set(sessionCart, cart);
                }
                cart.AddAmount(product, quantity);                
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveProduct(int? productId)
        {
            Product product = db.Products.Find(productId);
            Cart cart = Session.Get<Cart>(sessionCart);
            if (product != null && cart != null)
            {
                cart.Remove(product);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveAll()
        {
            Cart cart = Session.Get<Cart>(sessionCart);
            if (cart != null)
            {
                cart.Clear();
            }
            return RedirectToAction("Index");
        }
    }
}