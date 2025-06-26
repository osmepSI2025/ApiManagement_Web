namespace SME_WEB_ApiManagement.Models
{
    public class MSystemInfoModels
    {
        public int Id { get; set; }

        public string? SystemCode { get; set; }

        public bool? FlagActive { get; set; }

        public string? FlagDelete { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? Note { get; set; }

        public string? ApiUrlProdInbound { get; set; }

        public string? ApiUrlUatInbound { get; set; }

        public string? ApiUser { get; set; }

        public string? ApiPassword { get; set; }

        public string? ApiKey { get; set; }
    }
}
