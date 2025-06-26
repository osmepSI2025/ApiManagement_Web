namespace SME_WEB_ApiManagement.Models
{
    public class DropdownModels
    {
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
    }
    public class vDropdownDTO
    {
        public string responseCode { get; set; } = string.Empty;
        public string responseDesc { get; set; } = string.Empty;
        //public string StatusUpdate { get; set; } = string.Empty;
        public List<DropdownModels> DropdownList { get; set; } = new List<DropdownModels>();
    }
    public class ddlModelsJoget
    {
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
    }
}
