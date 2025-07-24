namespace PJ_P_Installation_Management_System.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TaskDescription { get; set; }

        public int StaffId { get; set; }
        public Staff Staff { get; set; }
    }

}
