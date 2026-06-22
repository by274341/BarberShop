using System.ComponentModel.DataAnnotations;

namespace SalonBooking.API.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required, MaxLength(20)]
    public string Role { get; set; } = "Customer";
    
    [Required, MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Avatar { get; set; }
    
    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Stylist? StylistProfile { get; set; }
    public ICollection<Appointment> CustomerAppointments { get; set; } = new List<Appointment>();
    public ICollection<Appointment> StylistAppointments { get; set; } = new List<Appointment>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<UserCoupon> UserCoupons { get; set; } = new List<UserCoupon>();
}
