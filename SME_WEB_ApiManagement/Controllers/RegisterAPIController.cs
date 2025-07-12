using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;
using System.Reflection;

namespace SME_WEB_ApiManagement.Controllers
{
    public class RegisterAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterAPIController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        public RegisterAPIController(ILogger<RegisterAPIController> logger, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
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
        public IActionResult RegisterList(ViewRegisterApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,
            string searchDate = null, string clearSearcData = null)
        {
            ViewBag.EmployeeId = HttpContext.Session.GetString("EmployeeId");
            ViewBag.EmployeeRole = HttpContext.Session.GetString("EmployeeRole");
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
            if (!string.IsNullOrEmpty(searchDate))
            {
                var xlist = SystemDAO.GetRegister(vm, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);

                result.LRegis = xlist.LRegis;
                if (result.LRegis != null)
                {
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(xlist.TotalRowsList ?? 0, currentPageNumber, PageSize);
                }
                else
                {
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(0, currentPageNumber, PageSize);
                }
                result.TotalRowsList = xlist.TotalRowsList;
            }
            else if (!string.IsNullOrEmpty(clearSearcData))
            {
                return Redirect("RegisterList");
            }
            else
            {
                var xlist = SystemDAO.GetRegister(vm, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);

                result.LRegis = xlist.LRegis;
                if (result.LRegis != null)
                {
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(xlist.TotalRowsList ?? 0, currentPageNumber, PageSize);
                }
                else
                {
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel(0, currentPageNumber, PageSize);
                }
                result.TotalRowsList = xlist.TotalRowsList;
            }
            result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
            result.vDdlOrg = Service_CenterDAO.GetDropdownOrganization(API_Path_Main + API_Path_Sub, null);

            ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
            ViewBag.vDdlOrg = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

            return View(result);
        }
        public IActionResult RegisterAPI(ViewRegisterApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,
            string searchNews = null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null)
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
                if (!string.IsNullOrEmpty(saveData))
                {
                    if (vm.MRegister != null)
                    {
                        UpSertRegisterApiModels um = new UpSertRegisterApiModels();
                        um.MRegister = vm.MRegister;
                        um.LSystem = vm.LSystem;

                        var Upsert = SystemDAO.UpsertRegister(um, API_Path_Main + API_Path_Sub, null);
                    }
                }
                else if (!string.IsNullOrEmpty(editData))
                {

                }
                else if (!string.IsNullOrEmpty(cancelData))
                {

                }
                else
                {

                }
                #region dropdown 
                result.LSystem = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null);
                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
                result.vDdlOrg = Service_CenterDAO.GetDropdownOrganization(API_Path_Main + API_Path_Sub, null);

                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlOrg = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                #endregion dropdown
                return View(result);
            }
            catch (Exception ex)
            {
                return View(result);
            }
        }
        public IActionResult RegisterAPIDetail(ViewRegisterApiModels vm, string previous, string first, string next, string last, string hidcurrentpage, string hidtotalpage,
            string searchNews = null, string DeleteData = null, string saveData = null, string cancelData = null, string editData = null, string OrgCode = null)
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
            ViewBag.UserRole = HttpContext.Session.GetString("EmployeeRole");
            ViewBag.EmployeeId = HttpContext.Session.GetString("EmployeeId");
            try
            {
                if (!string.IsNullOrEmpty(saveData))
                {
                    if (vm.MRegister != null)
                    {
                        UpSertRegisterApiModels um = new UpSertRegisterApiModels();
                        um.MRegister = vm.MRegister;
                        um.LSystem = new List<MSystemModels>();
                        if (vm.LApi != null)
                        {
                            List<TApiPermisionMappingModels> lsysApi = new List<TApiPermisionMappingModels>();
                            foreach (var item in vm.LApi)
                            {
                                lsysApi.Add(new TApiPermisionMappingModels
                                {
                                    SystemCode = item.SystemCode,
                                    IsSelected = item.IsSelected,
                                    OrganizationCode = item.OrganizationCode,
                                    ApiKey = item.ApiKey,
                                    ApiSystemCode = item.ApiSystemCode,
                                    StartDate = item.StartDate,
                                    EndDate = item.EndDate,
                                    SystemApiMappingId = item.SystemApiMappingId,
                                    Note = vm.MRegister.Note,
                                    RegisterId = vm.MRegister.Id,
                                });
                            }
                            um.LPerMapApi = lsysApi;
                        }
                        var Upsert = SystemDAO.UpsertRegister(um, API_Path_Main + API_Path_Sub, null);
                        if (Upsert != 0)
                        {
                            return RedirectToAction("RegisterList");
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(OrgCode))
                {
                    TApiPermisionMappingModels mo = new TApiPermisionMappingModels();
                    MRegisterModels og = new MRegisterModels();

                    ViewRegisterApiModels searchCode = new ViewRegisterApiModels();

                    mo.OrganizationCode = OrgCode;
                    og.OrganizationCode = OrgCode;
                    og.FlagDelete = "N";
                    searchCode.MRegister = og;
                    var xlist = SystemDAO.GetRegister(searchCode, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);

                    if (xlist.LRegis != null && xlist.LRegis.Count > 0)
                    {
                        og.OrganizationCode = OrgCode;
                        og.StartDate = xlist.LRegis[0].StartDate;
                        og.EndDate = xlist.LRegis[0].EndDate;
                        og.Note = xlist.LRegis[0].Note;
                        og.Id = xlist.LRegis[0].Id;
                        result.MRegister = og;
                    }
                    else
                    {
                        og.OrganizationCode = OrgCode;
                        og.StartDate = null;
                        og.EndDate = null;
                        og.Id = 0;
                        result.MRegister = og;
                    }
                    result.LApi = SystemDAO.GetTApiMappingBySearch(mo, API_Path_Main + API_Path_Sub, null);
                }
                #region dropdown 
                result.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
                result.vDdlOrg = Service_CenterDAO.GetDropdownOrganization(API_Path_Main + API_Path_Sub, null);

                ViewBag.vDdlStatus = new SelectList(result.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
                ViewBag.vDdlOrg = new SelectList(result.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

                #endregion dropdown
                return View(result);
            }
            catch (Exception ex)
            {
                return View(result);
            }
        }
        [HttpPost]
        public IActionResult Add(MRegisterModels model)
        {
            var viewModel = new ViewRegisterApiModels();

            ViewRegisterApiModels searchCode = new ViewRegisterApiModels();
            TApiPermisionMappingModels mo = new TApiPermisionMappingModels();
            MRegisterModels og = new MRegisterModels();

            og.FlagDelete = "N";
            og.OrganizationCode = model.OrganizationCode;
            searchCode.MRegister = og;
            var xlist = SystemDAO.GetRegister(searchCode, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);

            if (xlist.LRegis.Count > 0)
            {
              // ModelState.AddModelError("", "ไม่สามารถเพิ่มข้อมูลได้ ข้อมูลซ้ำ");
                ViewBag.PopupError = "ไม่สามารถเพิ่มข้อมูลได้ ข้อมูลซ้ำ";
                ViewBag.EmployeeRole = HttpContext.Session.GetString("EmployeeRole");
                ViewBag.EmployeeId = HttpContext.Session.GetString("EmployeeId");
            }
            else
            {
                var upsertModel = new UpSertRegisterApiModels
                {
                    MRegister = model,
                    LSystem = new List<MSystemModels>(),
                    LPerMapApi = new List<TApiPermisionMappingModels>()
                };

                var result = SystemDAO.UpsertRegister(upsertModel, API_Path_Main + API_Path_Sub, null);

                if (result > 0)
                {
                    return RedirectToAction("RegisterList");
                }
                else
                {
                  //  ModelState.AddModelError("", "ไม่สามารถเพิ่มข้อมูลได้");
                    ViewBag.PopupError = "ไม่สามารถเพิ่มข้อมูลได้";
                }
            }

            viewModel.vDdlStatus = Service_CenterDAO.GetLookups("STATUS", API_Path_Main + API_Path_Sub, null);
            viewModel.vDdlOrg = Service_CenterDAO.GetDropdownOrganization(API_Path_Main + API_Path_Sub, null);
            ViewBag.vDdlStatus = new SelectList(viewModel.vDdlStatus.DropdownList.OrderBy(x => x.Code), "Code", "Name");
            ViewBag.vDdlOrg = new SelectList(viewModel.vDdlOrg.DropdownList.OrderBy(x => x.Code), "Code", "Name");

            var listResult = SystemDAO.GetRegister(new ViewRegisterApiModels(), API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);
            viewModel.LRegis = listResult.LRegis;
            viewModel.PageModel = Service_CenterDAO.LoadPagingViewModel(listResult.TotalRowsList ?? 0, currentPageNumber, PageSize);
            viewModel.TotalRowsList = listResult.TotalRowsList;
      
            return View("RegisterList", viewModel);
        }
        [HttpPost]
        public JsonResult UpdateStatus(int id, bool flagActive)
        {
            var upsertModel = new MRegisterModels
            {
                Id = id,
                FlagActive = flagActive
            };
            var result = SystemDAO.UpdateStatusRegister(upsertModel, API_Path_Main + API_Path_Sub, null);

            return Json(new { success = true, message = "บันทึกข้อมูลสำเร็จ" });
        }
        [HttpPost]
        public JsonResult DeleteRegister(int id)
        {
            string result = SystemDAO.DeleteRegister(id.ToString(), API_Path_Main + API_Path_Sub, null);
            if (!string.IsNullOrEmpty(result))
                return Json(new { success = true, message = "ลบข้อมูลเรียบร้อยแล้ว" });
            else
                return Json(new { success = false, message = "ไม่พบระบบหรือเกิดข้อผิดพลาด" });
        }
    }
}
