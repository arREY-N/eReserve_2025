namespace WebApplication1.Models.ViewModels
{
    public class FacilityViewModel : UserViewModel
    {
        List<Facility>? Facilities { get; set; } = null!;

        public FacilityViewModel() : base()
        { }
    }
}
