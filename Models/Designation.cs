using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Designation
	{
		[Key]
		public int DesignationID { get; set; }
		public string DesignationName { get; set; } = null!;
		public List<User>? Users { get; set; }

		public Designation() { }
	}
}
