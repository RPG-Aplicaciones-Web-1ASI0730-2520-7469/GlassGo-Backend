namespace GlassGo.API.Analytics.Application.ACL;

/// <summary>
/// Facade interface for the Analytics bounded context.
/// </summary>
/// <remarks>
/// This interface serves as the Anti-Corruption Layer (ACL) entry point,
/// providing a simplified and stable interface for other bounded contexts
/// to interact with Analytics functionality without creating tight coupling.
/// </remarks>
public interface IAnalyticsContextFacade
{
    /// <summary>
    /// Generates a report for consumption by external bounded contexts.
    /// </summary>
    /// <param name="entityId">The identifier of the entity to generate a report for.</param>
    /// <param name="reportType">The type of report to generate.</param>
    /// <returns>An object containing the generated report data in a context-neutral format.</returns>
    Task<object> GenerateReportForExternalContext(int entityId, string reportType);

    /// <summary>
    /// Retrieves analytics summary data for external bounded contexts.
    /// </summary>
    /// <param name="contextName">The name of the requesting bounded context.</param>
    /// <returns>An object containing analytics summary data.</returns>
    Task<object> GetAnalyticsSummary(string contextName);
}