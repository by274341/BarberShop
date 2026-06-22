using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;
using SalonBooking.API.Models;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ServicesController(AppDbContext db) => _db = db;
    
    [HttpGet]
    public async Task<List<ServiceDto>> GetAll()
    {
        return await _db.Services.Where(s => s.IsActive).Select(s => new ServiceDto(
            s.Id, s.Name, s.Category, s.Price, s.Duration, s.Description, s.Image, s.IsActive, s.IsDiscountable
        )).ToListAsync();
    }
    
    [HttpGet("all")]
    [Authorize(Roles = "Manager")]
    public async Task<List<ServiceDto>> GetAllAdmin()
    {
        return await _db.Services.Select(s => new ServiceDto(
            s.Id, s.Name, s.Category, s.Price, s.Duration, s.Description, s.Image, s.IsActive, s.IsDiscountable
        )).ToListAsync();
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<ServiceDto>> Create([FromBody] ServiceDto dto)
    {
        var service = new Service
        {
            Name = dto.Name, Category = dto.Category, Price = dto.Price,
            Duration = dto.Duration, Description = dto.Description, Image = dto.Image, IsActive = true
        };
        _db.Services.Add(service);
        await _db.SaveChangesAsync();
        return Ok(new ServiceDto(service.Id, service.Name, service.Category, service.Price, service.Duration, service.Description, service.Image, service.IsActive, service.IsDiscountable));
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> Update(int id, [FromBody] ServiceDto dto)
    {
        var service = await _db.Services.FindAsync(id);
        if (service == null) return NotFound();
        service.Name = dto.Name; service.Category = dto.Category; service.Price = dto.Price;
        service.Duration = dto.Duration; service.Description = dto.Description; service.Image = dto.Image;
        service.IsActive = dto.IsActive; service.IsDiscountable = dto.IsDiscountable;
        await _db.SaveChangesAsync();
        return Ok();
    }
}
