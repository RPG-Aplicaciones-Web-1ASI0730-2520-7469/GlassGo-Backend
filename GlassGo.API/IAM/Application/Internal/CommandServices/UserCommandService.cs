using GlassGo.API.IAM.Application.Internal.OutboundServices;
using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Domain.Model.Commands;
using GlassGo.API.IAM.Domain.Repositories;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.IAM.Application.Internal.CommandServices;

/// <summary>
/// Handles user-related commands such as sign-in and sign-up.
/// </summary>
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /// <summary>
    /// Authenticate a user using the provided credentials.
    /// </summary>
    /// <param name="command">The sign-in command containing username/email and password.</param>
    /// <returns>A tuple with the authenticated <see cref="User"/> and the generated JWT token.</returns>
    /// <exception cref="Exception">Thrown when credentials are invalid.</exception>
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
            throw new Exception("Username and password are required");

        // Try to find user by username first, then by email
        var user = await userRepository.FindByUsernameAsync(command.Username);
        
        if (user == null)
        {
            user = await userRepository.FindByEmailAsync(command.Username);
        }

        if (user == null)
            throw new Exception("Invalid credentials - User not found");

        if (!hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid credentials - Wrong password");

        if (!user.IsActive)
            throw new Exception("User account is inactive");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    /// <summary>
    /// Create a new user account.
    /// </summary>
    /// <param name="command">The sign-up command with user details.</param>
    /// <returns>A completed <see cref="Task"/> when the operation succeeds.</returns>
    /// <exception cref="Exception">Thrown when the username/email is already taken or creation fails.</exception>
    public async Task Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword);
        
        // Set all the user properties
        user.UpdateEmail(command.Email);
        user.UpdateProfile(
            command.FirstName, 
            command.LastName, 
            command.Phone,
            command.Company,
            command.BusinessName,
            command.TaxId,
            command.Address
        );
        user.UpdateRole(command.Role);
        
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
    
    /// <summary>
    /// Handle a user role update operation.
    /// </summary>
    /// <param name="command">The update user role command.</param>
    public async Task Handle(UpdateUserRoleCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null) throw new Exception("User not found");

        user.UpdateRole(command.Role);
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
    }
    
    /// <summary>
    /// Handle a user profile update operation.
    /// </summary>
    /// <param name="command">The update profile command.</param>
    public async Task Handle(UpdateProfileCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null) throw new Exception("User not found");

        user.UpdateProfile(
            command.FirstName,
            command.LastName,
            command.Phone,
            command.Company,
            command.BusinessName,
            command.TaxId,
            command.Address
        );
        
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
    }
    
    /// <summary>
    /// Handle a notification settings update operation.
    /// </summary>
    /// <param name="command">The update notification settings command.</param>
    public async Task Handle(UpdateNotificationSettingsCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null) throw new Exception("User not found");

        user.UpdateNotifications(command.Notifications);
        
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
    }
}