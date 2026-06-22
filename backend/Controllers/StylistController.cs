using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;
using SalonBooking.API.Models;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/stylist")]
[Authorize(Roles = "Stylist")]
public class StylistController : ControllerBase
{
    private readonly AppDbContext _db;
    public StylistController(AppDbContext db) => _db = db;

    private async Task<int> GetMyStylistId()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var stylist = await _db.Stylists.FirstOrDefaultAsync(s => s.UserId == userId);
        return stylist?.Id ?? throw new UnauthorizedAccessException();
    }

    [HttpGet("stats")]
    public async Task<StylistStatsDto> GetStats()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var today = DateTime.Today;

        var todayCount = await _db.Appointments.CountAsync(a => a.StylistId == userId && a.AppointmentDate.Date == today);
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var weekCount = await _db.Appointments.CountAsync(a => a.StylistId == userId && a.AppointmentDate.Date >= weekStart);
        var monthRevenue = await _db.Appointments
            .Where(a => a.StylistId == userId && a.AppointmentDate.Month == today.Month && a.Status == "Completed")
            .SumAsync(a => a.TotalPrice);

        var weeklyTrend = new List<DailyStat>();
        for (int i = 6; i >= 0; i--)
        {
            var date = today.AddDays(-i);
            var count = await _db.Appointments.CountAsync(a => a.StylistId == userId && a.AppointmentDate.Date == date.Date);
            weeklyTrend.Add(new DailyStat(date.ToString("MM-dd"), count, 0));
        }

        return new StylistStatsDto(todayCount, weekCount, monthRevenue, weeklyTrend);
    }

    [HttpGet("service-records")]
    public async Task<List<object>> GetServiceRecords()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return await _db.Appointments
            .Where(a => a.StylistId == userId)
            .Include(a => a.Customer)
            .Include(a => a.AppointmentServices).ThenInclude(x => x.Service)
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new {
                a.Id, customerName = a.Customer!.Nickname,
                date = a.AppointmentDate.ToString("yyyy-MM-dd"),
                services = string.Join(",", a.AppointmentServices.Select(x => x.Service!.Name)),
                a.TotalPrice, a.Status
            }).ToListAsync<object>();
    }

    [HttpGet("schedule")]
    public async Task<List<ScheduleDto>> GetSchedule()
    {
        var stylistId = await GetMyStylistId();
        return await _db.StylistSchedules.Where(s => s.StylistId == stylistId)
            .Select(s => new ScheduleDto(s.Id, s.StylistId, s.DayOfWeek,
                s.StartTime.ToString(@"hh\:mm"), s.EndTime.ToString(@"hh\:mm"), s.IsOff))
            .ToListAsync();
    }

    [HttpPut("schedule")]
    public async Task<ActionResult> SetSchedule([FromBody] BatchScheduleRequest req)
    {
        var stylistId = await GetMyStylistId();
        var existing = await _db.StylistSchedules.Where(s => s.StylistId == stylistId).ToListAsync();
        _db.StylistSchedules.RemoveRange(existing);

        foreach (var item in req.Items)
        {
            _db.StylistSchedules.Add(new StylistSchedule
            {
                StylistId = stylistId, DayOfWeek = item.DayOfWeek,
                StartTime = TimeSpan.Parse(item.StartTime),
                EndTime = TimeSpan.Parse(item.EndTime),
                IsOff = item.IsOff
            });
        }
        await _db.SaveChangesAsync();
        return Ok();
    }
}