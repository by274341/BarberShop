using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }
    
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    
    [ForeignKey("Stylist")]
    public int StylistId { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [MaxLength(500)]
    public string? Content { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Appointment? Appointment { get; set; }
    public User? Customer { get; set; }
    public Stylist? Stylist { get; set; }
}
