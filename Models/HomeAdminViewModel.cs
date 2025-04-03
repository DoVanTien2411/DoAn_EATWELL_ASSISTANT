namespace EatWellAssistant.Models
{
    public class HomeAdminViewModel
    {
        public int totalFood { get; set; }
        public int totalUser { get; set; }
        public int totalPlan { get; set; }
        public HomeAdminViewModel()
        {
            this.totalFood = 0;
            this.totalUser = 0;
            this.totalPlan = 0;
        }
        public HomeAdminViewModel(int p, int o, int r)
        {
            this.totalPlan = r;
            this.totalFood = p;
            this.totalUser = o;
        }
    }
}
