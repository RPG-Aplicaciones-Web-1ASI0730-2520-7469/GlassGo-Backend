using GlassGo.API.Analytics.Application.Internal.QueryServices;

namespace GlassGo.API.Analytics.Application.ACL;

/// <summary>
/// Implementation of the Analytics bounded context facade.
/// </summary>
/// <remarks>
/// This class provides the Anti-Corruption Layer (ACL) implementation,
/// translating internal Analytics domain concepts into formats suitable
/// for external bounded contexts, preventing domain pollution.
/// </remarks>
public class AnalyticsContextFacade : IAnalyticsContextFacade
{
    private readonly ReportQueryService _reportQueryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalyticsContextFacade"/> class.
    /// </summary>
    /// <param name="reportQueryService">The query service for report operations.</param>
    public AnalyticsContextFacade(ReportQueryService reportQueryService)
    {
        _reportQueryService = reportQueryService;
    }

    /// <inheritdoc />
    public async Task<object> GenerateReportForExternalContext(int entityId, string reportType)
    {
        var report = await _reportQueryService.Handle(entityId);

        return new
        {
            ReportId = report?.Id,
            Type = reportType,
            GeneratedAt = DateTime.UtcNow,
            Data = report != null ? TransformToExternalFormat(report) : null
        };
    }

    /// <inheritdoc />
    public async Task<object> GetAnalyticsSummary(string contextName)
    {
        var reports = await _reportQueryService.Handle();

        return new
        {
            Context = contextName,
            TotalReports = reports.Count(),
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Transforms internal domain report to external format.
    /// </summary>
    /// <param name="report">The internal report entity.</param>
    /// <returns>An object representing the report in external format.</returns>
    private object TransformToExternalFormat(Domain.Entities.Report report)
    {
        return new
        {
            report.Id,
            report.Type,
            report.Data
        };
    }
}
