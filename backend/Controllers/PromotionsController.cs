using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;
using SalonBooking.API.Models;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class PromotionsController : ControllerBase
{
    private readonly AppDbContext _db;
    public PromotionsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<PromotionDto>> GetAll()
    {
        return await _db.Promotions.Include(p => p.Service)
            .Select(p => new PromotionDto(p.Id, p.Name, p.ServiceId, p.Service!.Name,
                p.DiscountRate, p.StartTime, p.EndTime, p.IsActive))
            .ToListAsync();
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<List<PromotionDto>> GetActive() =>
        await _db.Promotions.Include(p => p.Service)
            .Where(p => p.IsActive)
            .Select(p => new PromotionDto(p.Id, p.Name, p.ServiceId, p.Service!.Name,
                p.DiscountRate, p.StartTime, p.EndTime, true))
            .ToListAsync();

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PromotionCreateRequest req)
    {
        _db.Promotions.Add(new Promotion
        {
            Name = req.Name, ServiceId = req.ServiceId, DiscountRate = req.DiscountRate,
            StartTime = req.StartTime, EndTime = req.EndTime, IsActive = true
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}/toggle")]
    public async Task<ActionResult> Toggle(int id)
    {
        var p = await _db.Promotions.FindAsync(id);
        if (p == null) return NotFound();
        p.IsActive = !p.IsActive;
        await _db.SaveChangesAsync();
        return Ok();
    }
}