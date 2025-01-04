using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserID { get; set; } 
        public User? User { get; set; }
        public string? Layout { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public UserViewModel() { }
    }
}