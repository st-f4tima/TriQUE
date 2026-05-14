using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.Reports
{
    public abstract class ReportBase
    {
        protected readonly string GeneratedBy;

        protected ReportBase(string generatedBy)
        {
            GeneratedBy = generatedBy;
        }
        
        public abstract string GeneratePdf();

        // builds file path
        protected string BuildOutputPath(string prefix)
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads",
                $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );
        }

        // format date range in the report header
        protected string FormatDateLabel(DateTime? from, DateTime? to)
        {
            string fromLabel = from == null ? "All Time" : from.Value.ToString("MMM dd, yyyy");
            string toLabel = to == null ? "Present" : to.Value.ToString("MMM dd, yyyy");
            return $"{fromLabel} - {toLabel}";
        }
    }
}
