using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;

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
                    result.PageModel = Service_CenterDAO.LoadPagingViewModel( 0, currentPageNumber, PageSize);

                }
                result.TotalRowsList = xlist.TotalRowsList;
            }
            else if (!string.IsNullOrEmpty(clearSearcData)) 
            {
                return Redirect("RegisterList");
            }
            else
            {
               
                var xlist    = SystemDAO.GetRegister(vm, API_Path_Main + API_Path_Sub, "N", currentPageNumber, PageSize, null);

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
            result.vDdlOrg = Service_CenterDAO.GetDropdownOrganizationWithOutData(result.LRegis, API_Path_Main + API_Path_Sub, null);
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
            // 
            try
            {
                if (!string.IsNullOrEmpty(saveData))
                {
                    if (vm.MRegister != null)
                    {
                        UpSertRegisterApiModels um = new UpSertRegisterApiModels();
                        um.MRegister = vm.MRegister;
                        um.LSystem = vm.LSystem;

                        // insert/update TB register
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

                //
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
            // 
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
                                });
                            }
                            um.LPerMapApi = lsysApi;

                        }
                        // insert/update TB register
                        var Upsert = SystemDAO.UpsertRegister(um, API_Path_Main + API_Path_Sub, null);
                        if (Upsert != 0)
                        {
                            return RedirectToAction("RegisterList");
                        }

                    }



                }
                else if (!string.IsNullOrEmpty(OrgCode))
                {
                    // get data by orgcode
                    TApiPermisionMappingModels mo = new TApiPermisionMappingModels();
                    MRegisterModels og = new MRegisterModels();


                    mo.OrganizationCode = OrgCode;

                    result.LApi = SystemDAO.GetTApiMappingBySearch(mo, API_Path_Main + API_Path_Sub, null);

                    if (result.LApi != null)
                    {
                        og.OrganizationCode = OrgCode;
                        og.StartDate = result.LApi[0].StartDate;
                        og.EndDate = result.LApi[0].EndDate;
                        result.MRegister = og;
                    }
                    else 
                    {
                        og.OrganizationCode = OrgCode;
                        og.StartDate = null;
                        og.EndDate = null;
                        result.MRegister = og;
                    }


                }


                #region dropdown 
                //  result.LSystem = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null);

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
        public IActionResult Add(MRegisterModels model)
        {
            if (ModelState.IsValid)
            {
                // เตรียมข้อมูลสำหรับบันทึก
                var upsertModel = new UpSertRegisterApiModels
                {
                    MRegister = model,
                    LSystem = new List<MSystemModels>(),
                    LPerMapApi = new List<TApiPermisionMappingModels>()
                };

                // เรียก DAO เพื่อเพิ่มข้อมูล
                var result = SystemDAO.UpsertRegister(upsertModel, API_Path_Main + API_Path_Sub, null);

                if (result > 0)
                {
                    // เพิ่มสำเร็จ กลับไปหน้ารายการ
                    return RedirectToAction("RegisterList");
                }
                else
                {
                    // เพิ่มไม่สำเร็จ แสดง error
                    ModelState.AddModelError("", "ไม่สามารถเพิ่มข้อมูลได้");
                }
            }
            // ถ้าไม่ผ่าน validation หรือ error ให้กลับไปหน้าเดิม
            return View("RegisterList");
        }


    }

}
