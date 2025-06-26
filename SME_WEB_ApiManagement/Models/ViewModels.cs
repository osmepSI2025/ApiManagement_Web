namespace SME_WEB_ApiManagement.Models
{
    public class ViewRegisterApiModels
    {
        public vDropdownDTO vDdlSystem { get; set; } = new vDropdownDTO();
        public vDropdownDTO vDdlOrg { get; set; } = new vDropdownDTO();
        public vDropdownDTO vDdlStatus { get; set; } = new vDropdownDTO();
        public PagingModel PageModel { get; set; } = new PagingModel();
        public MRegisterModels MRegister { get; set; } = new MRegisterModels();
        public List<TApiPermisionMappingModels> LApi { get; set; } = new List<TApiPermisionMappingModels>();
        public List<TApiPermisionMappingModels> LPerMapApiSelect { get; set; } = new List<TApiPermisionMappingModels>();
        public List<MSystemModels> LSystem { get; set; } = new List<MSystemModels>();
        public List<MRegisterModels> LRegis { get; set; } = new List<MRegisterModels>();
        public int? TotalRowsList { get; set; } = new int?();
        public MOrganizationModels MOrg { get; set; } = new MOrganizationModels();
        public List<MOrganizationModels> LOrg { get; set; } = new List<MOrganizationModels>();
        public MOrganizationModels InsMOrg { get; set; } = new MOrganizationModels();
    }
    public class UpSertRegisterApiModels
    {
     
        public MRegisterModels MRegister { get; set; }

        public List<MSystemModels> LSystem { get; set; }

        public List<TApiPermisionMappingModels> LPerMapApi { get; set; }
      

    }
    public class viewRegisterModels
    {
        public List<MRegisterModels> LRegister { get; set; } = new List<MRegisterModels>();
        public MRegisterModels MRegisterModels { get; set; } = new MRegisterModels();

        public int? TotalRowsList { get; set; }
        public PagingModel PageModel { get; set; }
    }
}
