namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record UserSettingsResource(
    int UserId,
    NotificationSettingsResource Notifications,
    TwoFactorAuthSettingsResource TwoFactorAuth,
    string Language,
    string Timezone,
    string Theme,
    string PreferredCurrency
);

public record TwoFactorAuthSettingsResource(
    bool Enabled,
    string? Method
);
