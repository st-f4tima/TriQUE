// TriQue/Services/ReportService.cs
using QuestPDF.Infrastructure;
using TriQue.Reports;
using TriQue.Reports;

namespace TriQue.Services
{
    public class ReportService
    {
        public ReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public string Generate(ReportBase report)
        {
            return report.GeneratePdf();
        }

        // Convenience factories so callers don't need to know the concrete types
        public string GenerateTripSummaryPdf(DateTime? from, DateTime? to, int? routeID, int? driverID, string generatedBy)
        {
            return Generate(new TripSummaryReport(from, to, routeID, driverID, generatedBy));
        }


        public string GenerateDriverPerformancePdf(DateTime? from, DateTime? to, int? routeID, int? driverID, string generatedBy)
        {
            return Generate(new DriverPerformanceReport(from, to, generatedBy));
        }
    }
}