using System.ComponentModel.DataAnnotations;

namespace SalonBooking.API.Models;

public class Coupon
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public decimal FaceValue { get; set; }
    
    public decimal MinAmount { get; set; }
    
    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }
    
    public int TotalCount { get; set; }
    
    public int IssuedCount { get; set; }
    
    public ICollection<UserCoupon> UserCoupons { get; set; } = new List<UserCoupon>();
}
