using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace WebApplication1.Models
{
    public class Reservation
    {
        [Key]
        public int RequestID { get; set; }
		public DateTime RequestDateTime { get; set; }
		public DateTime ReserveDate { get; set; }
        public TimeSpan ReserveTimeStart { get; set; }
		public TimeSpan ReserveTimeEnd { get; set; }
        public string Purpose { get; set; } = null!;
        public string? Status { get; set; } = "Pending";
        public int FacilityID { get; set; }
        [ForeignKey("FacilityID")]
        public Facility? Facility { get; set; }
        
        public int UserID { get; set; }
        [ForeignKey("UserID")]
		public User? User { get; set; }
        
		public Reservation() { }
    }
}
