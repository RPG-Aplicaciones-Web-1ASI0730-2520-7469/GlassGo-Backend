using GlassGo.API.Analytics.Domain.Entities;
using GlassGo.API.Analytics.Domain.Interfaces;

namespace GlassGo.API.Analytics.Application.Internal.QueryServices;

/// <summary>
/// Query service responsible for handling all read operations related to reports.
/// </summary>
/// <remarks>
/// This service implements the Query side of the CQRS pattern, providing
/// efficient read-only access to report data without modifying state.
/// </remarks>
public class ReportQueryService
{
    private readonly IReportRepository _reportRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportQueryService"/> class.
    /// </summary>
    /// <param name="reportRepository">The repository for report data retrieval.</param>
    public ReportQueryService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    /// <summary>
    /// Retrieves all reports from the system.
    /// </summary>
    /// <returns>A collection containing all report entities.</returns>
    public async Task<IEnumerable<Report>> Handle()
    {
        return await _reportRepository.ListAsync();
    }

    /// <summary>
    /// Retrieves a specific report by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the report.</param>
    /// <returns>The report entity if found; otherwise, null.</returns>
    public async Task<Report?> Handle(int id)
    {
        return await _reportRepository.FindByIdAsync(id);
    }

    /// <summary>
    /// Retrieves reports filtered by a specific type.
    /// </summary>
    /// <param name="reportType">The type of reports to retrieve.</param>
    /// <returns>A collection of reports matching the specified type.</returns>
    public async Task<IEnumerable<Report>> HandleByType(string reportType)
    {
        var allReports = await _reportRepository.ListAsync();
        return allReports.Where(r => r.Type == reportType);
    }
}
