using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Data;
using TriQue.Data.Repositories;
using QuestDocument = QuestPDF.Fluent.Document;

namespace TriQue.Reports
{
    public class DriverPerformanceReport : ReportBase
    {
        private readonly DriverRepository _driverRepo = new();

        private readonly DateTime? _from;
        private readonly DateTime? _to;
        private readonly int? _routeID;
        private readonly int? _driverID;

        public DriverPerformanceReport(DateTime? from, DateTime? to, int? routeID, int? driverID, string generatedBy) : base(generatedBy)
        {
            _from = from;
            _to = to;
            _routeID = routeID;
            _driverID = driverID;
        }

        public override string GeneratePdf()
        {
            var data = _driverRepo.GetDriverPerformance(_from, _to, _routeID, _driverID);
            var stats = _driverRepo.GetDriverPerformanceStats(_from, _to);

            string dateRange = FormatDateLabel(_from, _to);
            string outputPath = BuildOutputPath("DriverPerformance");

            QuestDocument.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("TriQue - Driver Performance Report")
                            .FontSize(18).Bold().FontColor("#1a56db");

                        col.Item().Text($"Period: {dateRange}")
                            .FontSize(10).FontColor("#6b7280");

                        col.Item().Text($"Generated: {DateTime.Now:MMM dd, yyyy hh:mm tt} by {GeneratedBy}")
                            .FontSize(9).FontColor("#9ca3af");

                        col.Item().PaddingTop(10).Row(row =>
                        {
                            StatCard(row, "Top Earner", stats.topEarner, "#059669");
                            StatCard(row, "Top Earnings", $"₱ {stats.topEarnings:N2}", "#059669");
                            StatCard(row, "Most Trips", stats.mostTrips, "#7c3aed");
                            StatCard(row, "Trip Count", stats.tripCount.ToString(), "#7c3aed");
                            StatCard(row, "Avg Duration", $"{stats.avgDuration:0.0} min", "#0891b2");
                        });

                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#e5e7eb");
                    });

                    page.Content().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(2); // Driver
                            cols.RelativeColumn(1); // Body No
                            cols.RelativeColumn(1); // Group
                            cols.RelativeColumn(1); // Total Trips
                            cols.RelativeColumn(1); // Completed
                            cols.RelativeColumn(1); // Earnings
                            cols.RelativeColumn(1); // Avg Duration
                            cols.RelativeColumn(1); // Fastest
                            cols.RelativeColumn(1); // Slowest
                        });

                        string[] headers =
                        {
                        "Driver", "Body No", "Group",
                        "Total Trips", "Completed", "Earnings",
                        "Avg Duration", "Fastest", "Slowest"
                    };

                        foreach (var h in headers)
                        {
                            table.Cell().Background("#1a56db").Padding(5)
                                .Text(h).FontColor("#ffffff").Bold().FontSize(9);
                        }

                        bool alternate = false;

                        foreach (var driver in data)
                        {
                            string bg = alternate ? "#f9fafb" : "#ffffff";
                            alternate = !alternate;

                            string[] cols =
                            {
                            driver.FullName,
                            driver.BodyNumber,
                            driver.GroupName,
                            driver.TotalTrips.ToString(),
                            driver.CompletedTrips.ToString(),
                            $"₱ {driver.TotalEarnings:N2}",
                            $"{driver.AvgDuration:0.0} min",
                            $"{driver.FastestTrip:0.0} min",
                            $"{driver.SlowestTrip:0.0} min"
                        };

                            foreach (var val in cols)
                            {
                                table.Cell()
                                    .Background(bg)
                                    .Padding(5)
                                    .Text(val)
                                    .FontSize(9);
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