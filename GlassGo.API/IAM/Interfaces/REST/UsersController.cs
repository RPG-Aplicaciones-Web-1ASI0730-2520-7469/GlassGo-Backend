using System.Net.Mime;
using GlassGo.API.IAM.Domain.Model.Queries;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Interfaces.REST.Resources;
using GlassGo.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using GlassGo.API.IAM.Domain.Model.Commands;

namespace GlassGo.API.IAM.Interfaces.REST;

/// <summary>
/// Controller that exposes endpoints to work with users.
/// </summary>
/// <remarks>
/// Routes are rooted at <c>api/v1/users</c>. Endpoints require authorization.
/// </remarks>
[Microsoft.AspNetCore.Authorization.Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User endpoints")]
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
}