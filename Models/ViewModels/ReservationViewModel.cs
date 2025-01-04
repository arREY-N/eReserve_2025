using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models.ViewModels
{
    public class ReservationViewModel : UserViewModel
    {
        public int? RequestID { get; set; }
        [Required]
        public DateTime RequestDateTime { get; set; }
        [Required]
        public DateTime ReserveDate { get; set; }
        [Required]
        public TimeSpan ReserveTimeStart { get; set; }
        [Required]
        public TimeSpan ReserveTimeEnd { get; set; }
        [Required]
        public string Purpose { get; set; } = null!;
        [Required]
        public int FacilityID { get; set; }

        public ReservationViewModel() : base()
        {
            RequestDateTime = DateTime.Now;
            ReserveDate = DateTime.Now.Date;
            ReserveTimeStart = DateTime.Now.TimeOfDay;
            ReserveTimeEnd = DateTime.Now.AddHours(1).TimeOfDay;
        }
    }
}
