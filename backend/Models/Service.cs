using System.ComponentModel.DataAnnotations;

namespace SalonBooking.API.Models;

public class Service
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required, MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public int Duration { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(200)]
    public string? Image { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public bool IsDiscountable { get; set; } = true;
    
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
    public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
