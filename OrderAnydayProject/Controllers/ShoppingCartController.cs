using Microsoft.AspNet.Identity;
using OrderAnydayProject.Models;
using OrderAnydayProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderAnydayProject.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        OrderAnyDayContext storeDB = new OrderAnyDayContext();

        //         // GET: /ShoppingCart/ 

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel             
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view             
            return View(viewModel);
        } 

            //         // GET: /Store/AddToCart/5 

        public ActionResult AddToCart(int id)
        {

            // Retrieve the product from the database             
            var addedProduct = storeDB.Products.Single(prod => prod.ItemNumber == id);

            // Add it to the shopping cart             
            var cart = ShoppingCart.GetCart(this.HttpContext); 

            cart.AddToCart(addedProduct);

            TempData["success"] = addedProduct.ItemName +
                        " has been added to your shopping cart.";

            return Redirect(Request.UrlReferrer.ToString());         } 

            //         
            // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {             // Remove the item from the cart             
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the product to display confirmation             
            string productName = storeDB.Carts.Single(item => item.Id == id).Product.ItemName;

            // Remove from cart             
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message             
            var results = new ShoppingCartRemoveViewModel
            {
                //Message = Server.HtmlEncode(productName) + " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            TempData["Removed"] = Server.HtmlEncode(productName) +
                      " has been removed from your shopping cart.";
            return Json(results);
        }

        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the product to display confirmation 
            string productName = storeDB.Carts
                .Single(item => item.Id == id).Product.ItemName;

            // Update the cart count 
            int itemCount = cart.UpdateCartCount(id, cartCount);

            //Prepare messages
            //string msg = "The quantity of " + Server.HtmlEncode(productName) +
            //        " has been updated in your shopping cart.";
            //if (itemCount == 0) msg = Server.HtmlEncode(productName) +
            //        " has been removed from your shopping cart.";
            if (itemCount == 0)
            {
                TempData["Removed"] = Server.HtmlEncode(productName) +
                      " has been removed from your shopping cart.";
            }
            else
            {
                TempData["success"] = "The quantity of " + Server.HtmlEncode(productName) +
                        " has been updated in your shopping cart.";
            }
            var results = new ShoppingCartRemoveViewModel
            {
                //Message = msg,
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            //Prepare messages

            return Json(results);
            
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
           
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var cartsize = cart.GetCartItems();
            if (cartsize.Count() > 0)
            {
                Order order = new Order();
                order.UserId = User.Identity.GetUserId();
                order.Last_Active = DateTime.Now;
                order.Placed = DateTime.Now;
                order.IsActive = "p";

                int orderNum = cart.SubmitOrder(order);
                TempData["Success"] = "The Order Was Successfully Placed.";//create message on successful Order Placement
                return RedirectToAction("Profile", "Home");
            }
            else
            {
                TempData["Error"] = "Your Cart Is Empty!";//create message on empty cart order attempt
                return RedirectToAction("Index");
            }
        }

        //         // GET: /ShoppingCart/CartSummary 

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}