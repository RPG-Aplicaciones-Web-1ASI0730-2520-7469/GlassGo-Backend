using GlassGo.API.Analytics.Domain.Entities;
using GlassGo.API.Analytics.Domain.Interfaces;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.Analytics.Application.Internal.CommandServices;

/// <summary>
/// Command service responsible for handling all write operations related to reports.
/// </summary>
/// <remarks>
/// This service implements the Command side of the CQRS pattern, managing
/// report creation, updates, and deletions. It ensures data consistency through
/// unit of work pattern and maintains domain integrity.
/// </remarks>
public class ReportCommandService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportCommandService"/> class.
    /// </summary>
    /// <param name="reportRepository">The repository for report persistence operations.</param>
    /// <param name="unitOfWork">The unit of work for transaction management.</param>
    public ReportCommandService(IReportRepository reportRepository, IUnitOfWork unitOfWork)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new report in the system.
    /// </summary>
    /// <param name="report">The report entity to be created.</param>
    /// <returns>The created report entity with generated identifier.</returns>
    public async Task<Report> Handle(Report report)
    {
        await _reportRepository.AddAsync(report);
        await _unitOfWork.CompleteAsync();
        return report;
    }

    /// <summary>
    /// Updates an existing report with new information.
    /// </summary>
    /// <param name="id">The identifier of the report to update.</param>
    /// <param name="report">The report entity containing updated information.</param>
    /// <returns>The updated report entity.</returns>
    public async Task<Report> Handle(int id, Report report)
    {
        _reportRepository.Update(report);
        await _unitOfWork.CompleteAsync();
        return report;
    }

    /// <summary>
    /// Deletes a report from the system by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the report to delete.</param>
    /// <returns>True if the report was successfully deleted; otherwise, false.</returns>
    public async Task<bool> Handle(int id)
    {
        var report = await _reportRepository.FindByIdAsync(id);
        if (report == null) return false;

        _reportRepository.Remove(report);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
