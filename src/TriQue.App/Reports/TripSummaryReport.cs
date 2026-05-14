using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Data;
using TriQue.Data.Repositories;
using QuestDocument = QuestPDF.Fluent.Document;

namespace TriQue.Reports
{
    public class TripSummaryReport : ReportBase
    {
        private readonly TripRepository _tripRepo = new TripRepository();

        private readonly DateTime? _from;
        private readonly DateTime? _to;

        private readonly int? _routeID;
        private readonly int? _driverID;

        public TripSummaryReport(DateTime? from, DateTime? to, int? routeID, int? driverID, string generatedBy) : base(generatedBy)
        {
            _from = from;
            _to = to;
            _routeID = routeID;
            _driverID = driverID;
        }

        public override string GeneratePdf()
        {
            var data = _tripRepo.GetTripSummary(_from, _to, _routeID, _driverID);
            var stats = _tripRepo.GetReportStats(_from, _to, _routeID, _driverID);

            string dateRange = FormatDateLabel(_from, _to);
            string outputPath = BuildOutputPath("TripSummary");

            QuestDocument.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("TriQue - Trip Summary Report")
                                    .FontSize(18).Bold().FontColor("#1a56db");
                                c.Item().Text($"Period: {dateRange}")
                                    .FontSize(10).FontColor("#6b7280");
                                c.Item().Text($"Generated: {DateTime.Now:MMM dd, yyyy hh:mm tt} by {GeneratedBy}")
                                    .FontSize(9).FontColor("#9ca3af");
                            });
                        });

                        col.Item().PaddingTop(10).Row(row =>
                        {
                            StatCard(row, "Total Trips", stats.totalTrips.ToString(), "#1a56db");
                            StatCard(row, "Total Earnings", $"₱ {stats.totalEarnings:N2}", "#059669");
                            StatCard(row, "Most Active", stats.mostActive, "#7c3aed");
                            StatCard(row, "Least Active", stats.leastActive, "#6b7280");
                            StatCard(row, "Fastest Trip", $"{stats.fastest:0} min", "#0891b2");
                            StatCard(row, "Slowest Trip", $"{stats.slowest:0} min", "#dc2626");
                        });

                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#e5e7eb");
                    });

                    page.Content().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            foreach (var h in new[] { "Date", "Driver", "Body No.", "Route", "Status", "Earnings", "Duration" })
                            {
                                header.Cell().Background("#1a56db").Padding(5)
                                    .Text(h).FontColor("#ffffff").Bold().FontSize(9);
                            }
                        });

                        bool alternate = false;
                        foreach (DataRow row in data.Rows)
                        {
                            string bg = alternate ? "#f9fafb" : "#ffffff";
                            alternate = !alternate;

                            foreach (var col in new[] { "Date", "Driver", "Body No", "Route", "Status", "Earnings", "Duration" })
                            {
                                string val = row[col]?.ToString() ?? "-";
                                if (col == "Date" && DateTime.TryParse(val, out var dt))
                                    val = dt.ToString("MMM dd, yyyy HH:mm");

                                table.Cell().Background(bg).Padding(5).Text(val).FontSize(9);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ").FontSize(9).FontColor("#9ca3af");
                        x.CurrentPageNumber().FontSize(9).FontColor("#9ca3af");
                        x.Span(" of ").FontSize(9).FontColor("#9ca3af");
                        x.TotalPages().FontSize(9).FontColor("#9ca3af");
                    });
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }

        private void StatCard(RowDescriptor row, string label, string value, string color)
        {
            row.RelativeItem().Border(1).BorderColor("#e5e7eb").Padding(8).Column(c =>
            {
                c.Item().Text(value).FontSize(13).Bold().FontColor(color);
                c.Item().Text(label).FontSize(8).FontColor("#6b7280");
            });
        }
    }
}