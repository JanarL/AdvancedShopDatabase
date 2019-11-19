using ShopDatabaseAdvanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDatabaseAdvanced.Model
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public Customer()
        {
        }

        public Customer(string name)
        {
            Id = Guid.NewGuid();
            ShoppingCarts = new List<ShoppingCart>();
            Name = name;
        }

        internal void AddToCustomer(ShoppingCart cart)
        {
            ShoppingCarts.Add(cart);
        }

    }
}
