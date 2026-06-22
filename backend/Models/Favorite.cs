using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class Favorite
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    
    [ForeignKey("Stylist")]
    public int StylistId { get; set; }
    
    public User? Customer { get; set; }
    public Stylist? Stylist { get; set; }
}
