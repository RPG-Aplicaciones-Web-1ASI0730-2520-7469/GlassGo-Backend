namespace GlassGo.API.IAM.Domain.Model.ValueObjects;

/// <summary>
/// Value object representing user notification preferences.
/// </summary>
public record NotificationSettings
{
    public bool Email { get; init; }
    public bool Sms { get; init; }
    public bool Push { get; init; }

    public NotificationSettings()
    {
        Email = true;
        Sms = false;
        Push = true;
    }

    public NotificationSettings(bool email, bool sms, bool push)
    {
        Email = email;
        Sms = sms;
        Push = push;
    }
}
