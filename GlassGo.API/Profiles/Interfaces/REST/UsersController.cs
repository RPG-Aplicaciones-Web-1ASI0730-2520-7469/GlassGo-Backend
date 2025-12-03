using System.Net.Mime;
using GlassGo.API.IAM.Domain.Model.Queries;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Interfaces.REST.Resources;
using GlassGo.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using GlassGo.API.IAM.Domain.Model.Commands;

namespace GlassGo.API.Profiles.Interfaces.REST;

/// <summary>
/// Controller that exposes endpoints to work with user profiles.
/// </summary>
/// <remarks>
/// Routes are rooted at <c>api/v1/users</c>. Endpoints require authorization.
/// </remarks>
[Microsoft.AspNetCore.Authorization.Authorize]
[ApiController]
[Route("api/v1/users")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User Profile Management endpoints")]
public class UsersController(IUserQueryService userQueryService, IUserCommandService userCommandService) : ControllerBase
{
    /// <summary>
    /// Get a user by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to retrieve.</param>
    /// <returns>Returns an <see cref="IActionResult"/> that contains the <see cref="UserResource"/> when found.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a user by its id",
        Description = "Get a user by its id",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was found", typeof(UserResource))]
    public async Task<IActionResult> GetUserById(int id)
    {
        var getUserByIdQuery = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    /// <summary>
    /// Get all users. (Admin only)
    /// </summary>
    /// <returns>Returns an <see cref="IActionResult"/> that contains a collection of <see cref="UserResource"/> objects.</returns>
    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Get all users (Admin only)",
        Description = "Get all users",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
    
    /// <summary>
    /// Updates a user's role. (Admin only)
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="resource">The update user role resource.</param>
    /// <returns>An updated user resource.</returns>
    [HttpPatch("{userId}")]
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Updates a user's role (Admin only)",
        Description = "Updates a user's role by user id.",
        OperationId = "UpdateUserRole")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user role was updated", typeof(UserResource))]
    public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleResource resource)
    {
        var command = new UpdateUserRoleCommand(userId, resource.Role);
        await userCommandService.Handle(command);
        
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId));
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
    
    /// <summary>
    /// Update user profile information.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="resource">The updated profile information.</param>
    /// <returns>The updated user resource.</returns>
    [HttpPut("{userId}/profile")]
    [SwaggerOperation(
        Summary = "Update user profile",
        Description = "Update user profile information",
        OperationId = "UpdateUserProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile updated successfully", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileResource resource)
    {
        var command = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(userId, resource);
        await userCommandService.Handle(command);
        
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId));
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
    
    /// <summary>
    /// Update user notification settings.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="resource">The updated notification settings.</param>
    /// <returns>The updated user resource.</returns>
    [HttpPut("{userId}/settings/notifications")]
    [SwaggerOperation(
        Summary = "Update notification settings",
        Description = "Update user notification preferences",
        OperationId = "UpdateNotificationSettings")]
    [SwaggerResponse(StatusCodes.Status200OK, "Settings updated successfully", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> UpdateNotificationSettings(int userId, [FromBody] UpdateNotificationSettingsResource resource)
    {
        var command = UpdateNotificationSettingsCommandFromResourceAssembler.ToCommandFromResource(userId, resource);
        await userCommandService.Handle(command);
        
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId));
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
    
    /// <summary>
    /// Get user statistics for dashboard.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>User statistics.</returns>
    [HttpGet("{userId}/stats")]
    [SwaggerOperation(
        Summary = "Get user statistics",
        Description = "Get user metrics and statistics for dashboard",
        OperationId = "GetUserStats")]
    [SwaggerResponse(StatusCodes.Status200OK, "Statistics retrieved successfully", typeof(UserStatsResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetUserStats(int userId)
    {
        // TODO: Implement actual statistics calculation from orders, payments, etc.
        // For now, return mock data
        var stats = new UserStatsResource(
            userId,
            TotalOrders: 0,
            ActiveOrders: 0,
            CompletedOrders: 0,
            TotalRevenue: 0m,
            MonthlyRevenue: 0m,
            AverageOrderValue: 0m,
            CustomerSatisfaction: 0.0,
            Period: "last_30_days"
        );
        
        return Ok(stats);
    }
    
    /// <summary>
    /// Get user settings.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>User settings.</returns>
    [HttpGet("{userId}/settings")]
    [SwaggerOperation(
        Summary = "Get user settings",
        Description = "Get user preferences and settings",
        OperationId = "GetUserSettings")]
    [SwaggerResponse(StatusCodes.Status200OK, "Settings retrieved successfully", typeof(UserSettingsResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetUserSettings(int userId)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId));
        if (user == null) return NotFound();
        
        var settings = new UserSettingsResource(
            user.Id,
            new NotificationSettingsResource(
                user.Notifications.Email,
                user.Notifications.Sms,
                user.Notifications.Push
            ),
            new TwoFactorAuthSettingsResource(
                Enabled: false,
                Method: null
            ),
            Language: "es",
            Timezone: "America/Lima",
            Theme: "light",
            user.PreferredCurrency
        );
        
        return Ok(settings);
    }
    
    /// <summary>
    /// Update user settings.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="resource">The updated settings.</param>
    /// <returns>Updated user settings.</returns>
    [HttpPut("{userId}/settings")]
    [SwaggerOperation(
        Summary = "Update user settings",
        Description = "Update user preferences and settings",
        OperationId = "UpdateUserSettings")]
    [SwaggerResponse(StatusCodes.Status200OK, "Settings updated successfully", typeof(UserSettingsResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> UpdateUserSettings(int userId, [FromBody] UserSettingsResource resource)
    {
        var command = UpdateNotificationSettingsCommandFromResourceAssembler.ToCommandFromResource(
            userId, 
            new UpdateNotificationSettingsResource(resource.Notifications)
        );
        await userCommandService.Handle(command);
        
        return await GetUserSettings(userId);
    }
    
    /// <summary>
    /// Get user history.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>User activity history.</returns>
    [HttpGet("{userId}/histories")]
    [SwaggerOperation(
        Summary = "Get user histories",
        Description = "Get user activity history",
        OperationId = "GetUserHistories")]
    [SwaggerResponse(StatusCodes.Status200OK, "Histories retrieved successfully", typeof(IEnumerable<HistoryItemResource>))]
    public IActionResult GetUserHistories(int userId)
    {
        // TODO: Implement actual history retrieval from database
        // For now, return empty list
        var history = new List<HistoryItemResource>();
        return Ok(history);
    }
}
