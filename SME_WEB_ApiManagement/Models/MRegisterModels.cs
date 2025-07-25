﻿using System.ComponentModel.DataAnnotations;

namespace SME_WEB_ApiManagement.Models
{
    public class MRegisterModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "กรุณาระบุรหัสหน่วยงาน")]
        public string OrganizationCode { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? FlagActive { get; set; }
        public string? FlagDelete { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? OrganizationName { get; set; }
        public string? ApiKey { get; set; }
        public int rowOFFSet { get; set; }
        public int rowFetch { get; set; }
        public string? Token { get; set; }
        public string? Note { get; set; }
    }
}
