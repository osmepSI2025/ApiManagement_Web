using System.ComponentModel.DataAnnotations;

namespace SME_WEB_ApiManagement.Models
{
    public class TApiPermisionMappingModels
    {
        public int Id { get; set; }

        public string? OrganizationCode { get; set; }

        public string? SystemCode { get; set; }

        public string? ApiKey { get; set; }

        public string? ApiSystemCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? FlagActive { get; set; }

        public string? FlagDelete { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string? SystemName { get; set; }

        public int? SystemApiMappingId { get; set; }
        public string? ServiceName { get; set; }
        public bool IsSelected { get; set; }

        public string? OrganizationName { get; set; }
        public string? ApiMethod { get; set; }
        public string? ApiUrlUatOundbound { get; set; }
        public string? ApiUrlProdOundbound { get; set; }
        public string? ApiServiceCode { get; set; }
    }
}
