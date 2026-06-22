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
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AppointmentsController(AppDbContext db) => _db = db;
    
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Create([FromBody] AppointmentCreateRequest req)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var services = await _db.Services.Where(s => req.ServiceIds.Contains(s.Id)).ToListAsync();
        var totalPrice = services.Sum(s => s.Price);
        
        var now = DateTime.Now;
        foreach (var svc in services)
        {
            var promo = await _db.Promotions.FirstOrDefaultAsync(p =>
                p.ServiceId == svc.Id && p.IsActive && p.StartTime <= now && p.EndTime >= now);
            if (promo != null)
                totalPrice -= svc.Price * (1 - promo.DiscountRate);
        }
        
        var appointment = new Appointment
        {
            CustomerId = userId, StylistId = req.StylistId,
            TotalPrice = totalPrice, AppointmentDate = req.AppointmentDate,
            TimeSlot = req.TimeSlot, Status = "Pending", CreatedAt = DateTime.Now, Note = req.Note
        };
        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();
        
        foreach (var sid in req.ServiceIds)
            _db.AppointmentServices.Add(new AppointmentService { AppointmentId = appointment.Id, ServiceId = sid });
        await _db.SaveChangesAsync();
        
        return Ok(new { appointment.Id });
    }
    
    [HttpGet("my")]
    public async Task<List<AppointmentDto>> GetMy()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        
        IQueryable<Appointment> query = _db.Appointments
            .Include(a => a.Customer).Include(a => a.Stylist)
            .Include(a => a.AppointmentServices).ThenInclude(x => x.Service);
        
        query = role switch
        {
            "Customer" => query.Where(a => a.CustomerId == userId),
            "Stylist" => query.Where(a => a.StylistId == userId),
            _ => query
        };
        
        return await query.OrderByDescending(a => a.AppointmentDate).Select(a => ToDto(a)).ToListAsync();
    }
    
    [HttpGet("today")]
    [Authorize(Roles = "Stylist,Manager")]
    public async Task<List<AppointmentDto>> GetToday()
    {
        var today = DateTime.Today;
        var query = _db.Appointments
            .Include(a => a.Customer).Include(a => a.Stylist)
            .Include(a => a.AppointmentServices).ThenInclude(x => x.Service)
            .Where(a => a.AppointmentDate.Date == today);
        
        if (User.FindFirstValue(ClaimTypes.Role) == "Stylist")
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            query = query.Where(a => a.StylistId == userId);
        }
        
        return await query.OrderBy(a => a.TimeSlot).Select(a => ToDto(a)).ToListAsync();
    }
    
    [HttpPut("{id}/confirm")]
    [Authorize(Roles = "Stylist,Manager")]
    public async Task<ActionResult> Confirm(int id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null) return NotFound();
        appt.Status = "Confirmed";
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Stylist")]
    public async Task<ActionResult> Reject(int id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null) return NotFound();
        appt.Status = "Rejected";
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> Cancel(int id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null) return NotFound();
        appt.Status = "Cancelled";
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPut("{id}/complete")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> Complete(int id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt == null) return NotFound();
        appt.Status = "Completed";
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("admin-all")]
    [Authorize(Roles = "Manager")]
    public async Task<List<AppointmentDto>> GetAllAdmin([FromQuery] DateTime? date, [FromQuery] int? stylistId, [FromQuery] string? status)
    {
        var query = _db.Appointments
            .Include(a => a.Customer).Include(a => a.Stylist)
            .Include(a => a.AppointmentServices).ThenInclude(x => x.Service)
            .AsQueryable();
        
        if (date.HasValue) query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);
        if (stylistId.HasValue) query = query.Where(a => a.StylistId == stylistId);
        if (!string.IsNullOrEmpty(status)) query = query.Where(a => a.Status == status);
        
        return await query.OrderByDescending(a => a.AppointmentDate).Select(a => ToDto(a)).ToListAsync();
    }
    
    private static AppointmentDto ToDto(Appointment a) => new(
        a.Id, a.Customer?.Nickname ?? "", a.Stylist?.Nickname,
        string.Join(",", a.AppointmentServices.Select(x => x.Service?.Name ?? "")),
        a.TotalPrice, a.AppointmentDate, a.TimeSlot, a.Status, a.CreatedAt, a.Note
    );
}
