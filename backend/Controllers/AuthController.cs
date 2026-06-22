using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonBooking.API.Data;
using SalonBooking.API.DTOs;
using SalonBooking.API.Models;
using SalonBooking.API.Services;

namespace SalonBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    
    public AuthController(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "用户名或密码错误" });
        
        var token = _jwt.GenerateToken(user.Id, user.Username, user.Role);
        return new LoginResponse(user.Id, user.Username, user.Role, user.Nickname, token);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Username == req.Username))
            return BadRequest(new { message = "用户名已存在" });
        
        var user = new User
        {
            Username = req.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Role = "Customer",
            Nickname = req.Nickname,
            Phone = req.Phone,
            CreatedAt = DateTime.Now
        };
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        var token = _jwt.GenerateToken(user.Id, user.Username, user.Role);
        return new LoginResponse(user.Id, user.Username, user.Role, user.Nickname, token);
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();
        return new UserDto(user.Id, user.Username, user.Nickname, user.Phone, user.Avatar ?? "");
    }
}
