using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;
using SalonBooking.API.Models;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StylistsController : ControllerBase
{
    private readonly AppDbContext _db;
    public StylistsController(AppDbContext db) => _db = db;
    
    [HttpGet]
    public async Task<List<StylistDto>> GetAll()
    {
        return await _db.Stylists.Where(s => s.Status == "Active")
            .Include(s => s.User)
            .Include(s => s.Reviews)
            .Select(s => new StylistDto(
                s.Id, s.UserId, s.User!.Nickname, s.User.Avatar ?? "",
                s.Level, s.YearsOfExperience, s.Specialty, s.Description, s.Status,
                s.Reviews.Any() ? s.Reviews.Average(r => r.Rating) : 0,
                s.Reviews.Count
            )).ToListAsync();
    }
    
    [HttpGet("all")]
    [Authorize(Roles = "Manager")]
    public async Task<List<StylistDto>> GetAllAdmin()
    {
        return await _db.Stylists.Include(s => s.User).Select(s => new StylistDto(
            s.Id, s.UserId, s.User!.Nickname, s.User.Avatar ?? "",
            s.Level, s.YearsOfExperience, s.Specialty, s.Description, s.Status,
            s.Reviews.Any() ? s.Reviews.Average(r => r.Rating) : 0,
            s.Reviews.Count
        )).ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<StylistDto>> GetById(int id)
    {
        var s = await _db.Stylists.Include(x => x.User).Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return NotFound();
        return new StylistDto(s.Id, s.UserId, s.User!.Nickname, s.User.Avatar ?? "",
            s.Level, s.YearsOfExperience, s.Specialty, s.Description, s.Status,
            s.Reviews.Any() ? s.Reviews.Average(r => r.Rating) : 0,
            s.Reviews.Count);
    }
    
    [HttpGet("{id}/reviews")]
    public async Task<List<ReviewDto>> GetReviews(int id)
    {
        return await _db.Reviews.Where(r => r.StylistId == id)
            .Include(r => r.Customer)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewDto(r.Id, r.Customer!.Nickname, r.Rating, r.Content, r.CreatedAt))
            .ToListAsync();
    }
    
    [HttpGet("{id}/schedule")]
    public async Task<List<ScheduleDto>> GetSchedule(int id)
    {
        return await _db.StylistSchedules.Where(s => s.StylistId == id)
            .Select(s => new ScheduleDto(s.Id, s.StylistId, s.DayOfWeek,
                s.StartTime.ToString(@"hh\:mm"), s.EndTime.ToString(@"hh\:mm"), s.IsOff))
            .ToListAsync();
    }
    
    [HttpPut("{id}/schedule")]
    [Authorize(Roles = "Stylist,Manager")]
    public async Task<ActionResult> SetSchedule(int id, [FromBody] BatchScheduleRequest req)
    {
        var existing = await _db.StylistSchedules.Where(s => s.StylistId == id).ToListAsync();
        _db.StylistSchedules.RemoveRange(existing);
        
        foreach (var item in req.Items)
        {
            _db.StylistSchedules.Add(new StylistSchedule
            {
                StylistId = id, DayOfWeek = item.DayOfWeek,
                StartTime = TimeSpan.Parse(item.StartTime),
                EndTime = TimeSpan.Parse(item.EndTime),
                IsOff = item.IsOff
            });
        }
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> Create([FromBody] StylistCreateRequest req)
    {
        var user = new User
        {
            Username = $"stylist_{DateTime.Now.Ticks}",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Stylist", Nickname = req.Nickname, Phone = "",
            CreatedAt = DateTime.Now
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        var stylist = new Stylist
        {
            UserId = user.Id, Level = req.Level, YearsOfExperience = req.Years,
            Specialty = req.Specialty, Description = req.Description, Status = "Active"
        };
        _db.Stylists.Add(stylist);
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> ToggleStatus(int id)
    {
        var s = await _db.Stylists.FindAsync(id);
        if (s == null) return NotFound();
        s.Status = s.Status == "Active" ? "Inactive" : "Active";
        await _db.SaveChangesAsync();
        return Ok();
    }
}
