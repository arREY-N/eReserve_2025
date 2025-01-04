using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

		[Required(ErrorMessage = "Username is required.")]
		public string Username { get; set; } = null!;
		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; } = null!;
		[Required(ErrorMessage = "First Name is required.")]
		public string FirstName { get; set; } = null!;
		[Required(ErrorMessage = "Middle Name is required.")]
		public string MiddleName { get; set; } = null!;
		[Required(ErrorMessage = "Last Name is required.")]
		public string LastName { get; set; } = null!;
		[Required(ErrorMessage = "User number is required.")]
		public int UserNumber { get; set; }
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string UserEmail { get; set; } = null!;

        public int? DesignationID { get; set; }
        public Designation? Designation { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public User() { }
    }
}
