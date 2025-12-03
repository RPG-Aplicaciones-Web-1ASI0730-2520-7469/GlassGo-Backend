using System.Net.Mime;
using GlassGo.API.Profiles.Domain.Model.Queries;
using GlassGo.API.Profiles.Domain.Services;
using GlassGo.API.Profiles.Interfaces.REST.Resources;
using GlassGo.API.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlassGo.API.Profiles.Interfaces.REST;
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile Endpoints.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    [HttpGet("{profileId:int}")]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation("Get Profile by Id (Admin only)", "Get a profile by its unique identifier.", OperationId = "GetProfileById")]
    [SwaggerResponse(200, "The profile was found and returned.", typeof(ProfileResource))]
    [SwaggerResponse(404, "The profile was not found.")]
    public async Task<IActionResult> GetProfileById(int profileId)
    {
        var getProfileByIdQuery = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(getProfileByIdQuery);
        if (profile is null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }

    [HttpPost]
    [SwaggerOperation("Create Profile", "Create a new profile.", OperationId = "CreateProfile")]
    [SwaggerResponse(201, "The profile was created.", typeof(ProfileResource))]
    [SwaggerResponse(400, "The profile was not created.")]
    public async Task<IActionResult> CreateProfile(CreateProfileResource resource)
    {
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var profile = await profileCommandService.Handle(createProfileCommand);
        if (profile is null) return BadRequest();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return CreatedAtAction(nameof(GetProfileById), new { profileId = profile.Id }, profileResource);
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation("Get All Profiles (Admin only)", "Get all profiles.", OperationId = "GetAllProfiles")]
    [SwaggerResponse(200, "The profiles were found and returned.", typeof(IEnumerable<ProfileResource>))]
    [SwaggerResponse(404, "The profiles were not found.")]
    public async Task<IActionResult> GetAllProfiles()
    {
        var getAllProfilesQuery = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(getAllProfilesQuery);
        var profileResources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(profileResources);
    }
}