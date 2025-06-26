using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;
using SME_WEB_ApiManagement.Services;

namespace SME_WEB_ApiManagement.Controllers
{
    public class ErrorAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ErrorAPIController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        private readonly CallAPIService _callAPIService;
        public ErrorAPIController(ILogger<ErrorAPIController> logger,
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
        public IActionResult Index(ViewErroApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,
            string searchData = null, string clearSearcData = null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null)
        {
            #region panging
            int curpage = 0;
            int totalpage = 0;

            if (!string.IsNullOrEmpty(hidcurrentpage)) curpage = Convert.ToInt32(hidcurrentpage);
            if (!string.IsNullOrEmpty(hidtotalpage)) totalpage = Convert.ToInt32(hidtotalpage);
            if (!string.IsNullOrEmpty(first)) currentPageNumber = 1;
            else if (!string.IsNullOrEmpty(previous)) currentPageNumber = (curpage == 1) ? 1 : curpage - 1;
            else if (!string.IsNullOrEmpty(next)) currentPageNumber = (curpage == totalpage) ? totalpage : curpage + 1;
            else if (!string.IsNullOrEmpty(last)) currentPageNumber = totalpage;

            int PageSizeDummy = PageSize;
            int totalCount = 0;
            PageSize = PageSizeDummy;
            #endregion End panging

            ViewErroApiModels result = new ViewErroApiModels();
            try
            {
                if (!string.IsNullOrEmpty(searchData))
                {
                   var lerror = ErrorApiDAO.GetErrorApiBySearch(vm.ErrorModel, API_Path_Main + API_Path_Sub, null);
                    result.LError = lerror.LError;

                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(lerror.totalList??0, currentPageNumber, PageSize);
                    result.totalList = lerror.totalList ?? 0;
                }
                else 
                {
                    TErrorApiLogModels model = new TErrorApiLogModels();

                    var lerror = ErrorApiDAO.GetErrorApiBySearch(model, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);
                    result.LError = lerror.LError;
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(lerror.totalList ?? 0, currentPageNumber, PageSize);
                    result.totalList = lerror.totalList ?? 0;


                }

 
                result.LSystem = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null);

                var serviceCenter = new ServiceCenter(_configuration, _callAPIService);
            
              
                return View(result);
            }
            catch (Exception ex)
            {
                return View(result);
            }
        }


        [HttpPost]
        public JsonResult DeleteSystem(int id)
        {
            // ลบข้อมูลจากฐานข้อมูลหรือ DAO
            // ตัวอย่าง: สมมติว่ามี ErrorModelDAO.DeleteSystemById
            string result = ErrorApiDAO.DeleteSystem(id.ToString(), API_Path_Main + API_Path_Sub, null);
            if (!string.IsNullOrEmpty(result))
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "ไม่พบระบบหรือเกิดข้อผิดพลาด" });
        }

    }
}
