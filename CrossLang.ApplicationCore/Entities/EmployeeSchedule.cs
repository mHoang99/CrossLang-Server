using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("employee_schedule")]
    class EmployeeSchedule : BaseEntity
    {
        [DBColumn]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
        
        [DBColumn]
        [DisplayName("Nhân viên")]
        public long? EmployeeID { get; set; }

        [DBColumn]
        [DisplayName("Học sinh")]
        public long? StudentID { get; set; }

        [DBColumn]
        [DisplayName("Thời gian bắt đầu")]
        public DateTime? StartTime { get; set; }

        [DBColumn]
        [DisplayName("Thời gian kết thúc")]
        public DateTime? EndTime { get; set; }
    }
}
