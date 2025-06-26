using System.ComponentModel.DataAnnotations;

namespace SME_WEB_ApiManagement.Models
{
    public class MOrganizationModels
    {
        public int OrganizationId { get; set; }

        public string? OrganizationCode { get; set; }

        public string? OrganizationName { get; set; }

        public string? Description { get; set; }

        public string? ParentOrganizationId { get; set; }

        public string? TaxId { get; set; }

        public string? IndustryType { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? StateOrProvince { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? WebsiteUrl { get; set; }

        public string? LogoUrl { get; set; }

        public string? Status { get; set; }

        public bool FlagActive { get; set; }

        public string? FlagDelete { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int rowOFFSet { get; set; }
        public int rowFetch { get; set; }
        public string? Token { get; set; }
    }
}
