using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class StylistSchedule
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Stylist")]
    public int StylistId { get; set; }
    
    public int DayOfWeek { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public bool IsOff { get; set; } = false;
    
    public Stylist? Stylist { get; set; }
}
