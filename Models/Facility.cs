using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Facility
	{
		[Key]
		public int FacilityID { get; set; }
		public string? FacilityName { get; set; } = null!;
		public int Floor { get; set; }
		public int Number { get; set; }
		public string? Status { get; set; } = null!;
		public List<Reservation>? Reservations { get; set; }
		public Facility() { }
	}
}
