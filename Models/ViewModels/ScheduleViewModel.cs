namespace WebApplication1.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public int FacilityID { get; set; } = 0;
        public List<TimeBlock>? Schedule { get; set; } = null;
        public ScheduleViewModel() { }
    }
}
