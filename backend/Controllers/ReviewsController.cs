using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;
using SalonBooking.API.Models;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ReviewsController(AppDbContext db) => _db = db;
    
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Create([FromBody] ReviewCreateRequest req)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var appt = await _db.Appointments.FindAsync(req.AppointmentId);
        if (appt == null || appt.CustomerId != userId) return BadRequest();
        if (appt.Status != "Completed") return BadRequest(new { message = "只能评价已完成的订单" });
        if (await _db.Reviews.AnyAsync(r => r.AppointmentId == req.AppointmentId))
            return BadRequest(new { message = "已评价过" });
        
        var review = new Review
        {
            AppointmentId = req.AppointmentId, CustomerId = userId,
            StylistId = appt.StylistId ?? 0, Rating = req.Rating, Content = req.Content, CreatedAt = DateTime.Now
        };
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
