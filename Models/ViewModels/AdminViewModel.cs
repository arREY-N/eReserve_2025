namespace WebApplication1.Models.ViewModels
{
    public class AdminViewModel : UserViewModel
    {
        public List<User>? Users { get; set; }

        public AdminViewModel() : base()
        {

        }
    }
}
