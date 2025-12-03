namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record NotificationSettingsResource(
    bool Email,
    bool Sms,
    bool Push
);
