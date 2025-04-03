using EatWellAssistant.Models.Entities;
using X.PagedList;

namespace EatWellAssistant.Models
{
    public class MaterialViewModel
    {
        public X.PagedList.IPagedList<Food> foods { get; set; }
        public ICollection<Category> categories { get; set; }
        public Cart cart { get; set; }
        public int pageNumber { get; set; }
        public int cate { get; set; }

        public MaterialViewModel(X.PagedList.IPagedList<Food> f, ICollection<Category> c, int pageNumber, int cate, Cart carts)
        {
            foods = f;
            categories = c;
            this.pageNumber = pageNumber;
            this.cate = cate;
            this.cart = carts;
        }
    }
}
