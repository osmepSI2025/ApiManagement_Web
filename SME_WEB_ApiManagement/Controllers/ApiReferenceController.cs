using Microsoft.AspNetCore.Mvc;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;
using SME_WEB_ApiManagement.Services;
using System.Collections.Generic;

namespace SME_WEB_ApiManagement.Controllers
{
    public class ApiReferenceController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiReferenceController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        private readonly CallAPIService _callAPIService;

        public ApiReferenceController(ILogger<ApiReferenceController> logger,
        IConfiguration configuration, IWebHostEnvironment webHostEnvironment
         , CallAPIService callAPIService)

        {
            _logger = logger;
            _configuration = configuration;
            API_Path_Main = _configuration.GetValue<string>("API_Path_Main");
            API_Path_Sub = _configuration.GetValue<string>("API_Path_Sub");
            API_Path_Trigger = _configuration.GetValue<string>("API_Path_Trigger");
            API_Path_Sub_Trigger = _configuration.GetValue<string>("API_Path_Sub_Trigger");
            PageSize = _configuration.GetValue<Int32>("PageSize");
            PageSizMedium = _configuration.GetValue<Int32>("PageSizMedium");
            currentPageNumber = 1;
            _webHostEnvironment = webHostEnvironment;
            _callAPIService = callAPIService;

        }
        public IActionResult Index()
        {
            ViewRegisterApiModels result = new ViewRegisterApiModels();
            try {
                TApiPermisionMappingModels mo = new TApiPermisionMappingModels();
                mo.OrganizationCode = "ORG000001";
                result.LApi = SystemDAO.GetTApiMappingBySearch(mo, API_Path_Main + API_Path_Sub, null);
                // Replace with your actual data source (e.g., from a database)
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ApiReferenceController Index method");
                // Handle the error as needed, e.g., return an error view or message
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
          
        }
        public IActionResult Detail(int id)
        {
            TSystemApiMappingModels mo = new TSystemApiMappingModels();
            mo.Id = id;
            var apiList = SystemDAO.GetTSystemMappingBySearch(mo, API_Path_Main + API_Path_Sub, null);
            var api = apiList?.FirstOrDefault();
            if (api == null)
            {
                return NotFound();
            }
            return View(api);
        }
    }
}