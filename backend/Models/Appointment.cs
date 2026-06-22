using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class Appointment
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    
    [ForeignKey("Stylist")]
    public int? StylistId { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public DateTime AppointmentDate { get; set; }
    
    [Required, MaxLength(20)]
    public string TimeSlot { get; set; } = string.Empty;
    
    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending";
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [MaxLength(500)]
    public string? Note { get; set; }
    
    public User? Customer { get; set; }
    public User? Stylist { get; set; }
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
    public Review? Review { get; set; }
}
