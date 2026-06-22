namespace SalonBooking.API.DTOs;

public record LoginRequest(string Username, string Password);
public record LoginResponse(int UserId, string Username, string Role, string Nickname, string Token);
public record RegisterRequest(string Username, string Password, string Nickname, string Phone);

public record UserDto(int Id, string Username, string Nickname, string Phone, string Avatar);

public record StylistDto(int Id, int UserId, string Nickname, string Avatar, string Level, int Years, string Specialty, string Description, string Status, double AvgRating, int ReviewCount);
public record StylistCreateRequest(string Nickname, string Level, int Years, string Specialty, string Description);

public record ServiceDto(int Id, string Name, string Category, decimal Price, int Duration, string? Description, string? Image, bool IsActive, bool IsDiscountable);

public record AppointmentCreateRequest(List<int> ServiceIds, int? StylistId, DateTime AppointmentDate, string TimeSlot, string? Note);
public record AppointmentDto(int Id, string CustomerName, string? StylistName, string Services, decimal TotalPrice, DateTime AppointmentDate, string TimeSlot, string Status, DateTime CreatedAt, string? Note);

public record ScheduleDto(int Id, int StylistId, int DayOfWeek, string StartTime, string EndTime, bool IsOff);
public record BatchScheduleRequest(List<ScheduleItem> Items);
public record ScheduleItem(int DayOfWeek, string StartTime, string EndTime, bool IsOff);

public record ReviewDto(int Id, string CustomerName, int Rating, string? Content, DateTime CreatedAt);
public record ReviewCreateRequest(int AppointmentId, int Rating, string? Content);

public record CouponDto(int Id, string Name, decimal FaceValue, decimal MinAmount, DateTime ValidFrom, DateTime ValidTo, int TotalCount, int IssuedCount);
public record CouponCreateRequest(string Name, decimal FaceValue, decimal MinAmount, DateTime ValidFrom, DateTime ValidTo, int TotalCount);

public record PromotionDto(int Id, string Name, int ServiceId, string ServiceName, decimal DiscountRate, DateTime StartTime, DateTime EndTime, bool IsActive);
public record PromotionCreateRequest(string Name, int ServiceId, decimal DiscountRate, DateTime StartTime, DateTime EndTime);

public record DashboardDto(int TodayAppointments, decimal TodayRevenue, int ActiveStylists, int PendingOrders, List<DailyStat> WeeklyStats, List<AppointmentDto> TodayList);
public record DailyStat(string Date, int Count, decimal Revenue);

public record StylistStatsDto(int TodayOrders, int WeekOrders, decimal MonthRevenue, List<DailyStat> WeeklyTrend);
public record StylistPerformanceDto(int StylistId, string Nickname, int TotalOrders, decimal TotalRevenue);

public record CustomerDto(int Id, string Nickname, string Phone, int TotalOrders, decimal TotalSpent, DateTime CreatedAt);
