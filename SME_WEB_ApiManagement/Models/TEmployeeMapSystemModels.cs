namespace SME_WEB_ApiManagement.Models
{
    public class TEmployeeMapSystemModels
    {
        public int Id { get; set; }

        public int? SystemApiId { get; set; }

        public string? SystemApiName { get; set; }

        public string? EmployeeId { get; set; }

        public bool? FlagActive { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
    public class UserSystemSettingViewModel
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string? FirstNameTh { get; set; }
        public string? LastNameTh { get; set; }
        public string? PositionName { get; set; }
        public string? BusinessUnitName { get; set; }

        // รายการระบบทั้งหมด
        public List<TEmployeeMapSystemModels> AllSystems { get; set; } = new();

        // รายการ SystemApiId ที่ผู้ใช้ดูแล (จาก TEmployeeMapSystemModels)
        public List<int> SelectedSystemIds { get; set; } = new();
    }
}
