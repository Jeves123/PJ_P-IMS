namespace PJ_P_Installation_Management_System.Models
{
    public class Installation
    {
        public int InstallationId { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } // In Progress, Completed

        public int StaffId { get; set; }
        public Staff Staff { get; set; }
    }

}
