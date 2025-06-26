namespace SME_WEB_ApiManagement.Models
{
    public class ViewSystemApiModels
    {
        public vDropdownDTO vDdlSystem { get; set; } = new vDropdownDTO();
        public vDropdownDTO vDdlMethodApi { get; set; } = new vDropdownDTO();    
        public vDropdownDTO vDdlStatus { get; set; } = new vDropdownDTO();
        public vDropdownDTO vDdlOrg { get; set; } = new vDropdownDTO();
        public PagingModel PageModel { get; set; } = new PagingModel();
        public TSystemApiMappingModels TSystemAPI { get; set; } = new TSystemApiMappingModels();
        public List<TSystemApiMappingModels> LSysApi { get; set; } = new List<TSystemApiMappingModels>();
        public List<MSystemModels> LSystem { get; set; } = new List<MSystemModels>();
        public MSystemModels MSystem { get; set; } = new MSystemModels();
        public MSystemModels InsMSystem { get; set; } = new MSystemModels();

        public MSystemInfoModels MSystemInfo { get; set; } = new MSystemInfoModels();
        public List<MSystemInfoModels>  LMSystemInfo { get; set; } = new List<MSystemInfoModels>();
    }
    public class UpSerTSystemApiMappingModels
    {

        public TSystemApiMappingModels TSystemAPI { get; set; }
        public MSystemModels InsMSystem { get; set; }
        //   public List<MSystemModels> LSystem { get; set; }

    }
}
