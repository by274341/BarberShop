using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class StylistTag
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Stylist")]
    public int StylistId { get; set; }
    
    [Required, MaxLength(50)]
    public string Tag { get; set; } = string.Empty;
    
    public Stylist? Stylist { get; set; }
}
