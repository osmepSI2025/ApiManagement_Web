namespace SME_WEB_ApiManagement.Models
{
    public class ViewMOrganizationModels
    {
        public vDropdownDTO vDdlSystem { get; set; }
        public vDropdownDTO vDdlOrg { get; set; }
        public vDropdownDTO vDdlStatus { get; set; }
        public PagingModel PageModel { get; set; }
        public MOrganizationModels MOrg { get; set; }
        public List<MOrganizationModels> LApi { get; set; }
    }
}
