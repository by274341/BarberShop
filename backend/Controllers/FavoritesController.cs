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
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _db;
    public FavoritesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<StylistDto>> GetMy()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return await _db.Favorites.Where(f => f.CustomerId == userId)
            .Include(f => f.Stylist).ThenInclude(s => s!.User)
            .Include(f => f.Stylist).ThenInclude(s => s!.Reviews)
            .Select(f => new StylistDto(
                f.Stylist!.Id, f.Stylist.UserId, f.Stylist.User!.Nickname, f.Stylist.User.Avatar ?? "",
                f.Stylist.Level, f.Stylist.YearsOfExperience, f.Stylist.Specialty,
                f.Stylist.Description, f.Stylist.Status,
                f.Stylist.Reviews.Any() ? f.Stylist.Reviews.Average(r => r.Rating) : 0,
                f.Stylist.Reviews.Count
            )).ToListAsync();
    }

    [HttpPost("{stylistId}")]
    public async Task<ActionResult> Add(int stylistId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (await _db.Favorites.AnyAsync(f => f.CustomerId == userId && f.StylistId == stylistId))
            return Ok();
        _db.Favorites.Add(new Favorite { CustomerId = userId, StylistId = stylistId });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{stylistId}")]
    public async Task<ActionResult> Remove(int stylistId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var fav = await _db.Favorites.FirstOrDefaultAsync(f => f.CustomerId == userId && f.StylistId == stylistId);
        if (fav != null) { _db.Favorites.Remove(fav); await _db.SaveChangesAsync(); }
        return Ok();
    }
}