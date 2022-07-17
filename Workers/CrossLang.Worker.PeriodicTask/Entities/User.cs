using System;
namespace CrossLang.Worker.PeriodicTask.Entities
{
    public class User
    {
        public long? ID { get; set; }

        public string? Username { get; set; }


        public string? Password { get; set; }


        public string? Avatar { get; set; }

        public string? FullName { get; set; }


        public DateTime? DateOfBirth { get; set; }


        public string? Email { get; set; }


        public string? PhoneNumber { get; set; }


        public string? Address { get; set; }


        public long? RoleID { get; set; }


        public long? UserPermission { get; set; }

        public int? Package { get; set; }


        public string? EmployeeCode { get; set; }


        public bool? IsEmployee { get; set; }


        public DateTime? ExpDate { get; set; }

        public bool? IsTrialUsed { get; set; }
    }
}

