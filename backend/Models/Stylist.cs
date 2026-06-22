using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class Stylist
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [Required, MaxLength(20)]
    public string Level { get; set; } = "初级";
    
    public int YearsOfExperience { get; set; }
    
    [MaxLength(200)]
    public string Specialty { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required, MaxLength(20)]
    public string Status { get; set; } = "Active";
    
    public User? User { get; set; }
    public ICollection<StylistSchedule> Schedules { get; set; } = new List<StylistSchedule>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Favorite> FavoritedBy { get; set; } = new List<Favorite>();
}
