using GlassGo.API.IAM.Domain.Model.ValueObjects;

namespace GlassGo.API.IAM.Domain.Model.Commands;

/// <summary>
/// Command to update user notification settings.
/// </summary>
public record UpdateNotificationSettingsCommand(
    int UserId,
    NotificationSettings Notifications
);
