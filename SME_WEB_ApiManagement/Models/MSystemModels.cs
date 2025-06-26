using System.ComponentModel.DataAnnotations;

namespace SME_WEB_ApiManagement.Models
{
    public class MSystemModels
    {
        public int Id { get; set; }
        public string? SystemCode { get; set; }
        public string? SystemName { get; set; }
        public bool FlagActive { get; set; }
        public string? FlagDelete { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsSelected { get; set; }
        public string? OwnerSystemCode { get; set; }
        public int rowOFFSet { get; set; }
        public int rowFetch { get; set; }
        public string? Token { get; set; }
    }
}
