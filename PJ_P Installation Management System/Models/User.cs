namespace PJ_P_Installation_Management_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Admin, Staff
        public bool IsActive { get; set; }
        public string Password { get; set; }


    }

}
