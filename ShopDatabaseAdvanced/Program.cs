using ShopDatabaseAdvanced.Model;
using ShopDatabaseAdvanced.Models;
using ShopDatabaseAdvanced.ShopAdvancedDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDatabaseAdvanced
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var db = new AdvancedShopDatabaseContext())
			{
                //1. Ask client's name and greet him or her by name.

                Console.WriteLine("What's your name?");

                var customerNameInput = Console.ReadLine();
                var customers = db.Customers;

                Console.WriteLine();
                Console.WriteLine($"Hello, {customerNameInput}!");

                //2. Determine if this client exists in the database.
                //3. If client doesn't exist, add him or her to the database (remember to save the changes!)

                Customer createNewCustomer = new Customer(customerNameInput);

                if (customers.FirstOrDefault(x => x.Name == customerNameInput) == null)
                {
                    db.Customers.Add(createNewCustomer);
                    db.SaveChanges();
                }

                //4. Create a new Shopping Cart and let your client do the shopping.

                ShoppingCart newCart = new ShoppingCart();
				db.ShoppingCarts.Add(newCart);

                ChooseFood(db, newCart);
				while (Console.ReadLine() == "y")
				{
					ChooseFood(db, newCart);
				}

				db.SaveChanges();

                //5. Show the contents of the current shopping cart together with the its total price.

                Console.WriteLine("Current cart: ");
                Console.WriteLine();
                foreach (var x in newCart.Items)
                {
                    Console.WriteLine($"{x.Name} - {x.Price} euros");
                }
                Console.WriteLine();
                Console.WriteLine($"Cart totals: {newCart.Sum} euros");

                //6. Add current Shopping Cart to the list of client's shopping carts and save the changes to database.

                Customer currentClient = db.Customers.FirstOrDefault(x => x.Name == customerNameInput);
                currentClient.AddToCustomer(newCart);
                db.SaveChanges();

                //7. Determine, how many times your current client has visited the shop (hint: count of his/her shopping carts' list).

                Console.WriteLine("Total visits to shop: " + currentClient.ShoppingCarts.Count());

                //8. If it's not the client's first time to visit the shop, ask him/her, if he/she wants to see his/her shopping history.
                // If yes, show all his/ her shopping carts together with contents and total prices.

                if (currentClient.ShoppingCarts.Count() > 1)
                {
                    Console.WriteLine("Do you want to see your shopping history? y/n");

                    if (Console.ReadLine() == "y")
                    {
                        var shoppings = currentClient.ShoppingCarts.OrderBy(x => x.DateCreated).ToList();

                        foreach (var cart in shoppings)
                        {
                            Console.WriteLine("Shoppingcart was created on: " + cart.DateCreated);
                            Console.WriteLine();
                            Console.WriteLine("Including:");
                            foreach (var x in cart.Items)
                            {
                                Console.Write(x.Name + " = " + x.Price + "euros");
                                Console.WriteLine();
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Shoppingcart totals: {cart.Sum} euros");
                        }
                    }
                }
                //9. Thank your client for visiting and wish him or her a nice day!
                Console.WriteLine();
                Console.WriteLine("Thank you for visiting our store, " + currentClient.Name + " Hope you have a nice day!");
            }
            Console.ReadKey();
		}
		private static void ChooseFood(AdvancedShopDatabaseContext db, ShoppingCart newCart)
		{
			Console.WriteLine("What do you want to buy?");
			string foodName = Console.ReadLine();
			Food chosenFood = db.Foods.FirstOrDefault(x => x.Name == foodName);
			newCart.AddToCart(chosenFood);
			Console.WriteLine("Anything else? y/n");
		}
	}
}
