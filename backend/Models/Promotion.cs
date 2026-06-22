using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class Promotion
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [ForeignKey("Service")]
    public int ServiceId { get; set; }
    
    public decimal DiscountRate { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public Service? Service { get; set; }
}
