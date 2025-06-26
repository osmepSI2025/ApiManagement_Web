namespace SME_WEB_ApiManagement.Models
{
    public class PagingModel
    {
        public int? PageSize { get; set; }
        public int? CurrentPageNumber { get; set; }
        public double? TotalPage { get; set; }

        public int? TotalRows { get; set; }

    }
}
