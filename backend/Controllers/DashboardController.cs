using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) => _db = db;
    
    [HttpGet]
    public async Task<DashboardDto> Get()
    {
        var today = DateTime.Today;
        var todayAppts = await _db.Appointments.Where(a => a.AppointmentDate.Date == today).ToListAsync();
        var todayCount = todayAppts.Count;
        var todayRevenue = todayAppts.Where(a => a.Status == "Completed").Sum(a => a.TotalPrice);
        var activeStylists = await _db.Stylists.CountAsync(s => s.Status == "Active");
        var pendingCount = await _db.Appointments.CountAsync(a => a.Status == "Pending");
        
        var weeklyStats = new List<DailyStat>();
        for (int i = 6; i >= 0; i--)
        {
            var date = today.AddDays(-i);
            var dayAppts = await _db.Appointments.Where(a => a.AppointmentDate.Date == date.Date).ToListAsync();
            weeklyStats.Add(new DailyStat(date.ToString("MM-dd"), dayAppts.Count,
                dayAppts.Where(a => a.Status == "Completed").Sum(a => a.TotalPrice)));
        }
        
        return new DashboardDto(todayCount, todayRevenue, activeStylists, pendingCount, weeklyStats, new());
    }
    
    [HttpGet("performance")]
    public async Task<List<StylistPerformanceDto>> GetPerformance()
    {
        return await _db.Stylists.Include(s => s.User).Select(s => new StylistPerformanceDto(
            s.Id, s.User!.Nickname,
            s.Appointments.Count(a => a.Status == "Completed"),
            s.Appointments.Where(a => a.Status == "Completed").Sum(a => a.TotalPrice)
        )).OrderByDescending(s => s.TotalRevenue).ToListAsync();
    }
    
    [HttpGet("service-stats")]
    public async Task<List<object>> GetServiceStats()
    {
        return await _db.AppointmentServices
            .Include(x => x.Service)
            .Where(x => x.Appointment!.Status == "Completed")
            .GroupBy(x => x.Service!.Name)
            .Select(g => new { name = g.Key, value = g.Count() })
            .ToListAsync<object>();
    }
    
    [HttpGet("revenue")]
    public async Task<List<DailyStat>> GetRevenue([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        start ??= DateTime.Today.AddDays(-30);
        end ??= DateTime.Today;
        
        var result = new List<DailyStat>();
        for (var d = start.Value; d <= end.Value; d = d.AddDays(1))
        {
            var dayAppts = await _db.Appointments
                .Where(a => a.AppointmentDate.Date == d.Date && a.Status == "Completed").ToListAsync();
            result.Add(new DailyStat(d.ToString("MM-dd"), dayAppts.Count, dayAppts.Sum(a => a.TotalPrice)));
        }
        return result;
    }
}
