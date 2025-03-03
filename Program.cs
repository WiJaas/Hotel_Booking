using Hotel_Booking.Models;
using Microsoft.EntityFrameworkCore;
using Hotel_Booking.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<HotelBookingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("gestionHotelContextConnection")));

builder.Services.AddDbContext<Hotel_BookingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("gestionHotelContextConnection")));
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<Hotel_BookingContext>().AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Hotel_BookingContext>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add role support
    .AddEntityFrameworkStores<Hotel_BookingContext>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<Hotel_BookingContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
