using GlassGo.API.Analytics.Domain.Entities;
using GlassGo.API.Analytics.Domain.Interfaces;
using GlassGo.API.Analytics.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Analytics.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DashboardAnalyticsContext _context;

        public ReportRepository(DashboardAnalyticsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task AddAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}