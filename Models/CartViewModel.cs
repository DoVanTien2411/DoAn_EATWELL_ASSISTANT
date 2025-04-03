using EatWellAssistant.Models.Entities;

namespace EatWellAssistant.Models
{
    public class CartViewModel
    {
        public Cart cart { get; set; }
        public CartItems cartItem { get; set; }

        public CartViewModel(Cart cart, CartItems cartItem)
        {
            this.cart = cart;
            this.cartItem = cartItem;
        }
    }
}
