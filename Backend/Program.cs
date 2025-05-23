using BookingApplication.Data;
using BookingApplication.Interfaces;
using BookingApplication.Models;
using BookingApplication.Models.Dtos;
using BookingApplication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using BookingApplication.Repositories;


namespace BookingApplication;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BookingAppContext>(options =>
            options.UseNpgsql(DbConfig.connectionString)
        );

        builder
            .Services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<BookingAppContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole<Guid>>();

        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<RoomService>();
        builder.Services.AddScoped<IRepository<Room>, RoomRepository>();
        builder.Services.AddScoped<IService<Booking, CreateBookingRequest, EditBookingRequest>, BookingService>();
        builder.Services.AddScoped<IRepository<Booking>, BookingRepository>();
        builder.Services.AddScoped<ScheduleService>();

        // builder
        //     .Services.AddIdentityCore<User>()
        //     .AddEntityFrameworkStores<BookingAppContext>()
        //     .AddApiEndpoints();

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

        // For roles.
        builder.Services.AddAuthorization();

        // Cors for frontend and cookies
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            );
        });


        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapControllers();

        //Must be before Authent/Author
        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        // app.UseHttpsRedirection();

        await CreateDefaultRoles(app);
        await CreateAdminAccount(app);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.Run();
    }



    static async Task CreateDefaultRoles(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (await roleManager.FindByNameAsync("Admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }
        if (await roleManager.FindByNameAsync("User") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        }
    }

    static async Task CreateAdminAccount(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        if (await userManager.FindByNameAsync("Admin") == null)
        {
            var user = new User { UserName = "Admin" };
            var password = "Pass123!";
            var createUserResult = await userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                throw new Exception("Unable to create Admin user.");
            }
            var addRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            if (!addRoleResult.Succeeded)
            {
                throw new Exception("Unable to assign role: 'Admin' to Admin user.");
            }
        }
    }
}
