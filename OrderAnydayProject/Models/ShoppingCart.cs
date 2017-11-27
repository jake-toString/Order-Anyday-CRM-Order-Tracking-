using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderAnydayProject.Models
{
    public partial class ShoppingCart
    {
        OrderAnyDayContext DB = new OrderAnyDayContext();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context); return cart;
        }

        // Helper method for shopping cart calls         
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        } 

        public void AddToCart(Product product)
        {             
            // Get the matching cart and product instances             
            var cartItem = DB.Carts.SingleOrDefault( c => c.CartId == ShoppingCartId  && c.ItemNumber == product.ItemNumber);
            var request = HttpContext.Current.Request;
            int getcount = Convert.ToInt32(request["quantity"]);
            if (cartItem == null)
            {                 
                // Create a new cart item if no cart item exists                 
                cartItem = new Cart
                {
                    ItemNumber = product.ItemNumber,
                    CartId = ShoppingCartId,
                    Count = getcount,
                    DateCreated = DateTime.Now
                }; 

                DB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add quantity entered to the quantity                 
                cartItem.Count += getcount;
            } 

                // Save changes             
                DB.SaveChanges();
        } 

        public int RemoveFromCart(int id)
        {             
            // Get the cart             
            var cartItem = DB.Carts.Single( cart => cart.CartId == ShoppingCartId  && cart.Id == id);

            //int itemCount = 0;

            if (cartItem != null)
            {
                //if (cartItem.Count > 1)
                //{
                //    cartItem.Count--;
                //    itemCount = cartItem.Count;
                //} else {
                //    DB.Carts.Remove(cartItem);
                //}
                cartItem.Count = 0;
                DB.Carts.Remove(cartItem);
                DB.SaveChanges();
            } 

                return 0;
            }


        public void EmptyCart()
        {
            var cartItems = DB.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems) { DB.Carts.Remove(cartItem); }
            
            DB.SaveChanges();
        } 

        public List<Cart> GetCartItems() { return DB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList(); }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up             
            int? count = (from cartItems in DB.Carts where cartItems.CartId == ShoppingCartId select (int?)cartItems.Count).Sum();

            // Return 0 if all entries are null             
            return count ?? 0;
        } 

        public decimal GetTotal()
        {
            // Multiply product price by count of product to get the cart total
            var cart = (from cartItems in DB.Carts where cartItems.CartId == ShoppingCartId select (cartItems));
            decimal uPrice = 0;
            decimal ordertotal = 0;
            decimal total = 0;
            foreach (Cart c in cart)
            {
                uPrice = Convert.ToDecimal(c.Product.Price);
                total = uPrice * c.Count;
                ordertotal += total;
            }
            
            return ordertotal;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            // Iterate over the items in the cart, add order details for each             
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderItem
                {
                    ItemNumber = item.ItemNumber,
                    OrderNumber = order.OrderNumber,
                    //Price = item.Product.Price,
                    Quantity = item.Count
                };

                decimal UnitPrice = Convert.ToDecimal(item.Product.Price);
                // Set the order total of the shopping cart                 
                orderTotal += (item.Count * UnitPrice);

                DB.OrderItems.Add(orderDetail);

        }

            // Set the order's total to the orderTotal count             
            //order.Total = orderTotal;

            // Save the order             
            DB.SaveChanges();

            // Empty the shopping cart             
            EmptyCart();

            // Return the OrderId as the confirmation number             
            return order.OrderNumber;
        }

        public int UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cartItem = DB.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.Id == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartCount > 0)
                {
                    cartItem.Count = cartCount;
                    itemCount = cartItem.Count;
                }
                else
                {
                    DB.Carts.Remove(cartItem);
                }
                // Save changes 
                DB.SaveChanges();
            }
            return itemCount;
        }

        public int SubmitOrder(Order order)
        {
            //var cart = (from cartItems in DB.Carts where cartItems.CartId == ShoppingCartId select (cartItems));

            //decimal orderTotal = 0;

            var cartItems = GetCartItems();
            //Order order = new Order();

            // Iterate over the items in the cart, add order details for each             
            foreach (Cart item in cartItems)
            {
                var orderitem = new OrderItem
                {
                    ItemNumber = item.ItemNumber,
                    OrderNumber = order.OrderNumber,
                    Quantity = item.Count,
                    VendorID = item.Product.VendorID,
                    
                };
                //order.OrderItems.Add(orderitem);
                //decimal UnitPrice = Convert.ToDecimal(item.Product.Price);
                //// Set the order total of the shopping cart                 
                //orderTotal += (item.Count * UnitPrice);

                DB.OrderItems.Add(orderitem);

            }

            // Set the order's total to the orderTotal count             
            //order.Total = orderTotal;
            DB.Orders.Add(order);
            // Save the order             
            DB.SaveChanges();

            // Empty the shopping cart             
            EmptyCart();

            // Return the OrderId as the confirmation number             
            return order.OrderNumber;
        }

        // Use HttpContextBase to allow access to cookies.         
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                } else {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie                     
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            } 

                    return context.Session[CartSessionKey].ToString();
                }

        // When a user has logged in, migrate their shopping cart to         
        // be associated with their username         
        public void MigrateCart(string userName)
        {
            var shoppingCart = DB.Carts.Where(c => c.CartId == ShoppingCartId);
            
            foreach (Cart item in shoppingCart) { item.CartId = userName; }
            DB.SaveChanges();
            }
        }
    }