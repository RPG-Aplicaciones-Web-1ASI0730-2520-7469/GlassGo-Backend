namespace GlassGo.API.IAM.Domain.Model.Commands;

public record UpdateUserRoleCommand(int UserId, string Role);