using System.Text.Json.Serialization;
using GlassGo.API.IAM.Domain.Model.ValueObjects;

namespace GlassGo.API.IAM.Domain.Model.Aggregates;

/// <summary>
/// Represents an application user.
/// </summary>
/// <remarks>
/// This aggregate encapsulates user identity details and exposes methods to update mutable fields.
/// Password hashes are ignored for JSON serialization.
/// </remarks>
public class User(string username, string passwordHash)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with empty values.
    /// </summary>
    public User() : this(string.Empty, string.Empty)
    {
    }

    /// <summary>
    /// Gets the identifier of the user.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the username.
    /// </summary>
    public string Username { get; private set; } = username;

    /// <summary>
    /// Gets the password hash. This property is ignored for JSON serialization.
    /// </summary>
    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;
    
    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's role.
    /// </summary>
    public string Role { get; private set; } = "business-owner";
    
    /// <summary>
    /// Gets or sets the user's company name.
    /// </summary>
    public string Company { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's business name (for business owners).
    /// </summary>
    public string? BusinessName { get; private set; }
    
    /// <summary>
    /// Gets or sets the user's tax ID.
    /// </summary>
    public string? TaxId { get; private set; }
    
    /// <summary>
    /// Gets or sets the user's address.
    /// </summary>
    public string? Address { get; private set; }
    
    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    public string Phone { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's preferred currency.
    /// </summary>
    public string PreferredCurrency { get; private set; } = "PEN";
    
    /// <summary>
    /// Gets or sets the user's loyalty points.
    /// </summary>
    public int LoyaltyPoints { get; private set; }
    
    /// <summary>
    /// Gets or sets whether the user account is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;
    
    /// <summary>
    /// Gets the timestamp when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the user's notification preferences.
    /// </summary>
    public NotificationSettings Notifications { get; private set; } = new();
    
    // TEMPORARILY DISABLED - TODO: Re-enable after database schema is fixed
    // /// <summary>
    // /// Gets or sets the user's payment methods.
    // /// </summary>
    // public List<PaymentMethod> PaymentMethods { get; private set; } = new();

    /// <summary>
    /// Update the user's username.
    /// </summary>
    /// <param name="username">The new username.</param>
    /// <returns>The updated <see cref="User"/> instance.</returns>
    public User UpdateUsername(string username)
    {
        Username = username;
        return this;
    }

    /// <summary>
    /// Update the user's password hash.
    /// </summary>
    /// <param name="passwordHash">The new password hash.</param>
    /// <returns>The updated <see cref="User"/> instance.</returns>
    public User UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }
    
    /// <summary>
    /// Updates the user's role.
    /// </summary>
    /// <param name="role">The new role.</param>
    /// <returns>The updated <see cref="User"/> instance.</returns>
    public User UpdateRole(string role)
    {
        Role = role;
        return this;
    }
    
    /// <summary>
    /// Updates the user's email.
    /// </summary>
    /// <param name="email">The new email.</param>
    /// <returns>The updated <see cref="User"/> instance.</returns>
    public User UpdateEmail(string email)
    {
        Email = email;
        return this;
    }
    
    /// <summary>
    /// Updates the user's profile information.
    /// </summary>
    public User UpdateProfile(string firstName, string lastName, string phone, string? company = null, 
        string? businessName = null, string? taxId = null, string? address = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        if (company != null) Company = company;
        if (businessName != null) BusinessName = businessName;
        if (taxId != null) TaxId = taxId;
        if (address != null) Address = address;
        return this;
    }
    
    /// <summary>
    /// Updates the user's notification settings.
    /// </summary>
    public User UpdateNotifications(NotificationSettings notifications)
    {
        Notifications = notifications;
        return this;
    }
    
    /// <summary>
    /// Adds loyalty points to the user.
    /// </summary>
    public User AddLoyaltyPoints(int points)
    {
        LoyaltyPoints += points;
        return this;
    }
    
    /// <summary>
    /// Deactivates the user account.
    /// </summary>
    public User Deactivate()
    {
        IsActive = false;
        return this;
    }
    
    /// <summary>
    /// Activates the user account.
    /// </summary>
    public User Activate()
    {
        IsActive = true;
        return this;
    }
}