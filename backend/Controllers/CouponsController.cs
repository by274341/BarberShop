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
[Authorize(Roles = "Manager")]
public class CouponsController : ControllerBase
{
    private readonly AppDbContext _db;
    public CouponsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<CouponDto>> GetAll()
    {
        return await _db.Coupons.Select(c => new CouponDto(
            c.Id, c.Name, c.FaceValue, c.MinAmount, c.ValidFrom, c.ValidTo, c.TotalCount, c.IssuedCount
        )).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CouponCreateRequest req)
    {
        _db.Coupons.Add(new Coupon
        {
            Name = req.Name, FaceValue = req.FaceValue, MinAmount = req.MinAmount,
            ValidFrom = req.ValidFrom, ValidTo = req.ValidTo, TotalCount = req.TotalCount
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{id}/issue")]
    public async Task<ActionResult> Issue(int id, [FromBody] List<int> userIds)
    {
        var coupon = await _db.Coupons.FindAsync(id);
        if (coupon == null) return NotFound();

        foreach (var uid in userIds)
        {
            _db.UserCoupons.Add(new UserCoupon { UserId = uid, CouponId = id });
            coupon.IssuedCount++;
        }
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<List<object>> GetMy()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return await _db.UserCoupons.Where(uc => uc.UserId == userId)
            .Include(uc => uc.Coupon)
            .Select(uc => new {
                uc.Id, uc.Coupon!.Name, uc.Coupon.FaceValue, uc.Coupon.MinAmount,
                uc.IsUsed, uc.UsedAt, uc.Coupon.ValidTo
            }).ToListAsync<object>();
    }

    [HttpPost("{id}/claim")]
    [Authorize]
    public async Task<ActionResult> Claim(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var coupon = await _db.Coupons.FindAsync(id);
        if (coupon == null) return NotFound();
        if (coupon.IssuedCount >= coupon.TotalCount) return BadRequest(new { message = "已领完" });
        if (await _db.UserCoupons.AnyAsync(uc => uc.UserId == userId && uc.CouponId == id))
            return BadRequest(new { message = "已领取过" });
        _db.UserCoupons.Add(new UserCoupon { UserId = userId, CouponId = id });
        coupon.IssuedCount++;
        await _db.SaveChangesAsync();
        return Ok();
    }
}