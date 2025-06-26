using Microsoft.AspNetCore.Mvc;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;
using SME_WEB_ApiManagement.Services;
using System.Diagnostics;
using System.Text.Json;

namespace SME_WEB_ApiManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICallAPIService _callAPIService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        public HomeController(ILogger<HomeController> logger, ICallAPIService callAPIService
            , IConfiguration configuration
            , IWebHostEnvironment webHostEnvironment

            )
        {
            _logger = logger;
            _callAPIService = callAPIService;
            _configuration = configuration;
            API_Path_Main = _configuration.GetValue<string>("API_Path_Main");
            API_Path_Sub = _configuration.GetValue<string>("API_Path_Sub");
            API_Path_Trigger = _configuration.GetValue<string>("API_Path_Trigger");
            API_Path_Sub_Trigger = _configuration.GetValue<string>("API_Path_Sub_Trigger");
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                EmployeeModels model = new EmployeeModels();

                var claimsJson = HttpContext.Session.GetString("UserClaims");
                Dictionary<string, string>? claimsDict = null;
                if (!string.IsNullOrEmpty(claimsJson))
                {
                    claimsDict = JsonSerializer.Deserialize<Dictionary<string, string>>(claimsJson);
                    string GetClaim(Dictionary<string, string>? dict, string key)
                        => dict != null && dict.ContainsKey(key) ? dict[key] : string.Empty;

                    // Use nameidentifier as primary, fallback to userprincipalname, then email
                    string email = GetClaim(claimsDict, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                    model.Email = email;
                    model.EmployeeId = GetClaim(claimsDict, "EmpID");
                    var EmpDetail = await EmployeeDAO.GetDetaiEmp(model, API_Path_Main + API_Path_Sub);

                    if (EmpDetail.EmployeeId !=null) 
                    
                    {
                        var empDetailJson = JsonSerializer.Serialize(EmpDetail);
                        HttpContext.Session.SetString("EmpDetail", empDetailJson);
                        HttpContext.Session.SetString("EmployeeId", EmpDetail.EmployeeId);

                        ViewBag.EmpDetail = EmpDetail;
                        ViewBag.claimsDict = claimsJson;
                    }
             
                    return View();
                }
                else
                {
                    return RedirectToAction("login", "account");
                }
            }
            catch (Exception ex)
            {
                return View();
                //return RedirectToAction("login", "account");
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Admin()
        {
            return View();
        }
    }
}
