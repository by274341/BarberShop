using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _db;
    public CustomersController(AppDbContext db) => _db = db;
    
    [HttpGet]
    public async Task<List<CustomerDto>> GetAll()
    {
        return await _db.Users.Where(u => u.Role == "Customer").Select(u => new CustomerDto(
            u.Id, u.Nickname, u.Phone,
            u.CustomerAppointments.Count(a => a.Status == "Completed"),
            u.CustomerAppointments.Where(a => a.Status == "Completed").Sum(a => a.TotalPrice),
            u.CreatedAt
        )).ToListAsync();
    }
}
