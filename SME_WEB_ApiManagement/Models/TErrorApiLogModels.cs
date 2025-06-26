namespace SME_WEB_ApiManagement.Models
{
    public class TErrorApiLogModels
    {
        public int Id { get; set; }

        public string? SystemCode { get; set; }

        public string? Message { get; set; }

        public string? StackTrace { get; set; }

        public string? Source { get; set; }

        public string? TargetSite { get; set; }

        public DateTime? ErrorDate { get; set; }

        public string? UserName { get; set; }

        public string? Path { get; set; }

        public string? HttpMethod { get; set; }

        public string? RequestData { get; set; }

        public string? InnerException { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? Createdate { get; set; }
        public int rowOFFSet { get; set; }
        public int rowFetch { get; set; }

        public string? SystemName { get; set; }

        public string? ApiKey { get; set; }
        public string? HttpCode { get; set; }
    }

    public class ViewErroApiModels
    {
        public vDropdownDTO vDdlSystem { get; set; } = new vDropdownDTO();
      
        public PagingModel PageModel { get; set; } = new PagingModel();
        public List<MSystemModels> LSystem { get; set; } = new List<MSystemModels>();
        public List<TErrorApiLogModels> LError { get; set; } = new List<TErrorApiLogModels>();
        public TErrorApiLogModels ErrorModel { get; set; } = new TErrorApiLogModels();
        public int? totalList { get; set; }

    }
}
