using GlassGo.API.Analytics.Domain.Entities;
using GlassGo.API.Analytics.Domain.Interfaces;

namespace GlassGo.API.Analytics.Domain.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}