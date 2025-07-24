namespace PJ_P_Installation_Management_System.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportType { get; set; } // Inventory, Financial, Progress
        public DateTime GeneratedOn { get; set; }
        public string FilePath { get; set; }
    }

}
