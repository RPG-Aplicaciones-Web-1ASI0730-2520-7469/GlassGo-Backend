using GlassGo.API.IAM.Domain.Model.Commands;
using GlassGo.API.IAM.Domain.Model.ValueObjects;
using GlassGo.API.IAM.Interfaces.REST.Resources;

namespace GlassGo.API.IAM.Interfaces.REST.Transform;

public static class UpdateNotificationSettingsCommandFromResourceAssembler
{
    public static UpdateNotificationSettingsCommand ToCommandFromResource(int userId, UpdateNotificationSettingsResource resource)
    {
        var notifications = new NotificationSettings(
            resource.Notifications.Email,
            resource.Notifications.Sms,
            resource.Notifications.Push
        );
        
        return new UpdateNotificationSettingsCommand(userId, notifications);
    }
}
