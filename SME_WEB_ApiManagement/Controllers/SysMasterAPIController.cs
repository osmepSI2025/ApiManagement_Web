using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;
using SME_WEB_ApiManagement.Services;
using System.Diagnostics;

namespace SME_WEB_ApiManagement.Controllers
{
    public class SysMasterAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SysMasterAPIController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        private readonly CallAPIService _callAPIService;
        public SysMasterAPIController(ILogger<SysMasterAPIController> logger,
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
        public IActionResult SysMasterAPI(ViewSystemApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,
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

            ViewSystemApiModels result = new ViewSystemApiModels();
            try
            {
                if (!string.IsNullOrEmpty(searchData))
                {
                    result.LSystem = SystemDAO.GetSystemBySearch(vm.MSystem, API_Path_Main + API_Path_Sub, null);
                    if (result.LSystem != null)
                    {
                        totalCount = SystemDAO.GetSystemBySearch(vm.MSystem, API_Path_Main + API_Path_Sub, "Y", 0, 0, null).Count();
                    }
                    else
                    {
                        totalCount = 0;
                    }

                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(totalCount, currentPageNumber, PageSize);
                }
                else if (!string.IsNullOrEmpty(clearSearcData))
                {
                    return Redirect("SysMasterAPI");
                }
                else if (!string.IsNullOrEmpty(saveData))
                {
                    int? save = SystemDAO.UpsertSystem(vm.InsMSystem, API_Path_Main + API_Path_Sub, null);
                    return Redirect("SysMasterAPI");
                }
                else if (!string.IsNullOrEmpty(DeleteData))
                {
                    var del = SystemDAO.DeleteSystem(vm.MSystem.Id.ToString(), API_Path_Main + API_Path_Sub, null);
                    return Redirect("SysMasterAPI");
                }
                else
                {
                    MSystemModels model = new MSystemModels();
                    model.FlagDelete = "N";

                    result.LSystem = SystemDAO.GetSystemBySearch(model, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);
                    totalCount = SystemDAO.GetSystemBySearch(model, API_Path_Main + API_Path_Sub, "Y", 0, 0, null).Count();
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(totalCount, currentPageNumber, PageSize);
                }

                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);

                var serviceCenter = new ServiceCenter(_configuration, _callAPIService);
                result.vDdlOrg = serviceCenter.GetDdlDepartment(API_Path_Main + API_Path_Sub, "business-units").Result;
                ViewBag.DDLDepartment = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlOrg = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                return View(result);
            }
            catch (Exception ex)
            {
                return View(result);
            }
        }
        public async Task<IActionResult> SubSysMasterAPI(string systemcode)
        {
            var result = new ViewSystemApiModels();
            try
            {
                if (systemcode != "")
                {
                    // get data tsystemlist
                    return View(result);
                }
                else
                {
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                return View(result);
            }


        }
        public IActionResult SysMasterAPIInbound(ViewSystemApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,
    string searchData = null, string clearSearcData = null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null)
        {
            ViewBag.EmployeeId = HttpContext.Session.GetString("EmployeeId");
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

            ViewSystemApiModels result = new ViewSystemApiModels();
            try
            {
                if (!string.IsNullOrEmpty(searchData))
                {
                    result.LSystem = SystemDAO.GetSystemBySearch(vm.MSystem, API_Path_Main + API_Path_Sub, null);
                    if (result.LSystem != null)
                    {
                        totalCount = SystemDAO.GetSystemBySearch(vm.MSystem, API_Path_Main + API_Path_Sub, "Y", 0, 0, null).Count();
                    }
                    else
                    {
                        totalCount = 0;
                    }

                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(totalCount, currentPageNumber, PageSize);
                }
                else if (!string.IsNullOrEmpty(clearSearcData))
                {
                    return Redirect("SysMasterAPIInbound");
                }
                else if (!string.IsNullOrEmpty(saveData))
                {
                    int? save = SystemDAO.UpsertSystem(vm.InsMSystem, API_Path_Main + API_Path_Sub, null);
                    return Redirect("SysMasterAPIInbound");
                }
                else if (!string.IsNullOrEmpty(DeleteData))
                {
                    var del = SystemDAO.DeleteSystem(vm.MSystem.Id.ToString(), API_Path_Main + API_Path_Sub, null);
                    return Redirect("SysMasterAPIInbound");
                }
                else
                {
                    MSystemModels model = new MSystemModels();
                    model.FlagDelete = "N";

                    result.LSystem = SystemDAO.GetSystemBySearch(model, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);
                    totalCount = SystemDAO.GetSystemBySearch(model, API_Path_Main + API_Path_Sub, "Y", 0, 0, null).Count();
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(totalCount, currentPageNumber, PageSize);
                }

                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);

                var serviceCenter = new ServiceCenter(_configuration, _callAPIService);
                result.vDdlOrg = serviceCenter.GetDdlDepartment(API_Path_Main + API_Path_Sub, "business-units").Result;
                ViewBag.DDLDepartment = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlOrg = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                return View(result);
            }
            catch (Exception ex)
            {
                return View(result);
            }
        }

