using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonBooking.API.Models;

public class AppointmentService
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }
    
    [ForeignKey("Service")]
    public int ServiceId { get; set; }
    
    public Appointment? Appointment { get; set; }
    public Service? Service { get; set; }
}
