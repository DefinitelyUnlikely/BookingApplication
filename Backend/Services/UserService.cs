using BookingApplication.Controllers;
using BookingApplication.Exceptions;
using BookingApplication.Helpers;
using BookingApplication.Interfaces;
using BookingApplication.Models;
using BookingApplication.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Services;

public interface IUserService : IService<User, RegisterUserRequest, EditUserRequest>
{
    public Task<SignInUserResponse> LoginAsync(SignInUserRequest request);

    // id, username
    //public Task<Dictionary<string, string>> GetAllAsync();
    public Task<User?> GetByIdAsync(string id);
    public Task<User?> DeleteAsync(string entityId);
}

public class UserService : IUserService //: EfService<User, RegisterRequest, EditUserRequest>, IUserService
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    //private readonly EntityUpdaterService updaterService;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager
    //EntityUpdaterService updaterService
    )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        //this.updaterService = updaterService;
    }

    public async Task<User> CreateFromRequestAsync(RegisterUserRequest request)
    {
        var user = new User()
        {
            UserName = request.Username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new IdentityException($"Error while creating user: {errorMessages}");
        }
        return user;
    }

    public async Task<User?> DeleteAsync(string entityId)
    {
        var user = await userManager.FindByIdAsync(entityId.ToString());
        if (user == null)
            return null;

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
            return null;

        throw new IdentityException($"Unable to delete {user.UserName}");
    }

    public async Task DeleteAsync(User entityToRemove)
    {
        var result = await userManager.DeleteAsync(entityToRemove);
        if (result.Succeeded)
            return;

        throw new IdentityException($"Unable to delete {entityToRemove.UserName}");
    }

    public Task<User?> DeleteAsync(Guid entityId)
    {
        throw new NotImplementedException();
    }

    public Task EditAsync(User entityToEdit)
    {
        throw new NotImplementedException();
    }

    public async Task<User> EditFromRequestAsync(EditUserRequest request)
    {
        // // Possible solution for updating User if nothing easier is found.
        // var user =
        //     await userManager.FindByIdAsync(request.Id)
        //     ?? throw new ArgumentNullException($"Unable to find user with id {request.Id}");

        // EntityUpdaterHelper.UpdateEntity<User>(user, request);

        var user = new User { Id = request.Id, UserName = request.Username };

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
            return user;

        throw new IdentityException($"Unable to update {user.UserName}");
    }

    // Implement with pagination if too many users
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await userManager.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public Task<User?> GetByIdAsync(Guid entityId)
    {
        throw new NotImplementedException();
    }

    public async Task<SignInUserResponse> LoginAsync(SignInUserRequest request)
    {
        var user =
            await userManager.FindByNameAsync(request.Username)
            ?? throw new IdentityException("Invalid username");

        var result = await signInManager.PasswordSignInAsync(
            request.Username,
            request.Password,
            false,
            false
        );

        if (result.Succeeded)
        {
            return new SignInUserResponse { Username = user.UserName! };
        }
        throw new IdentityException($"Invalid username or password");
    }

    Task IService<User, RegisterUserRequest, EditUserRequest>.DeleteAsync(Guid entityId)
    {
        return DeleteAsync(entityId);
    }
}