        public IActionResult SysMasterAPIConnectInbound(ViewSystemApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,

            string searchNews = null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null, string SystemCode = null
            , string sortColumn = null, string sortOrder = null, string saveSubData = null)
        {

            ViewBag.EmployeeId = HttpContext.Session.GetString("EmployeeId");
            #region panging
            int curpage = 0;
            int totalpage = 0;
            //set data to view
            ViewSystemApiModels result = new ViewSystemApiModels
            {
                LSysApi = new List<TSystemApiMappingModels>(), // Always initialize to empty list
                InsMSystem = new MSystemModels(), // Initialize to a new instance
                LMSystemInfo = new List<MSystemInfoModels>(), // Initialize to an empty list
                LSystem = new List<MSystemModels>(), // Initialize to an empty list
                MSystem = new MSystemModels(), // Initialize to a new instance
                MSystemInfo = new MSystemInfoModels(), // Initialize to a new instance
                PageModel = new PagingModel() // Initialize to a new instance
                         ,
                TSystemAPI = new TSystemApiMappingModels() // Initialize to a new instance
                         ,
                vDdlStatus = new vDropdownDTO(), // Initialize to a new instance
                vDdlSystem = new vDropdownDTO(), // Initialize to a new instance
                vDdlMethodApi = new vDropdownDTO() // Initialize to a new instance
            };

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
            // 

            try
            {

                if (!string.IsNullOrEmpty(saveData))
                {
                    if (vm.TSystemAPI != null)
                    {
                        MSystemInfoModels um = new MSystemInfoModels();
                        um.Note = vm.TSystemAPI.ApiNote;
                        um.ApiUrlProdInbound = vm.TSystemAPI.ApiUrlProdInbound;
                        um.ApiUrlUatInbound = vm.TSystemAPI.ApiUrlUatInbound;
                        um.ApiUser = vm.TSystemAPI.ApiUser;
                        um.ApiPassword = vm.TSystemAPI.ApiPassword;
                        um.ApiKey = vm.TSystemAPI.ApiKey;
                        um.SystemCode = vm.TSystemAPI.OwnerSystemCode;
                        um.FlagActive = vm.TSystemAPI.FlagActive;
                        um.FlagDelete = "N"; // default N
                        um.CreateBy = ViewBag.EmployeeId;
                        um.UpdateBy = ViewBag.EmployeeId;
                        um.CreateDate = DateTime.Now; // ใช้วันที่ปัจจุบัน

                        var Upsert = SystemDAO.MSystemInfoUpsertSystem(um, API_Path_Main + API_Path_Sub, null);

                        return RedirectToAction("SysMasterAPIConnectInbound", "SysMasterAPI", new { SystemCode = vm.TSystemAPI.OwnerSystemCode });
                    }
                }
               else if (!string.IsNullOrEmpty(saveSubData))
                {
                    if (vm.TSystemAPI != null)
                    {
                        string syscode = vm.MSystemInfo.SystemCode;

                        UpSerTSystemApiMappingModels um = new UpSerTSystemApiMappingModels();
                        um.TSystemAPI = vm.TSystemAPI;
                        // um.LSystem = vm.LSystem;

                        // insert/update TB register
                        var Upsert = SystemDAO.UpsertSystemApi(um, API_Path_Main + API_Path_Sub, null);

                        return RedirectToAction("SysMasterAPIConnectInbound", "SysMasterAPI", new { SystemCode = syscode });
                    }
                }
                else if (!string.IsNullOrEmpty(editData))
                {

                }
                else if (!string.IsNullOrEmpty(cancelData))
                {

                }
                else if ((!string.IsNullOrEmpty(SystemCode)) && (string.IsNullOrEmpty(sortColumn)) && (string.IsNullOrEmpty(sortOrder)))
                {
                    TSystemApiMappingModels ma = new TSystemApiMappingModels();
                    List<TSystemApiMappingModels> xts = new List<TSystemApiMappingModels>();
                    ma.OwnerSystemCode = SystemCode;
                    result.TSystemAPI = ma;
                    // list data Tsystem by Owner

                    xts = SystemDAO.GetTSystemMappingBySearch(ma, API_Path_Main + API_Path_Sub, null);
                    if (xts == null)
                    {
                        xts = new List<TSystemApiMappingModels>();
                    }
                    result.LSysApi = xts;

                    result.MSystemInfo = SystemDAO.GetSystemInfoByCode(SystemCode, API_Path_Main + API_Path_Sub, null);

                }
                else if (((!string.IsNullOrEmpty(sortColumn)) || (!string.IsNullOrEmpty(sortOrder))) && (!string.IsNullOrEmpty(SystemCode)))
                {
                    sortColumn ??= "Id";
                    sortOrder = sortOrder == "asc" ? "desc" : "asc"; // เปลี่ยน asc <-> desc

                    TSystemApiMappingModels ms = new TSystemApiMappingModels();
                    ms.OwnerSystemCode = SystemCode;

                    var data = SystemDAO.GetTSystemMappingBySearch(ms, API_Path_Main + API_Path_Sub, null);


                    var sortedData = sortColumn switch
                    {
                        "Name" => sortOrder == "asc" ? data.OrderBy(x => x.ApiServiceName) : data.OrderByDescending(x => x.ApiServiceName),
                        "CreatedDate" => sortOrder == "asc" ? data.OrderBy(x => x.CreateDate) : data.OrderByDescending(x => x.CreateDate),
                        _ => sortOrder == "asc" ? data.OrderBy(x => x.Id) : data.OrderByDescending(x => x.Id),
                    };
                    result.LSysApi = sortedData.ToList(); // ✅ แปลงเป็น List<T>
                }
                else
                {

                }
                #region dropdown 
                //  result.LSystem = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null);

                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
                result.vDdlSystem = Service_CenterDAO.GetDropdownSystem(API_Path_Main + API_Path_Sub, null);
                result.vDdlMethodApi = Service_CenterDAO.GetLookups("METHODAPI", API_Path_Main + API_Path_Sub, null);


                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlSystem = new SelectList(result.vDdlSystem.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlMethodApi = new SelectList(result.vDdlMethodApi.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                //
                ViewBag.SortColumn = sortColumn;
                ViewBag.SortOrder = sortOrder;
                ViewBag.SystemCode = SystemCode; // ส่งค่า SystemCode ไปที่ View
                #endregion dropdown



                return View(result);
            }
            catch (Exception)
            {
                return View(result);
            }

        }
        public IActionResult SysMasterAPIConnect(ViewSystemApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,

    string searchNews = null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null, string SystemCode = null
    , string sortColumn = null, string sortOrder = null)
        {
            #region panging
            int curpage = 0;
            int totalpage = 0;
            ViewSystemApiModels result = new ViewSystemApiModels();

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
            // 

            try
            {

                if (!string.IsNullOrEmpty(saveData))
                {
                    if (vm.TSystemAPI != null)
                    {
                        UpSerTSystemApiMappingModels um = new UpSerTSystemApiMappingModels();
                        um.TSystemAPI = vm.TSystemAPI;
                        // um.LSystem = vm.LSystem;

                        // insert/update TB register
                        var Upsert = SystemDAO.UpsertSystemApi(um, API_Path_Main + API_Path_Sub, null);

                        return RedirectToAction("SysMasterAPIConnect", "SysMasterAPI", new { SystemCode = vm.TSystemAPI.OwnerSystemCode });
                    }
                }
                else if (!string.IsNullOrEmpty(editData))
                {

                }
                else if (!string.IsNullOrEmpty(cancelData))
                {

                }
                else if ((!string.IsNullOrEmpty(SystemCode)) && (string.IsNullOrEmpty(sortColumn)) && (string.IsNullOrEmpty(sortOrder)))
                {
                    TSystemApiMappingModels ma = new TSystemApiMappingModels();

                    ma.OwnerSystemCode = SystemCode;
                    result.TSystemAPI = ma;
                    // list data Tsystem by Owner

                    result.LSysApi = SystemDAO.GetTSystemMappingBySearch(ma, API_Path_Main + API_Path_Sub, null);
                }
                else if (((!string.IsNullOrEmpty(sortColumn)) || (!string.IsNullOrEmpty(sortOrder))) && (!string.IsNullOrEmpty(SystemCode)))
                {
                    sortColumn ??= "Id";
                    sortOrder = sortOrder == "asc" ? "desc" : "asc"; // เปลี่ยน asc <-> desc

                    TSystemApiMappingModels ms = new TSystemApiMappingModels();
                    ms.OwnerSystemCode = SystemCode;


                    var data = SystemDAO.GetTSystemMappingBySearch(ms, API_Path_Main + API_Path_Sub, null);


                    var sortedData = sortColumn switch
                    {
                        "Name" => sortOrder == "asc" ? data.OrderBy(x => x.ApiServiceName) : data.OrderByDescending(x => x.ApiServiceName),
                        "CreatedDate" => sortOrder == "asc" ? data.OrderBy(x => x.CreateDate) : data.OrderByDescending(x => x.CreateDate),
                        _ => sortOrder == "asc" ? data.OrderBy(x => x.Id) : data.OrderByDescending(x => x.Id),
                    };
                    result.LSysApi = sortedData.ToList(); // ✅ แปลงเป็น List<T>
                }
                else
                {

                }
                #region dropdown 
                //  result.LSystem = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null);

                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
                result.vDdlSystem = Service_CenterDAO.GetDropdownSystem(API_Path_Main + API_Path_Sub, null);
                result.vDdlMethodApi = Service_CenterDAO.GetLookups("METHODAPI", API_Path_Main + API_Path_Sub, null);


                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlSystem = new SelectList(result.vDdlSystem.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlMethodApi = new SelectList(result.vDdlMethodApi.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                //
                ViewBag.SortColumn = sortColumn;
                ViewBag.SortOrder = sortOrder;
                ViewBag.SystemCode = SystemCode; // ส่งค่า SystemCode ไปที่ View
                #endregion dropdown
                return View(result);
            }
            catch (Exception)
            {
                return View(result);
            }

        }

        [HttpPost]
        public JsonResult DeleteSystem(int id)
        {
            // ลบข้อมูลจากฐานข้อมูลหรือ DAO
            // ตัวอย่าง: สมมติว่ามี MSystemDAO.DeleteSystemById
            string result = SystemDAO.DeleteSystem(id.ToString(), API_Path_Main + API_Path_Sub, null);
            if (!string.IsNullOrEmpty(result))
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "ไม่พบระบบหรือเกิดข้อผิดพลาด" });
        }

        [HttpGet]
        public JsonResult GetApiDetail(int id)
        {
            // ดึงข้อมูลจาก DAO ตาม id
            TSystemApiMappingModels xdata = new TSystemApiMappingModels();
            xdata.Id = id;

            var api = SystemDAO.GetTSystemMappingBySearch(xdata, API_Path_Main + API_Path_Sub, null);
            var result = api.FirstOrDefault();
            if (result != null)
                return Json(new
                {
                    id = result.Id,
                    apiMethod = result.ApiMethod,
                    apiServiceName = result.ApiServiceName,
                    apiServiceCode = result.ApiServiceCode,
                    endPoint = result.EndPoint,
                    flagActive = result.FlagActive,
                    // เพิ่ม field สำหรับ request/response
                    apiRequestParamater = result.ApiRequestParamater,
                    apiRequestParamaterType = result.ApiRequestParamaterType,
                    apiRequestDescription = result.ApiRequestDescription,
                    apiRequestExample = result.ApiRequestExample,
                    apiResponseParamater = result.ApiResponseParamater,
                    apiResponseParamaterType = result.ApiResponseParamaterType,
                    apiResponseDescription = result.ApiResponseDescription,
                    apiResponseExample = result.ApiResponseExample
                });
            else
                return Json(new { success = false, message = "ไม่พบข้อมูล" });
        }

        [HttpPost]
        public JsonResult DeleteApiDetail(int id)
        {
            // Fix: Ensure the return type matches the expected 'bool' type
            bool result = SystemDAO.DeleteSystemAPIDetail(id.ToString(), API_Path_Main + API_Path_Sub, null);
            if (result)
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "ไม่พบระบบหรือเกิดข้อผิดพลาด" });
        }
    }
}