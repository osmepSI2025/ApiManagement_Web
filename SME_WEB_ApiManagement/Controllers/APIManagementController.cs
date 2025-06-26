using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;

namespace SME_WEB_ApiManagement.Controllers
{
    public class APIManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<APIManagementController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        public APIManagementController(ILogger<APIManagementController> logger, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
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


        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SubApiManagement()
        {
            return View();
        }
        public IActionResult AgencyCompany(ViewRegisterApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,

            string searchData = null,string clearSearcData=null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null)
        {
            #region panging
            int curpage = 0;
            int totalpage = 0;
            ViewRegisterApiModels result = new ViewRegisterApiModels();

            if (!string.IsNullOrEmpty(hidcurrentpage)) curpage = Convert.ToInt32(hidcurrentpage);
            if (!string.IsNullOrEmpty(hidtotalpage)) totalpage = Convert.ToInt32(hidtotalpage);
            if (!string.IsNullOrEmpty(first)) currentPageNumber = 1;
            else if (!string.IsNullOrEmpty(previous)) currentPageNumber = (curpage == 1) ? 1 : curpage - 1;
            else if (!string.IsNullOrEmpty(next)) currentPageNumber = (curpage == totalpage) ? totalpage : curpage + 1;
            else if (!string.IsNullOrEmpty(last)) currentPageNumber = totalpage;

            int PageSizeDummy = PageSize;
            int totalCount = 0;
            PageSize = PageSizeDummy;
            #endregion
            try
            {
                if (!string.IsNullOrEmpty(searchData))
                {
                    result.LOrg = SystemDAO.GetOrgBySeach(vm.MOrg, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);
                    if (result.LOrg!=null)
                    {
                        totalCount = SystemDAO.GetOrgBySeach(vm.MOrg, API_Path_Main + API_Path_Sub, "Y", currentPageNumber, PageSize, null).Count();
                     

                    }
                    else 
                    {
                        totalCount = 0;
                    }
                    result.TotalRowsList = totalCount;
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(totalCount, currentPageNumber, PageSize);
                }
                else if (!string.IsNullOrEmpty(clearSearcData))
                {
                    return Redirect("AgencyCompany");
                }
                else if (!string.IsNullOrEmpty(saveData))
                {
                    var createOrg = SystemDAO.UpsertOrg(vm.InsMOrg, API_Path_Main + API_Path_Sub, null);
                    return Redirect("AgencyCompany");
                }
                else if (!string.IsNullOrEmpty(editData))
                {
                    var createOrg = SystemDAO.UpsertOrg(vm.InsMOrg, API_Path_Main + API_Path_Sub, null);
                    return Redirect("AgencyCompany");
                }
                else if (!string.IsNullOrEmpty(DeleteData)) 
                {
                    var del = SystemDAO.DeleteOrg(vm.MOrg.OrganizationId.ToString(), API_Path_Main + API_Path_Sub, null);
                    return Redirect("AgencyCompany");
                }
                else if (!string.IsNullOrEmpty(cancelData))
                {

                }
                else
                {

                    result.LOrg = SystemDAO.GetOrgBySeach(vm.MOrg, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);
                    totalCount = SystemDAO.GetOrgBySeach(vm.MOrg, API_Path_Main + API_Path_Sub, "Y", currentPageNumber, PageSize, null).Count();
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(totalCount, currentPageNumber, PageSize);
                    result.TotalRowsList = totalCount;
                }
                #region dropdown 
                result.LSystem = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null);
                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
                result.vDdlOrg = Service_CenterDAO.GetDropdownOrganization(API_Path_Main + API_Path_Sub, null);


                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlOrg = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                //
                #endregion dropdown
                return View(result);
            }
            catch (Exception ex)
            {
                return View(result);
            }

        }


        [HttpPost]
        public JsonResult DeleteAgencyCompany(int id)
        {
            // ลบข้อมูลจากฐานข้อมูลหรือ DAO
            // สมมติ EmployeeDAO.DeleteUserById(id);
            bool result =  SystemDAO.DeleteOrg(id.ToString(), API_Path_Main + API_Path_Sub, null);
            if (result)
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "ไม่พบผู้ใช้งานหรือเกิดข้อผิดพลาด" });
        }
    }
}
