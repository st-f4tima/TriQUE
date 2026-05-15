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

        public string GenerateTripSummaryPdf(DateTime? from, DateTime? to, int? routeID, int? driverID, string generatedBy)
        {
            return Generate(new TripSummaryReport(from, to, routeID, driverID, generatedBy));
        }

        public string GenerateDriverPerformancePdf(DateTime? from, DateTime? to, int? routeID, int? driverID, string generatedBy)
        {
            var report = new DriverPerformanceReport(from, to, routeID, driverID, generatedBy);
            return report.GeneratePdf();
        }
    }
}