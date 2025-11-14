using System.Text.Json.Serialization;

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
}